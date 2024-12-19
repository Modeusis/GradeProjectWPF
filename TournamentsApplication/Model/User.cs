using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    public class User : ViewModelBase
    {
        private int user_id;
        private int? favTeamId;
        private int? favPlayerId;
        private int? favTournamentId;
        private string username;
        private byte[]? logo;
        private byte[]? headerImg;
        private string? description;
        private string login;
        private string password;
        private bool islogined;
        private bool isadmin;
        private DateTime? last_login;
        private DateTime created_at;
        private DateTime? updated_at;

        public int UserId {
            get { return user_id; }
            set { user_id = value; OnPropertyChanged(nameof(UserId)); }
        }
        public int? FavTeamId
        {
            get { return favTeamId; }
            set { favTeamId = value; OnPropertyChanged(nameof(FavTeamId)); }
        }
        public int? FavPlayerId
        {
            get { return favPlayerId; }
            set { favPlayerId = value; OnPropertyChanged(nameof(FavPlayerId)); }
        }
        public int? FavTournamentId
        {
            get { return favTournamentId; }
            set { favTournamentId = value; OnPropertyChanged(nameof(FavTournamentId)); }
        }
        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }
        public byte[]? Logo
        {
            get { return logo; }
            set { logo = value; OnPropertyChanged(nameof(Logo)); }
        }
        public byte[]? HeaderImg
        {
            get { return headerImg; }
            set { headerImg = value; OnPropertyChanged(nameof(HeaderImg)); }
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
        public bool IsAdmin
        {
            get { return isadmin; }
            set { isadmin = value; OnPropertyChanged(nameof(IsAdmin)); }
        }
        public bool IsLogined
        {
            get { return islogined; }
            set { islogined = value; OnPropertyChanged(nameof(IsLogined)); }
        }
        public DateTime? LastLogin
        {
            get { return last_login; }
            set { last_login = value; OnPropertyChanged(nameof(LastLogin)); }
        }
        public DateTime CreatedAt
        {
            get { return created_at; }
            set { created_at = value; OnPropertyChanged(nameof(CreatedAt)); }
        }
        public DateTime? UpdatedAt
        {
            get { return updated_at; }
            set { updated_at = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }

        public virtual ICollection<TournamentComment> Comments { get; set; } = new List<TournamentComment>();

        public virtual Tournament Tournament {get; set;}
        public virtual Team Team { get; set; }
        public virtual Player Player { get; set; }
    }
}
