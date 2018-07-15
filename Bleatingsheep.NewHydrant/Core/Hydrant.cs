﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bleatingsheep.NewHydrant.Attributions;
using Bleatingsheep.NewHydrant.Logging;
using Bleatingsheep.OsuMixedApi;
using Bleatingsheep.OsuMixedApi.MotherShip;
using Bleatingsheep.OsuQqBot.Database.Execution;
using Sisters.WudiLib;
using Sisters.WudiLib.Posts;

namespace Bleatingsheep.NewHydrant.Core
{
    public sealed class Hydrant
    {
        private readonly HttpApiClient _qq;
        private readonly ApiPostListener _listener;
        private readonly IConfigure _configure;
        private readonly INewbieDatabase _database;
        private readonly ExecutingInfo _executingInfo;
        private readonly ILogger _logger;
        private long SuperAdmin => _configure.SuperAdmin;

        public Hydrant(IConfigure configure, HttpApiClient httpApiClient, ApiPostListener listener)
        {
            _qq = httpApiClient;
            _listener = listener;
            _configure = configure;

            _database = new NewbieDatabase();
            _executingInfo = new ExecutingInfo
            {
                Database = _database,
                Qq = _qq,
                OsuApi = OsuApiClient.ClientUsingKey(_configure.ApiKey),
                MotherShipApi = new MotherShipApiClient(MotherShipApiClient.BleatingsheepCdnHost),
            };

            // 配置日志
            var executingFile = Assembly.GetExecutingAssembly().Location;
            var logFile = Path.Combine(Path.GetDirectoryName(executingFile), "log.txt");
            _logger = new FileLogger(logFile);

            Init();
        }

        #region 执行期间各种事件处理器集合
        private readonly IList<IInitializable> _initializableList = new List<IInitializable>();
        private readonly IList<IMessageCommand> _messageCommandList = new List<IMessageCommand>();
        private readonly IList<IMessageMonitor> _messageMonitorList = new List<IMessageMonitor>();
        #endregion

        private void Init()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes<FunctionAttribute>().Any());

            types.ForEach(InitType);

            _listener.MessageEvent += async (api, message) =>
            {
                await _messageMonitorList.ForEachAsync(async m =>
                {
                    try
                    {
                        await m.OnMessageAsync(message, api);
                    }
                    catch (Exception e)
                    {
                        _logger.LogException(e);
                    }
                });
            };
            _listener.MessageEvent += (api, message) =>
            {
                try
                {
                    _messageCommandList
                        .Select(c => c.Create())
                        .FirstOrDefault(c => c.ShouldResponse(message))
                        ?.ProcessAsync(message, api, _executingInfo)
                        .Wait();
                }
                catch (Exception e) when (!(e is ApiAccessException))
                {
                    api.SendMessageAsync(message.Endpoint, "有一些不好的事发生了").Wait();
                    _logger.LogException(e);
                }
            };

            // 添加必要的事件处理。
            _listener.FriendRequestEvent += ApiPostListener.ApproveAllFriendRequests;
            _listener.GroupRequestEvent += (api, e) => e.UserId == SuperAdmin ? new GroupRequestResponse { Approve = true } : null;
            _listener.GroupInviteEvent += (api, e) => e.UserId == SuperAdmin ? new GroupRequestResponse { Approve = true } : null;
            _listener.GroupAddedEvent += (api, e) => api.SetGroupCard(e.GroupId, e.SelfId, _configure.Name).Wait();
        }

        internal void InitType(Type t)
        {
            var interfaces = t.GetInterfaces();
            var lazy = new Lazy<object>(
                valueFactory: () => Assembly.GetAssembly(t).CreateInstance(t.FullName),
                mode: LazyThreadSafetyMode.None
            );
            Array.ForEach(interfaces, i => InitInterface(i, lazy));
        }

        internal void InitInterface(Type t, Lazy<object> lazy)
        {
            if (t == typeof(IInitializable))
            {
                var init = lazy.Value as IInitializable;

                var success = init.InitializeAsync().Result;
                if (!success) throw new AggregateException();

                if (!string.IsNullOrEmpty(init.Name)) _initializableList.Add(init);
            }
            if (t == typeof(IMessageCommand))
            {
                _messageCommandList.Add(lazy.Value as IMessageCommand ?? throw new InvalidCastException());
            }
            if (t == typeof(IMessageMonitor))
            {
                _messageMonitorList.Add(lazy.Value as IMessageMonitor ?? throw new InvalidCastException());
            }
        }
    }
}
