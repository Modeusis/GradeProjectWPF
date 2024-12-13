using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TournamentsApplication.Model;

namespace TournamentsApplication.Utility
{
    class UserService
    {
        private static UserService _instance;
        public static UserService Instance => _instance ?? (_instance = new UserService());

        private User? currentUser;
        private bool logined;

        public event Action UserChanged;

        public User? CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
        public bool Logined
        {
            get { return logined; }
            set { logined = value; }
        }
        private UserService()
        {
            CurrentUser = null;
            Logined = false;
        }
        public void UpdateCurrentUser(User user)
        {
            CurrentUser = user;
            Logined = CurrentUser.IsLogined;
            UserChanged?.Invoke();
        }
        public void LogOut()
        {
            CurrentUser.IsLogined = false;
            Logined= false;
        }
        public void ClearUser()
        {
            CurrentUser = null;
        }
    }
}
