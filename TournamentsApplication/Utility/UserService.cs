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
        private bool login;

        public event Action UserChanged;
        private UnitOfWork uow;

        public User? CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
        public bool Login
        {
            get { return login; }
            set { login = value; }
        }
        private UserService()
        {
            CurrentUser = null;
            Login = false;
        }
        public void UpdateCurrentUser(User user)
        {
            CurrentUser = user;
            Login = CurrentUser.IsLogined;
            UserChanged?.Invoke();
        }
        public void LogOut()
        {
            if (CurrentUser != null) 
            {
                uow = new UnitOfWork(new ApplicationContext());
                CurrentUser.IsLogined = false;
                Login = false;
                uow.Users.Update(CurrentUser);
                uow.Save();
                CurrentUser = null;
            }
        }
    }
}
