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
using TournamentsApplication.View;

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
                            if (uow.Users.GetAll().Where(a => a.Login == Login).Count() != 1)
                            {
                                throw new Exception("Invalid Login.");
                            }
                            tmpUser = uow.Users.GetAll().Where(a => a.Login == Login).FirstOrDefault();
                            if (!PasswordHasher.VerifyPassword(Password, tmpUser.Password))
                            {
                                throw new Exception("Invalid password.");
                            }
                            tmpUser.IsLogined = true;
                            tmpUser.LastLogin = DateTime.Now.ToUniversalTime();
                            uow.Users.Update(tmpUser);
                            UserService.Instance.CurrentUser = tmpUser;
                            NavigationService.Instance.SwitchCurrentView(new HomePageView());
                            StatusService.Instance.SetStatusMessage($"Welcome back, {tmpUser.Username}");
                            uow.Save();
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                            uow.Dispose();
                        }
                    }));
            }
        }
        private RelayCommand? changeViewCommand;
        public RelayCommand ChangeViewCommand
        {
            get
            {
                return changeViewCommand ??
                    (changeViewCommand = new RelayCommand((obj) =>
                    {
                        NavigationService.Instance.SwitchCurrentView(new RegistrationView());
                    }));
            }
        }
        private RelayCommand? guestContinueCommand;

        public RelayCommand? GuestContinueCommand
        {
            get 
            {
                return guestContinueCommand ?? (guestContinueCommand = new RelayCommand((obj) =>
                {
                    NavigationService.Instance.SwitchCurrentView(new HomePageView());
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
