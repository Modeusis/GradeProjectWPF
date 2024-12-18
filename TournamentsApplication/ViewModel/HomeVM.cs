using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TournamentsApplication.Utility;
using TournamentsApplication.Model;
using TournamentsApplication.View;
using System.Windows.Controls;
using System.Windows.Input;

namespace TournamentsApplication.ViewModel
{
    class HomeVM : ViewModelBase
    {
        public UserControl? CurrentView => NavigationService.Instance.CurrentView;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public byte[]? CurrentUserLogo => UserService.Instance.CurrentUser.Logo;
        public bool Login => UserService.Instance.Login;
        public bool Admin => UserService.Instance.Admin;

        private Type? selectedContentType;
        public Type? SelectedContentType
        {
            get => selectedContentType;
            set { selectedContentType = value; OnPropertyChanged(nameof(SelectedContentType)); }
        }

        private RelayCommand? switchContentCommand;
        public RelayCommand? SwitchContentCommand
        {
            get
            {
                return switchContentCommand ??
                    (switchContentCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Type viewType)
                        {
                            var view = Activator.CreateInstance(viewType) as UserControl;
                            ContentNavigationService.Instance.SwitchCurrentContentView(view);
                        }
                    }));
            }
        }
        private RelayCommand? logInCommand;

        public RelayCommand?  LogInCommand
        {
            get 
            {
                return logInCommand ?? 
                (logInCommand = new RelayCommand((obj) => 
                {
                    NavigationService.Instance.SwitchCurrentView(new LoginView());
                }));
            }
        }
        private RelayCommand? logOutCommand;

        public RelayCommand? LogOutCommand
        {
            get
            {
                return logOutCommand ??
                (logOutCommand = new RelayCommand((obj) =>
                {
                    UserService.Instance.LogOut();
                    NavigationService.Instance.SwitchCurrentView(new LoginView());
                }));
            }
        }
        private RelayCommand? toMainPageCommand;

        public RelayCommand? ToMainPageCommand
        {
            get
            {
                return toMainPageCommand ??
                (toMainPageCommand = new RelayCommand((obj) =>
                {
                    SelectedContentType = null;
                    ContentNavigationService.Instance.SwitchCurrentContentView(new MatchesView());
                }));
            }
        }
        private RelayCommand? toUserPageCommand;

        public RelayCommand? ToUserPageCommand
        {
            get
            {
                return toUserPageCommand ??
                (toUserPageCommand = new RelayCommand((obj) =>
                {
                    SelectedContentType = null;
                    ContentNavigationService.Instance.SwitchCurrentContentView(new UserPageView());
                }));
            }
        }

        public HomeVM()
        {
            UserService.Instance.UserChanged += OnUserChanged;
            NavigationService.Instance.NavigationChanged += OnViewChanged;
            ContentNavigationService.Instance.NavigationChanged += OnContentViewChanged;

            ContentNavigationService.Instance.SwitchCurrentContentView(new UserPageView());
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(Admin));
            OnPropertyChanged(nameof(CurrentUserLogo));
        }
        private void OnViewChanged()
        {
            OnPropertyChanged(nameof(CurrentView));
        }
        private void OnContentViewChanged()
        {
            OnPropertyChanged(nameof(CurrentContentView));
        }
    }
}
