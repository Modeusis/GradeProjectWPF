using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TournamentsApplication.View;

namespace TournamentsApplication.Utility
{
    class NavigationService
    {
        private static NavigationService _instance;
        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());

        private UserControl? currentView;
        private string? currentText;

        public event Action NavigationChanged;

        public UserControl? CurrentView
        {
            get { return currentView; }
            set { currentView = value; }
        }
        public string? CurrentText
        {
            get { return currentText; }
            set { currentText = value; }
        }
        private NavigationService()
        {
            CurrentView = null;
        }
        public void SwitchCurrentView(UserControl userControl)
        {
            CurrentView = userControl;
            CurrentText = "Log out";
            if (CurrentView?.GetType() == typeof(LoginView))
            {
                CurrentText = "I already have an account";
            }
            else if (CurrentView?.GetType() == typeof(RegistrationView))
            {
                CurrentText = "I don't have an account";
            }
            NavigationChanged?.Invoke();
        }

    }
}
