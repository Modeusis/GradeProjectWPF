using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TournamentsApplication.Utility;
using TournamentsApplication.View;
using TournamentsApplication.Model;
using System.Windows.Media;

namespace TournamentsApplication.ViewModel
{
    class MainVM : ViewModelBase
    {
        private UnitOfWork uow;
        public string StatusText => StatusService.Instance.StatusText;
        public double StatusOpacity => StatusService.Instance.StatusOpacity;
        public UserControl? CurrentView => NavigationService.Instance.CurrentView;
        public string? CurrentText => NavigationService.Instance.CurrentText;
        public SolidColorBrush? WindowBorderColor => NavigationService.Instance.CurrentWindowBrush;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        RelayCommand? closeWindowCommand;
        RelayCommand? minimizeWindowCommand;
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
            uow = new UnitOfWork(new ApplicationContext());
            Window window = Application.Current.MainWindow;
            WeakEventManager<Window, EventArgs>.AddHandler(window, "Closed", (sender, e) =>
            {
                if (CurrentUser != null)
                {
                    UserService.Instance.LogOut();
                    uow.Users.Update(CurrentUser);
                    uow.Save();
                    UserService.Instance.ClearUser();
                }
            });
            StatusService.Instance.StatusChanged += OnStatusChanged;
            NavigationService.Instance.NavigationChanged += OnViewChanged;
            UserService.Instance.UserChanged += OnUserChanged;
            
            NavigationService.Instance.SwitchCurrentView(new LoginView());
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
            OnPropertyChanged(nameof(WindowBorderColor));
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
        }
    }
}
