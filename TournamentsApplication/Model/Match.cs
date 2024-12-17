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
        public int? scoreFirstTeam;
        public int? scoreSecondTeam;
        private DateTime match_time;
        private bool status;
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
        public int? ScoreFirstTeam
        {
            get { return scoreFirstTeam; }
            set { scoreFirstTeam = value; OnPropertyChanged(nameof(ScoreFirstTeam)); }
        }
        public int? ScoreSecondTeam
        {
            get { return scoreSecondTeam; }
            set { scoreSecondTeam = value; OnPropertyChanged(nameof(ScoreSecondTeam)); }
        }
        public DateTime MatchTime
        {
            get { return match_time; }
            set { match_time = value; OnPropertyChanged(nameof(MatchTime)); }
        }
        public bool Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(nameof(Status)); }
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

        public virtual Tournament Tournament { get; set; }
        public virtual Team FirstTeam { get; set; }
        public virtual Team SecondTeam { get; set; }
        public virtual ICollection<Statistics> Statistics { get; set; } = new List<Statistics>();

    }
}
