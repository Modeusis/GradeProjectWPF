using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class User : ViewModelBase
    {
        private int user_id;
        private string username;
        private byte[] logo;
        private byte[] headerImg;
        private string? description;
        private string login;
        private string password;
        private bool islogined;
        private bool isadmin;
        private DateTime last_login;
        private DateTime created_at;
        private DateTime? updated_at;

        public int UserId { 
            get { return user_id; }
            set { user_id = value; OnPropertyChanged(nameof(UserId)); }
        }
        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }
        public byte[] Logo
        {
            get { return logo; }
            set { logo = value; OnPropertyChanged(nameof(Logo)); }
        }
        public byte[] HeaderImg
        { 
            get { return headerImg; }
            set { headerImg = value; OnPropertyChanged( nameof(HeaderImg)); }   
        }
        public string? Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(nameof(Description)); }
        }
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(nameof(Login)); }
        }
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }
        public DateTime LastLogin
        {
            get { return last_login; }
            set { last_login = value; OnPropertyChanged(nameof(LastLogin)); }
        }
        public DateTime CreatedAt
        {
            get { return created_at; }
            set { CreatedAt = value; OnPropertyChanged(nameof(CreatedAt)); }
        }
        public DateTime? UpdatedAt
        {
            get { return updated_at; }
            set { updated_at = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }

        public ICollection<TournamentComment> Comments { get; set; }
    }
}
