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
        RelayCommand? closeWindowCommand;
        RelayCommand? minimizeWindowCommand;
        RelayCommand? changeViewCommand;
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

        public MainVM()
        {
            CurrentView = new LoginView();
        }

        //public RelayCommand ChangeViewCommand
        //{
        //    get
        //    {
        //        return changeViewCommand ??
        //            (changeViewCommand = new RelayCommand((obj) =>
        //            {
        //                CurrentView = new RegistrationView();
        //            }));
        //    }
        //}
        public RelayCommand ChangeViewCommand
        {
            get
            {
                return changeViewCommand ??
                    (changeViewCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Type viewType)
                        {
                            var view = Activator.CreateInstance(viewType) as UserControl;
                            CurrentView = view;
                        }
                    }));
            }
        }
    }
}
