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

        public event Action UserChanged;

        public User? CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
        private UserService()
        {
            CurrentUser = null;
        }
        public void UpdateCurrentUser(User user)
        {
            CurrentUser = user;
            UserChanged?.Invoke();
        }
        public void LogOut()
        {
            CurrentUser = null;
        }
    }
}
