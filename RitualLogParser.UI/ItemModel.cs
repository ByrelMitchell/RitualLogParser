using System;
using System.Linq;
using RitualLogParser.UI.Extensions;

namespace RitualLogParser.UI
{
    public class ItemModel
    {
        public const string CsvHeader = "Name 1,Name 2,Tribute,Class,Rarity,Defer Cost,Item Level,Grid Position,Total Parse Time,Tribute Find Time,Cost Parse Time";

        public string Name { get; set; }
        public int Cost { get; set; }
        public string Class { get; set; }
        public string Rarity { get; set; }
        public int DeferCost { get; set; }
        public int ItemLevel { get; set; }
        public string Position { get; set; }
        public TimeSpan TotalParseTime { get; set; }
        public TimeSpan TributeFindTime { get; set; }
        public TimeSpan CostComputeTime { get; set; }
        public DateTime TimeStamp { get; set; }

        public string ToCsvLine()
        {
            var nameList = Name.Split("-").Select(s => s.Trim().StringToCSVCell()).ToList();
            var nameString = $"{nameList[0]},";
            if (nameList.Count > 1) nameString += nameList[1];

            return $"{nameString},{Cost},{Class.StringToCSVCell()},{Rarity.StringToCSVCell()},{DeferCost},{ItemLevel},{Position.StringToCSVCell()},{TotalParseTime.TotalMilliseconds},{TributeFindTime.TotalMilliseconds},{CostComputeTime.TotalMilliseconds}";
        }
    }
}
