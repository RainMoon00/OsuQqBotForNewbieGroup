﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bleatingsheep.NewHydrant.Attributions;
using Bleatingsheep.NewHydrant.Core;
using Bleatingsheep.OsuMixedApi;
using Sisters.WudiLib;

namespace Bleatingsheep.NewHydrant.Osu
{
    [Function("newly_ranked")]
    internal class NewlyRankedBeatmapNotify : IRegularAsync
    {
        public TimeSpan? OnUtc => null;
        public TimeSpan? Every { get; } = new TimeSpan(0, 15, 0);

        private static IEnumerable<long> s_groups = new List<long>
        {
            72318078,
            672076603, // HTTP 机器人。
            514661057, // 后花园。
            308419061, // Steam。
        };

        private readonly object _thisLock = new object();
        private IEnumerable<int> _oldSets;
        private DateTimeOffset _noBeatmapAfter = DateTimeOffset.MinValue;

        public async Task RunAsync(HttpApiClient api, ExecutingInfo executingInfo)
        {
            IEnumerable<BloodcatBeatmapSet> newSets = null;
            var result = await BloodcatApi.Client.SearchRankedByKeywordAsync();
            lock (_thisLock)
            {
                //var logger = executingInfo.Logger;
                //logger.LogInBackground($"取到 {result.Count()} 个");
                //logger.LogInBackground($"日期晚于最新：{result.Where(s => s.ApprovedDateOffset > _noBeatmapAfter).Count()} 个");
                if (!(_oldSets is null))
                {
                    //logger.LogInBackground($"提取前 {_oldSets.Count()} 个");
                    newSets = result.Take(_oldSets.Count()).Concat(result.Where(s => s.ApprovedDateOffset > _noBeatmapAfter))
                        .Where(s => !_oldSets.Contains(s.Id))
                        .Distinct()
                        .ToList();// 避免延迟求值
                    //logger.LogInBackground($"发送 {newSets.Count()} 个");
                }
                _oldSets = result.Select(s => s.Id).ToHashSet();
                if (_noBeatmapAfter < result.Max(s => s.ApprovedDateOffset))
                {
                    _noBeatmapAfter = result.Max(s => s.ApprovedDateOffset);
                    //logger.LogInBackground($"日期更新为 {_noBeatmapAfter}");
                }
            }
            var osu = executingInfo.OsuApi;
            if (newSets != null)
            {
                var qq = executingInfo.Qq;
                const int limit = 5;
                var messages = newSets.Take(limit).Select(s => BloodcatUtilities.GetMusicMessage(osu, s)).ToList();
                foreach (long group in s_groups)
                {
                    foreach (Message message in messages)
                    {
                        await qq.SendGroupMessageAsync(group, message);
                    }
                    if (newSets.Count() > limit)
                    {
                        await qq.SendGroupMessageAsync(group, $"共有 {newSets.Count()} 张新图，只显示前 {limit} 张。");
                    }
                }
            }
        }
    }
}
