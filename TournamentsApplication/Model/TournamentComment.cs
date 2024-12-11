﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentsApplication.Utility;

namespace TournamentsApplication.Model
{
    internal class TournamentComment : ViewModelBase
    {
        private int comment_id;
        private int tournament_id;
        private int author;
        private string content;
        private int comment_level;
        private int parent_comment_id;
        private DateTime created_at;
        private DateTime updated_at;

        public int CommentId
        {
            get { return comment_id; }
            set { comment_id = value; OnPropertyChanged(nameof(CommentId)); }
        }
        public int TournamentId
        {
            get { return tournament_id; }
            set { tournament_id = value; OnPropertyChanged(nameof(TournamentId)); }
        }
        public int Author
        {
            get { return author; }
            set { author = value; OnPropertyChanged(nameof(Author)); }
        }
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(nameof(Content)); }
        }
        public int CommentLevel
        {
            get { return comment_level; }
            set { comment_level = value; OnPropertyChanged(nameof(CommentLevel)); }
        }
        public int ParentCommentId
        {
            get { return parent_comment_id; }
            set { parent_comment_id = value; OnPropertyChanged(nameof(ParentCommentId)); }
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

        public User User { get; set; }
        public Tournament Tournament { get; set; }
    }
}