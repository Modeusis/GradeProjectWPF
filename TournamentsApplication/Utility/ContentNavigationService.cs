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
    class ContentNavigationService
    {
        private static ContentNavigationService _instance;
        public static ContentNavigationService Instance => _instance ?? (_instance = new ContentNavigationService());

        private UserControl? currentContentView;

        public event Action NavigationChanged;

        public UserControl? CurrentContentView
        {
            get { return currentContentView; }
            set { currentContentView = value; }
        }
        private ContentNavigationService()
        {
            currentContentView = null;
        }
        public void SwitchCurrentContentView(UserControl userControl)
        {
            CurrentContentView = userControl;
            
            NavigationChanged?.Invoke();
            
        }
    }
}
