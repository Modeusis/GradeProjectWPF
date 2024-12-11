using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class FavPlayer : ViewModelBase
    {
        private int favPlayerId;
        private int playerId;
        private int userId;

        public int FavPlayerId 
        { 
            get { return favPlayerId; }
            set { favPlayerId = value; OnPropertyChanged(nameof(FavPlayerId)); }
        }
        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; OnPropertyChanged(nameof(PlayerId)); }
        }
        public int UserId
        {
            get { return userId; }
            set { userId = value; OnPropertyChanged(nameof(UserId)); }
        }

        public Player Player { get; set; }
        public User User { get; set; }
    }
}
