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

namespace TournamentsApplication.ViewModel
{
    class MainVM : ViewModelBase
    {
        public string StatusText => StatusService.Instance.StatusText;
        public double StatusOpacity => StatusService.Instance.StatusOpacity;
        private UserControl currentView;
        private string currentText;
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
        public UserControl CurrentView
        {
            get { return currentView; }
            set { currentView = value; OnPropertyChanged(); }
        }
        public string CurrentText
        {
            get { return currentText; }
            set { currentText = value; OnPropertyChanged(); }
        }

        public MainVM()
        {
            CurrentView = new LoginView();
            CurrentText = "I dont have an account";

            

            StatusService.Instance.StatusChanged += OnStatusChanged;
        }

        public RelayCommand ChangeViewCommand
        {
            get
            {
                return changeViewCommand ??
                    (changeViewCommand = new RelayCommand((obj) =>
                    {
                        if (CurrentView.GetType() == typeof(LoginView))
                        {
                            CurrentView = new RegistrationView();
                            CurrentText = "I already have an account";
                            StatusService.Instance.SetStatusMessage(CurrentText);
                        }
                        else
                        {
                            CurrentView = new LoginView();
                            CurrentText = "I dont have an account";
                        }
                            
                    }));
            }
        }

        private void OnStatusChanged()
        {
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(StatusOpacity));
        }
    }
}
