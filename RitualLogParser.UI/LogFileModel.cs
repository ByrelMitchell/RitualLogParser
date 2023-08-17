using System;
using System.Collections.Generic;

namespace RitualLogParser.UI
{
    public class LogFileModel
    {
        public DateTime TimeStamp { get; set; }
        public int Tribute { get; set; }
        public List<ItemModel> Items { get; } = new List<ItemModel>();
    }
}
