﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    public class Tournament : ViewModelBase
    {
        private int tournament_id;
        private string tournament_name;
        private int discipline_id;
        private byte[] img;
        private DateTime? start_date;
        private DateTime? end_date;
        private DateTime created_at;
        private DateTime? updated_at;

        public int TournamentId
        {
            get { return tournament_id; }
            set { tournament_id = value; OnPropertyChanged(nameof(TournamentId)); }
        }
        public string TournamentName
        {
            get { return tournament_name; }
            set { tournament_name = value; OnPropertyChanged(nameof(TournamentName)); }
        }
        public int DisciplineId
        {
            get { return discipline_id; }
            set { discipline_id = value; OnPropertyChanged(nameof(DisciplineId)); }
        }
        public byte[] Img
        {
            get { return img; }
            set { img = value; OnPropertyChanged(nameof(Img)); }
        }
        public DateTime? StartDate
        {
            get { return start_date; }
            set { start_date = value; OnPropertyChanged(nameof(StartDate)); }
        }
        public DateTime? EndDate
        {
            get { return end_date; }
            set { end_date = value; OnPropertyChanged(nameof(EndDate)); }
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
        public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
        public virtual Discipline Discipline { get; set; }
        public virtual ICollection<TournamentTeam> Teams { get; set; } = new List<TournamentTeam>();
        public virtual ICollection<TournamentComment> TournamentComments { get; set; } = new List<TournamentComment>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
