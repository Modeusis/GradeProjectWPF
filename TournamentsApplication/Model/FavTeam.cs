using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class FavTeam : ViewModelBase
    {
        private int favTeamId;
        private int teamId;
        private int userId;

        public int FavTeamId
        {
            get { return favTeamId; }
            set { favTeamId = value; OnPropertyChanged(nameof(FavTeamId)); }
        }
        public int TeamId
        {
            get { return teamId; }
            set { teamId = value; OnPropertyChanged(nameof(TeamId)); }
        }
        public int UserId
        {
            get { return userId; }
            set { userId = value; OnPropertyChanged(nameof(UserId)); }
        }
        public Team Team { get; set; }
        public User User { get; set; }
    }
}
