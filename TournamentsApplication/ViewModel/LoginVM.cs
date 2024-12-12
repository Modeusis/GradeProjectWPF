using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;
using TournamentsApplication;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using TournamentsApplication.Model;

namespace TournamentsApplication.ViewModel
{
    class LoginVM : ViewModelBase
    {
        private UnitOfWork uow;
        public string StatusText => StatusService.Instance.StatusText;
        public double StatusOpacity => StatusService.Instance.StatusOpacity;
        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private User userToLogIn;

        public User UserToLogIn
        {
            get { return userToLogIn; }
            set { userToLogIn = value; OnPropertyChanged(); }
        }
        private RelayCommand? logInCommand;

        public RelayCommand LogInCommand
        {
            get
            {
                return logInCommand ??
                    (logInCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            StatusService.Instance.SetStatusMessage($"{Login}, {Password}");
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                            throw;
                        }
                    }));
            }
        }
        public LoginVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
            
        }

        private void OnStatusChanged()
        {
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(StatusOpacity));
        }
    }
}
