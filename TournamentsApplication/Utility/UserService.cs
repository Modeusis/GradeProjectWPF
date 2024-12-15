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
        private bool admin;


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
        public bool Admin
        {
            get { return admin; }
            set { admin = value; }
        }
        private UserService()
        {
            CurrentUser = null;
            Login = false;
            Admin = false;
        }
        public void UpdateCurrentUser(User user)
        {
            CurrentUser = user;
            Login = CurrentUser.IsLogined;
            Admin = CurrentUser.IsAdmin;
            UserChanged?.Invoke();
        }
        public void RenewCurrentUser(User user)
        {
            CurrentUser = user;
            UserChanged?.Invoke();
        }
        public void LogOut()
        {
            if (CurrentUser != null) 
            {
                uow = new UnitOfWork(new ApplicationContext());
                CurrentUser.IsLogined = false;
                Login = false;
                Admin = false;
                uow.Users.Update(CurrentUser);
                uow.Save();
                CurrentUser = null;
            }
        }
        public void DeleteCurrentUser()
        {
            if (CurrentUser != null)
            {
                uow = new UnitOfWork(new ApplicationContext());
                CurrentUser.IsLogined = false;
                Login = false;
                Admin = false;
                uow.Users.Delete(CurrentUser);
                uow.Save();
                CurrentUser = null;
            }
        }
    }
}
