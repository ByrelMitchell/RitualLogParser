using MahApps.Metro.Controls;
using Microsoft.Win32;
using RitualLogParser.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RitualLogParser.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ViewModel.LoadLogFolder();
            ViewModel.ShownItems.Filter = ItemsFilter;
        }

        public MainWindowViewModel ViewModel { get; } = new MainWindowViewModel();

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.ShownItems.Refresh();
        }

        private void ParseLogs_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ParseLogFiles();
        }

        private void LoadFolder_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadLogFolder();
        }

        public bool ItemsFilter(object rawItem)
        {

            if (!(rawItem is ItemModel item)) return true;

            return (ClassesListBox.SelectedItems.Contains(item.Class) || ClassesListBox.SelectedItems.Count < 1) &&
                (RaritiesListBox.SelectedItems.Contains(item.Rarity) || RaritiesListBox.SelectedItems.Count < 1);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var toBeExported = ViewModel.Items.Where(ItemsFilter).ToList();
            var fileDialog = new SaveFileDialog() { 
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = $"RitualExport {ViewModel.Items.Min(i => i.TimeStamp):yyyy-MM-dd-HH-mm-ss} - {ViewModel.Items.Max(i => i.TimeStamp):yyyy-MM-dd-HH-mm-ss}.csv"
            };
            if(fileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine(ItemModel.CsvHeader);
                foreach(var item in toBeExported)
                {
                    csvContent.AppendLine(item.ToCsvLine());
                }
                File.WriteAllText(fileDialog.FileName, csvContent.ToString());
            }

        }
    }
}
