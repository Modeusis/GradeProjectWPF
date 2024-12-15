using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    public class Match : ViewModelBase
    {
        private int match_id;
        private int tournament_id;
        private int first_participant_id;
        private int second_participant_id;
        private DateTime match_time;
        private string status;
        private string result;
        private int? winner_id;
        private DateTime created_at;
        private DateTime? updated_at;

        public int MatchId
        {
            get { return match_id; }
            set { match_id = value; OnPropertyChanged(nameof(MatchId)); }
        }
        public int TournamentId
        {
            get { return tournament_id; }
            set { tournament_id = value; OnPropertyChanged(nameof(TournamentId)); }
        }
        public int FirstParticipantId
        {
            get { return first_participant_id; }
            set { first_participant_id = value; OnPropertyChanged(nameof(FirstParticipantId)); }
        }
        public int SecondParticipantId
        {
            get { return second_participant_id; }
            set { second_participant_id = value; OnPropertyChanged(nameof(SecondParticipantId)); }
        }
        public DateTime MatchTime
        {
            get { return match_time; }
            set { match_time = value; OnPropertyChanged(nameof(MatchTime)); }
        }
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }
        public string Result
        {
            get { return result; }
            set { result = value; OnPropertyChanged(nameof(Result)); }
        }
        public int? WinnerId
        {
            get { return winner_id; }
            set { winner_id = value; OnPropertyChanged(nameof(WinnerId)); }
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

        public Tournament Tournament { get; set; }
        public Team FirstTeam { get; set; }
        public Team SecondTeam { get; set; }

    }
}
