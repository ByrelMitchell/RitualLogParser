using RitualLogParser.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace RitualLogParser.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private string ritualLogsPath = @"D:\Documents\POE\Ritual\RitualLogs";
        private bool pathLoaded;
        private DateTime minDate;
        private DateTime maxDate;
        private DateTime startDate;
        private DateTime endDate;
        private int totalNumberOfLogs;
        private ICollectionView shownItems;

        public MainWindowViewModel()
        {
            ShownItems = CollectionViewSource.GetDefaultView(Items);
        }

        public string RitualLogsPath { get => ritualLogsPath; set => SetProperty(value, ref ritualLogsPath); }

        public bool PathLoaded { get => pathLoaded; set => SetProperty(value, ref pathLoaded); }

        public DateTime MinDate
        {
            get => minDate; set => SetProperty(value, ref minDate);
        }

        public DateTime MaxDate
        {
            get => maxDate; set => SetProperty(value, ref maxDate);
        }

        public int NumberOfLogsInRange => LogFiles.Where(f => f.TimeStamp >= StartDate && f.TimeStamp <= EndDate).Count();
        public int TotalNumberOfLogs { get => totalNumberOfLogs; set => SetProperty(value, ref totalNumberOfLogs); }
        public DateTime StartDate
        {
            get => startDate; set
            {
                if (SetProperty(value, ref startDate))
                {
                    NotifyPropertyChanged(nameof(NumberOfLogsInRange));
                }
            }
        }


        public DateTime EndDate
        {
            get => endDate; set
            {
                if (SetProperty(value, ref endDate))
                {
                    NotifyPropertyChanged(nameof(NumberOfLogsInRange));
                }
            }
        }

        public ObservableCollection<LogFileEntryViewModel> LogFiles { get; } = new ObservableCollection<LogFileEntryViewModel>();
        public ObservableCollection<string> Classes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Rarities { get; } = new ObservableCollection<string>();
        public ObservableCollection<ItemModel> Items { get; } = new ObservableCollection<ItemModel>();
        public ICollectionView ShownItems { get => shownItems; set => SetProperty(value, ref shownItems); }

        public void LoadLogFolder()
        {
            var directory = new DirectoryInfo(RitualLogsPath);
            if (!directory.Exists)
            {
                PathLoaded = false;
                return;
            }
            var files = directory.GetFiles();
            LogFiles.Clear();
            foreach (var file in files)
            {
                LogFiles.Add(new LogFileEntryViewModel
                {
                    FileInfo = file,
                    TimeStamp = file.GetTimeStamp()
                });
            }

            MinDate = LogFiles.Min(f => f.TimeStamp);
            MaxDate = LogFiles.Max(f => f.TimeStamp);
            if (StartDate < MinDate) StartDate = MinDate;
            if (EndDate < MinDate || EndDate > MaxDate) EndDate = MaxDate;
            TotalNumberOfLogs = LogFiles.Count;
            PathLoaded = true;
        }

        public void ParseLogFiles()
        {
            var logFiles = LogFileParser.ParseLogFiles(LogFiles.Select(lf => lf.FileInfo));
            Items.Clear();
            Classes.Clear();
            Rarities.Clear();
            var classes = new List<string>();
            var rarities = new List<string>();
            foreach (var logFile in logFiles)
            {
                foreach (var item in logFile.Items)
                {
                    Items.Add(item);
                    if (!classes.Contains(item.Class)) classes.Add(item.Class);
                    if (!rarities.Contains(item.Rarity)) rarities.Add(item.Rarity);
                }
            }
            foreach (var classObj in classes.OrderBy(s => s))
            {
                Classes.Add(classObj);
            }
            foreach (var rarity in rarities.OrderBy(s => s))
            {
                Rarities.Add(rarity);
            }
        }


    }

    public class LogFileEntryViewModel : ViewModelBase
    {
        public FileInfo FileInfo { get; set; }
        public string FileName => FileInfo?.Name ?? "";
        public DateTime TimeStamp { get; set; }
    }
}
