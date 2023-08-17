using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RitualLogParser.UI.Extensions
{
    public static class FileInfoExtensions
    {
        public const string LogFileNameRegex = "text(?<Year>[0-9]*)_(?<Month>[0-9]*)_(?<Day>[0-9]*)_(?<Hour>[0-9]*)_(?<Minute>[0-9]*)_(?<Second>[0-9]*).txt";
        public static DateTime GetTimeStamp(this FileInfo @this)
        {
            var matches = Regex.Match(@this.Name, LogFileNameRegex);
            if (!matches.Success) return DateTime.MinValue;
            var year = int.Parse(matches.Groups["Year"].Value);
            var month = int.Parse(matches.Groups["Month"].Value);
            var day = int.Parse(matches.Groups["Day"].Value);
            var hour = int.Parse(matches.Groups["Hour"].Value);
            var minute = int.Parse(matches.Groups["Minute"].Value);
            var second = int.Parse(matches.Groups["Second"].Value);
            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
