using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TournamentsApplication.View;

namespace TournamentsApplication.Utility
{
    class NavigationService
    {
        private static NavigationService _instance;
        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());

        private UserControl? currentView;
        private string? currentText;
        private SolidColorBrush? currentWindowBrush;
        private Window? window;

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
        public SolidColorBrush? CurrentWindowBrush
        {
            get => currentWindowBrush;
            set { currentWindowBrush = value; }
        }
        private NavigationService()
        {
            CurrentView = null;
            CurrentWindowBrush = new SolidColorBrush(Colors.Transparent);
            window = Application.Current.MainWindow;
        }
        public void SwitchCurrentView(UserControl userControl)
        {
            CurrentView = userControl;
            
            if (CurrentView?.GetType() == typeof(LoginView))
            {
                CurrentText = "I don't have an account";
                CurrentWindowBrush = new SolidColorBrush(Colors.Transparent);
                window.Width = 800;
                window.Height = 540;
                CenterWindow();
            }
            else if (CurrentView?.GetType() == typeof(RegistrationView))
            {
                CurrentText = "I already have an account";
                CurrentWindowBrush = new SolidColorBrush(Colors.Transparent);
                window.Width = 800;
                window.Height = 540;
                CenterWindow();
            }
            else if (CurrentView?.GetType() == typeof(HomePageView))
            {
                CurrentWindowBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d2d2d"));
                window.Width = 1280;
                window.Height = 850;
                CenterWindow();
            }
            NavigationChanged?.Invoke();
            
        }
        private void CenterWindow()
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            var windowWidth = window.Width;
            var windowHeight = window.Height;

            window.Left = (screenWidth - windowWidth) / 2;
            window.Top = (screenHeight - windowHeight) / 2;
        }
    }
}
