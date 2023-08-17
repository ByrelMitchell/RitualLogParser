using RitualLogParser.UI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RitualLogParser.UI
{
    public static class LogFileParser
    {
        public static List<LogFileModel> ParseLogFiles(IEnumerable<FileInfo> logFiles)
        {
            var logFileModels = new List<LogFileModel>();
            foreach(var file in logFiles)
            {
                var items = SplitLogFileIntoItems(file);
                var tribute = 0;
                int.TryParse(Regex.Match(items[0], @"Tribute Available ([0-9]*)").Groups[1].Value, out tribute);

                var logFile = new LogFileModel
                {
                    TimeStamp = file.GetTimeStamp(),
                    Tribute = tribute
                };
                foreach (var item in items.Skip(1))
                {
                    var itemModel = ParseItemModel(item);
                    itemModel.TimeStamp = logFile.TimeStamp;

                    logFile.Items.Add(itemModel);

                }
                logFileModels.Add(logFile);
            }
            return logFileModels;
        }

        public static List<string> SplitLogFileIntoItems(FileInfo file)
        {
            var items = File.ReadAllText(file.FullName).Split("Pos: ");
            return items.ToList();
        }

        public static ItemModel ParseItemModel(string itemString)
        {
            var firstSection = itemString.Substring(0, itemString.IndexOf("--------"));
            int cost = 0, deferCost = 0, ilvl = 0, totalMs = 0, tribFindMs = 0, costCompMs = 0;
            int.TryParse(Regex.Match(firstSection, @"Cost: (.*)").Groups[1].Value, out cost);
            int.TryParse(Regex.Match(firstSection, @"Defer: (.*)").Groups[1].Value, out deferCost);
            int.TryParse(Regex.Match(itemString, @"Item Level: (.*)").Groups[1].Value, out ilvl);
            int.TryParse(Regex.Match(firstSection, @"Total time: (.*)").Groups[1].Value, out totalMs);
            int.TryParse(Regex.Match(firstSection, @"Tribute find: (.*)").Groups[1].Value, out tribFindMs);
            int.TryParse(Regex.Match(firstSection, @"Cost compute: (.*)").Groups[1].Value, out costCompMs);
            var item = new ItemModel
            {
                Position = Regex.Match(firstSection, @"([0-9]*)").Groups[1].Value.TrimEnd(),
                Cost = cost,
                Class = Regex.Match(firstSection, @"Item Class: (.*)").Groups[1].Value.TrimEnd(),
                Rarity = Regex.Match(firstSection, @"Rarity: (.*)").Groups[1].Value.TrimEnd(),
                ItemLevel = ilvl,
                DeferCost = deferCost,
                TotalParseTime = TimeSpan.FromMilliseconds(totalMs),
                TributeFindTime = TimeSpan.FromMilliseconds(tribFindMs),
                CostComputeTime = TimeSpan.FromMilliseconds(costCompMs)
            };
            var nameLines = firstSection.Split("Rarity:")[1].Split('\n').Skip(1).SkipLast(1);
            item.Name = String.Join(" - ", nameLines.Select(l => l.Trim()));
            return item;
        }
    }
}
