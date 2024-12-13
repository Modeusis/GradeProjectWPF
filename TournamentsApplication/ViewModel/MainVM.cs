using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TournamentsApplication.Utility;
using TournamentsApplication.View;
using TournamentsApplication.Model;

namespace TournamentsApplication.ViewModel
{
    class MainVM : ViewModelBase
    {
        public string StatusText => StatusService.Instance.StatusText;
        public double StatusOpacity => StatusService.Instance.StatusOpacity;
        public UserControl? CurrentView => NavigationService.Instance.CurrentView;
        public string? CurrentText => NavigationService.Instance.CurrentText;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        RelayCommand? closeWindowCommand;
        RelayCommand? minimizeWindowCommand;
        RelayCommand? changeViewCommand;
        RelayCommand? animationCompletedCommand;
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

        public MainVM()
        {

            StatusService.Instance.StatusChanged += OnStatusChanged;
            NavigationService.Instance.NavigationChanged += OnViewChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            NavigationService.Instance.SwitchCurrentView(new LoginView());
        }

        public RelayCommand ChangeViewCommand
        {
            get
            {
                return changeViewCommand ??
                    (changeViewCommand = new RelayCommand((obj) =>
                    {
                        if (CurrentView?.GetType() == typeof(LoginView))
                        {
                            NavigationService.Instance.SwitchCurrentView(new RegistrationView());
                        }
                        else if (CurrentView?.GetType() == typeof(RegistrationView))
                        {
                            NavigationService.Instance.SwitchCurrentView(new LoginView());
                        }
                        else
                        {
                            NavigationService.Instance.SwitchCurrentView(new LoginView());
                            UserService.Instance.LogOut();
                        }    
                    }));
            }
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
