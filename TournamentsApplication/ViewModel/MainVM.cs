using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TournamentsApplication.Utility;
using TournamentsApplication.View;

namespace TournamentsApplication.ViewModel
{
    class MainVM : ViewModelBase
    {
        private UserControl currentView;
        private string currentText;
        private string statusText;
        private bool isVisible;
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
        public string StatusText
        {
            get { return statusText; }
            set { statusText = value; OnPropertyChanged(); }
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; OnPropertyChanged(); }
        }

        public MainVM()
        {
            CurrentView = new LoginView();
            CurrentText = "I dont have an account";
            IsVisible = false;
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
                            ShowStatusMessage("Text");
                        }
                        else
                        {
                            CurrentView = new LoginView();
                            CurrentText = "I dont have an account";
                        }
                            
                    }));
            }
        }
        //public RelayCommand ChangeViewCommand
        //{
        //    get
        //    {
        //        return changeViewCommand ??
        //            (changeViewCommand = new RelayCommand((obj) =>
        //            {
        //                if (obj is Type viewType)
        //                {
        //                    var view = Activator.CreateInstance(viewType) as UserControl;
        //                    CurrentView = view;
        //                }
        //            }));
        //    }
        //}
        public void ShowStatusMessage(string message)
        {
            StatusText = message;
            IsVisible = true;
        }

        
        public RelayCommand AnimationCompletedCommand
        {
            get
            {
                return animationCompletedCommand ??= new RelayCommand((obj) =>
                {
                    IsVisible = false;
                    MessageBox.Show("1");
                });
            }
        }
    }
}
