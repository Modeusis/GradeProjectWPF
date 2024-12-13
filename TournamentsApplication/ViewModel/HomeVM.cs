using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TournamentsApplication.Utility;

namespace TournamentsApplication.ViewModel
{
    class HomeVM : ViewModelBase
    {
        private RelayCommand? switchContentCommand;
        public RelayCommand SwitchContentCommand
        {
            get
            {
                return switchContentCommand ??
                    (switchContentCommand = new RelayCommand((obj) =>
                    {

                    }));
            }
        }
    }
}
