using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class FavTournament : ViewModelBase
    {
        private int favTournamentId;
        private int tournamentId;
        private int userId;

        public int FavTournamentId
        {
            get { return favTournamentId; }
            set { favTournamentId = value; OnPropertyChanged(nameof(FavTournamentId)); }
        }
        public int TournamentId
        {
            get { return tournamentId; }
            set { tournamentId = value; OnPropertyChanged(nameof(TournamentId)); }
        }
        public int UserId
        {
            get { return userId; }
            set { userId = value; OnPropertyChanged(nameof(UserId)); }
        }
        public Tournament Tournament { get; set; }
        public User User { get; set; }
    }
}
