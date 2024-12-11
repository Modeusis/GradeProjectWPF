using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class Player : ViewModelBase
    {
        private int player_id;
        private string player_name;
        private byte[] player_img;
        private int? cur_team_id;
        private DateTime created_at;
        private DateTime updated_at;

        public int PlayerId
        {
            get { return player_id; }
            set { player_id = value; OnPropertyChanged(nameof(PlayerId)); }
        }
        public string PlayerName
        {
            get { return player_name; }
            set { player_name = value; OnPropertyChanged(nameof(PlayerName)); }
        }
        public byte[] PlayerImg
        {
            get { return player_img; }
            set { player_img = value; OnPropertyChanged(nameof(PlayerImg)); }
        }
        public int? CurTeamId
        {
            get { return cur_team_id; }
            set { cur_team_id = value; OnPropertyChanged(nameof(CurTeamId)); }
        }
        public DateTime CreatedAt
        {
            get { return created_at; }
            set { created_at = value; OnPropertyChanged(nameof(CreatedAt)); }
        }
        public DateTime UpdatedAt
        {
            get { return updated_at; }
            set { updated_at = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }

        public Team Team { get; set; }

        public ICollection<FavPlayer> FavPlayers { get; set; }
    }
}
