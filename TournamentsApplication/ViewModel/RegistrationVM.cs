using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TournamentsApplication.Utility;
using TournamentsApplication.Model;
using TournamentsApplication.VIew;
using System.Windows.Controls;

namespace TournamentsApplication.ViewModel
{
    internal class RegistrationVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentView => NavigationService.Instance.CurrentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public double StatusOpacity => StatusService.Instance.StatusOpacity;
        public string StatusText => StatusService.Instance.StatusText;
        private string login;

        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(); }
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        private string passwordConfirm;

        public string PasswordConfirm
        {
            get { return passwordConfirm; }
            set { passwordConfirm = value; OnPropertyChanged(); }
        }


        RelayCommand? closeWindowCommand;
        
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand ??
                    (closeWindowCommand = new RelayCommand((obj) =>
                    {
                        Window window = Application.Current.MainWindow;
                        window.Close();
                    }));
            }
        }
        RelayCommand? minimizeWindowCommand;

        public RelayCommand MinimizeWindowCommand
        {
            get
            {
                return minimizeWindowCommand ??
                    (minimizeWindowCommand = new RelayCommand((obj) =>
                    {
                        Window window = Application.Current.MainWindow;
                        window.WindowState = WindowState.Minimized;
                    }));
            }
        }
        private RelayCommand? signUpCommand;

        public RelayCommand SignUpCommand
        {
            get
            {
                return signUpCommand ??
                    (signUpCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            User? tmpUser;
                            if (uow.Users.GetAll().Where(a => a.Login == Login).Count() == 0)
                            {

                                if (Password == PasswordConfirm)
                                {
                                    tmpUser = new User() { Login = Login, Username = Username, Password = PasswordHasher.HashPassword(Password),
                                        IsLogined = false, IsAdmin = false, CreatedAt = DateTime.Now.ToUniversalTime() };
                                    uow.Users.Add(tmpUser);
                                    UserService.Instance.CurrentUser = tmpUser;
                                    NavigationService.Instance.SwitchCurrentView(new HomePageView());
                                    StatusService.Instance.SetStatusMessage($"Welcome, {tmpUser.Username}");
                                    uow.Save();
                                }
                            }
                            else
                            {
                                throw new Exception("This login already exists");
                            }
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }
        public RegistrationVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
        }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
        }
        private void OnStatusChanged()
        {
            OnPropertyChanged(nameof(StatusOpacity));
            OnPropertyChanged(nameof(StatusText));
        }
        private void OnViewChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }
    }
}
