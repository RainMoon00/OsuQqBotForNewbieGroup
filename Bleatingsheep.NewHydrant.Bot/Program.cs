﻿using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bleatingsheep.NewHydrant.Core;
using Bleatingsheep.NewHydrant.Osu;
using Bleatingsheep.NewHydrant.Osu.Newbie;
using Bleatingsheep.OsuQqBot.Database.Execution;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NLog;
using PuppeteerSharp;
using Sisters.WudiLib;
using Sisters.WudiLib.Posts;
using Sisters.WudiLib.WebSocket;
using Sisters.WudiLib.WebSocket.Reverse;

namespace Bleatingsheep.NewHydrant
{
    internal static class Program
    {
        private static readonly HardcodedConfigure s_hardcodedConfigure = new HardcodedConfigure();

        private static int s_connectedClinetCount = 0;

        private static void Main(string[] args)
        {
            var cultureInfo = CultureInfo.GetCultureInfo("zh-CN");
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            // 配置 osu
            OsuFunction.SetApiKey(s_hardcodedConfigure.ApiKey);

            try
            {
                // 本应在此设置 HttpApiClient，并启动，但是使用反向 WebSocket 时，无需手动这么做。
#if DEBUG
                var rServer = new ReverseWebSocketServer("http://localhost:9191");
#else
                var rServer = new ReverseWebSocketServer(s_hardcodedConfigure.ServerPort);
#endif
                rServer.SetAuthentication(r =>
                {
                    LogManager.LogFactory.GetLogger("Replica").Info($"{r.Headers["X-Forwarded-For"]} 尝试连接。");
                    return ReverseWebSocketServer.CreateAuthenticationFunction(s_hardcodedConfigure.ServerAccessToken, null)(r);
                });
                rServer.ConfigureListener((l, selfId) =>
                {
                    var logger = LogManager.LogFactory.GetLogger("Replica");
                    logger.Info($"客户端 {selfId} 成功连接。");
                    Hydrant hydrant = null;
                    try
                    {
                        hydrant = ConfigureHost(l.ApiClient, l, typeof(Highlight).Assembly);
                        hydrant.Start();
                        Console.WriteLine("Running...");
                        var count = Interlocked.Increment(ref s_connectedClinetCount);
                        logger.Info($"当前已有 {count} 个分身在连接。");
                        l.SocketDisconnected += () =>
                        {
                            Console.WriteLine("Disconnected.");
                            logger.Info("Disconnected");
                            Interlocked.Decrement(ref s_connectedClinetCount);
                            (hydrant as IDisposable)?.Dispose();
                        };
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        (hydrant as IDisposable)?.Dispose();
                    }
                });
                rServer.Start();

                Task.Delay(-1).Wait();
            }
            catch (Exception e)
            {
                var logPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "fatal.log");
                new Logging.FileLogger(logPath).LogException(e);
                LogManager.Shutdown();

                Console.WriteLine(e);
            }

            // 阻止自动重启
            Task.Delay(-1).Wait();
        }

        private static Hydrant ConfigureHost(HttpApiClient httpApiClient, ApiPostListener apiPostListener, params Assembly[] assemblies)
        {
            System.Console.WriteLine("开始配置消防栓。");
            var hydrant = new Hydrant(httpApiClient, apiPostListener, assemblies)
                .AddLogger(LogManager.LogFactory);

            System.Console.WriteLine("正在配置消防栓。(50%)");
            // 设置异常处理。
            hydrant.ExceptionCaught_Command += Hydrant_ExceptionCaught_Command;

            hydrant.Init<HydrantStartup>(new HydrantStartup());

            // 添加必要的事件处理。
            // Public
            apiPostListener.GroupRequestEvent += (api, e) => e.UserId == s_hardcodedConfigure.SuperAdmin ? new GroupRequestResponse { Approve = true } : null;
            apiPostListener.GroupInviteEvent += (api, e) => e.UserId == s_hardcodedConfigure.SuperAdmin ? new GroupRequestResponse { Approve = true } : null;

            // Private-only.
            //apiPostListener.FriendRequestEvent += ApiPostListener.ApproveAllFriendRequests;
            //apiPostListener.GroupInviteEvent += (api, e) => new GroupRequestResponse { Approve = true };
            //apiPostListener.GroupRequestEvent += hydrant.CreateServiceInstance<NotifyOnJoinRequest>().Monitor;

            Console.WriteLine("init complete.");
            return hydrant;
        }

        private static async Task Hydrant_ExceptionCaught_Command(string funcName, Exception exception, HttpApiClient api, Sisters.WudiLib.Posts.Message message)
        {
            if (exception is DatabaseFailException e)
            {
                await api.SendMessageAsync(
                    endpoint: message.Endpoint,
                    message: e.Message ?? (e.InnerException is DbUpdateConcurrencyException ? "数据库太忙。" : "无法访问数据库。")
                );
                Logging.FileLogger.Default.LogException(e);
            }
            else if (exception is MySqlException)
            {
                await api.SendMessageAsync(message.Endpoint, "无法访问 MySQL 数据库。");
            }
            else if (exception is ApiAccessException)
            {
                // 酷 Q 失败。
            }
            else if (exception is NavigationException)
            {
                await api.SendMessageAsync(message.Endpoint, "无法访问对应网站。").ConfigureAwait(false);
            }
            else
            {
                await api.SendMessageAsync(message.Endpoint, "有一些不好的事发生了。");
                Logging.FileLogger.Default.LogException(exception);
            }
        }
    }
}
