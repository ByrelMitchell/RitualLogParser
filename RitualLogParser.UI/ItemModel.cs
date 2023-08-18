using System;
using System.Linq;
using RitualLogParser.UI.Extensions;

namespace RitualLogParser.UI
{
    public class ItemModel
    {
        public const string CsvHeader = "Name 1,Name 2,Tribute,Tier,Class,Rarity,Item Level,Grid Position,Total Parse Time,Defer Time,Tribute Find Time,Cost Parse Time,TimeStamp";
        public const int MinT6 = 0;
        public const int MinT5 = 560;
        public const int MinT4 = 1050;
        public const int MinT3 = 1960;
        public const int MinT2 = 3951;
        public const int MinT1 = 7951;

        public string Name { get; set; }
        public int Cost { get; set; }
        public int Tier => Cost > MinT1 ? 1 : Cost > MinT2 ? 2 : Cost > MinT3 ? 3 : Cost > MinT4 ? 4 : Cost > MinT5 ? 5 : 6;
        public string Class { get; set; }
        public string Rarity { get; set; }
        public int ItemLevel { get; set; }
        public string Position { get; set; }
        public TimeSpan TotalParseTime { get; set; }
        public TimeSpan DeferTime { get; set; }
        public TimeSpan TributeFindTime { get; set; }
        public TimeSpan CostComputeTime { get; set; }
        public DateTime TimeStamp { get; set; }

        public string ToCsvLine()
        {
            var nameList = Name.Split("-").Select(s => s.Trim().StringToCSVCell()).ToList();
            var nameString = $"{nameList[0]},";
            if (nameList.Count > 1) nameString += nameList[1];

            return $"{nameString},{Cost},{Tier},{Class.StringToCSVCell()},{Rarity.StringToCSVCell()},{ItemLevel},{Position.StringToCSVCell()},{TotalParseTime.TotalMilliseconds},{DeferTime.TotalMilliseconds},{TributeFindTime.TotalMilliseconds},{CostComputeTime.TotalMilliseconds},{TimeStamp}";
        }
    }
}
