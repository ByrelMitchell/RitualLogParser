using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RitualLogParser.UI.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public bool SetProperty<T>(T value, ref T variable, [CallerMemberName]string variableName = "")
        {
            if (EqualityComparer<T>.Default.Equals(value, variable)) return false;

            variable = value;
            NotifyPropertyChanged(variableName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
