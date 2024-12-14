using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TournamentsApplication.Utility;
using TournamentsApplication.Model;

namespace TournamentsApplication.ViewModel
{
    class HomeVM : ViewModelBase
    {
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool Login => UserService.Instance.Login;
        private byte[] logo;
        public byte[] Logo
        {
            get
            {
                return logo;
            }
            set
            {
                logo = value; OnPropertyChanged(nameof(Logo));
            }
        }
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
        public HomeVM()
        {
            UserService.Instance.UserChanged += OnUserChanged;
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(Login));
        }
    }
}
