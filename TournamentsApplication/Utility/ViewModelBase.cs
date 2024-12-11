using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TournamentsApplication.Utility
{
    public class ViewModelBase
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            MessageBox.Show(propName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
