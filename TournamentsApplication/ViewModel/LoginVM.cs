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
using TournamentsApplication.View;
using TournamentsApplication.VIew;

namespace TournamentsApplication.ViewModel
{
    class LoginVM : ViewModelBase
    {
        private UnitOfWork uow;
        public string StatusText => StatusService.Instance.StatusText;
        public double StatusOpacity => StatusService.Instance.StatusOpacity;
        public UserControl? CurrentView => NavigationService.Instance.CurrentView;
        public string? CurrentText => NavigationService.Instance.CurrentText;
        public User? CurrentUser => UserService.Instance.CurrentUser;
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
                            User? tmpUser;
                            if (uow.Users.GetAll().Where(a => a.Login == Login).Count() == 1)
                            {
                                tmpUser = uow.Users.GetAll().Where(a => a.Login == Login).FirstOrDefault();
                                if (PasswordHasher.VerifyPassword(Password, tmpUser.Password))
                                {
                                    UserService.Instance.CurrentUser = tmpUser;
                                    NavigationService.Instance.SwitchCurrentView(new HomePageView());
                                    StatusService.Instance.SetStatusMessage($"Welcome back, {tmpUser.Username}");
                                }
                                else
                                {
                                    throw new Exception("Invalid password!");
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid Login! Try again");
                            }
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }
        public LoginVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
            NavigationService.Instance.NavigationChanged += OnViewChanged;
            UserService.Instance.UserChanged += OnUserChanged;
        }

        private void OnStatusChanged()
        {
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(StatusOpacity));
        }
        private void OnViewChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
            OnPropertyChanged(nameof(CurrentText));
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
        }
    }
}
