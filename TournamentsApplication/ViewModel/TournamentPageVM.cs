using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TournamentsApplication.Model;
using TournamentsApplication.Utility;
using TournamentsApplication.View;
using Microsoft.EntityFrameworkCore;

namespace TournamentsApplication.ViewModel
{
    internal class TournamentPageVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsLogin => UserService.Instance.Login;
        public bool IsAdmin => UserService.Instance.Admin;
        private Tournament showedTournament;
        private string tournamentName;
        private string commentContent;
        private bool isFavoriteTournament = false;
        private byte[] favoriteIcon;
        private byte[] tournamentIcon;
        private bool isHereCommentsPage = false;
        private bool isHereMatchesPage = false;
        private int currentCommentsPage = 1;
        private int currentMatchesPage = 1;
        private int amountOfCommentsPages = 1;
        private int amountOfMatchesPages = 1;
        public bool IsHereCommentsPage
        {
            get => isHereCommentsPage;
            set { isHereCommentsPage = value; OnPropertyChanged(); }
        }
        public bool IsHereMatchesPage
        {
            get => isHereMatchesPage;
            set { isHereMatchesPage = value; OnPropertyChanged(); }
        }
        public int CurrentCommentsPage
        {
            get => currentCommentsPage;
            set { currentCommentsPage = value; OnPropertyChanged(); }
        }
        public int CurrentMatchesPage
        {
            get => currentMatchesPage;
            set { currentMatchesPage = value; OnPropertyChanged(); }
        }
        public int AmountOfCommentsPages
        {
            get => amountOfCommentsPages;
            set { amountOfCommentsPages = value; OnPropertyChanged(); }
        }
        public int AmountOfMatchesPages
        {
            get => amountOfMatchesPages;
            set { amountOfMatchesPages = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TournamentComment> TournamentComments { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        public ObservableCollection<Match> Matches { get; set; }
        private TournamentComment? _selectedComment;
        public TournamentComment? SelectedComment
        {
            get => _selectedComment;
            set
            {
                if (_selectedComment != value)
                {
                    _selectedComment = value;
                    OnPropertyChanged();
                }
            }
        }

        public Tournament ShowedTournament
        {
            get => showedTournament;
            set { showedTournament = value; OnPropertyChanged(); }
        }
        public string TournamentName
        {
            get => tournamentName;
            set { tournamentName = value; OnPropertyChanged(); }
        }
        public string CommentContent
        {
            get => commentContent;
            set { commentContent = value; OnPropertyChanged(); }

        }
        private RelayCommand? nextPageCommand;
        public RelayCommand? NextPageCommand
        {
            get
            {
                return nextPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentCommentsPage < AmountOfCommentsPages)
                    {
                        CurrentCommentsPage++;
                        LoadComments(CurrentCommentsPage, 3); 
                    }
                });
            }
        }

        private RelayCommand? previousPageCommand;
        public RelayCommand? PreviousPageCommand
        {
            get
            {
                return previousPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentCommentsPage > 1)
                    {
                        CurrentCommentsPage--;
                        LoadComments(CurrentCommentsPage, 3); 
                    }
                });
            }
        }

        private RelayCommand? nextMatchesPageCommand;
        public RelayCommand? NextMatchesPageCommand
        {
            get
            {
                return nextMatchesPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentMatchesPage < AmountOfMatchesPages)
                    {
                        CurrentMatchesPage++;
                        LoadMatches(CurrentMatchesPage, 3);
                    }
                });
            }
        }

        private RelayCommand? previousMatchesPageCommand;
        public RelayCommand? PreviousMatchesPageCommand
        {
            get
            {
                return nextMatchesPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentMatchesPage > 1)
                    {
                        CurrentMatchesPage--;
                        LoadMatches(CurrentMatchesPage, 3);
                    }
                });
            }
        }


        private RelayCommand? changeFavoriteCommand;
        public RelayCommand ChangeFavoriteCommand
        {
            get
            {
                return changeFavoriteCommand ??
                    (changeFavoriteCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (IsFavoriteTournament == true)
                            {
                                User user = CurrentUser;
                                user.FavTournamentId = null;
                                IsFavoriteTournament = false;
                                StatusService.Instance.SetStatusMessage("Unfavorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                            else
                            {
                                User user = CurrentUser;
                                user.FavTournamentId = ShowedTournament.TournamentId;
                                IsFavoriteTournament = true;
                                StatusService.Instance.SetStatusMessage("Favorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                        }
                        catch (Exception ex)
                        {
                            StatusService.Instance.SetStatusMessage(ex.ToString());
                        }


                    }));
            }
        }
        private RelayCommand? toTeamPageCommand;
        public RelayCommand? ToTeamPageCommand
        {
            get
            {
                return toTeamPageCommand ??
                    (toTeamPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Team team)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new TeamPageView(team));
                        }
                    }));
            }
        }
        private RelayCommand? toMatchPageCommand;
        public RelayCommand? ToMatchPageCommand
        {
            get
            {
                return toMatchPageCommand ??
                    (toMatchPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Match match)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new MatchPageView(match));
                        }
                    }));
            }
        }
        private RelayCommand? addMessageCommand;
        public RelayCommand? AddMessageCommand
        {
            get
            {
                return addMessageCommand ??
                    (addMessageCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(CommentContent))
                            {
                                throw new Exception("Empty comment");
                            }
                            TournamentComment tempComment = new TournamentComment();
                            tempComment.Content = CommentContent;
                            tempComment.Author = CurrentUser.UserId;
                            tempComment.TournamentId = ShowedTournament.TournamentId;
                            tempComment.CreatedAt = DateTime.UtcNow;
                            uow.TournamentComments.Add(tempComment);
                            TournamentComments.Add(tempComment);
                            uow.Save();
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }
        private RelayCommand? deleteMessageCommand;
        public RelayCommand? DeleteMessageCommand
        {
            get
            {
                return deleteMessageCommand ??
                    (deleteMessageCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (SelectedComment is TournamentComment comment)
                            {

                                uow.TournamentComments.Delete(comment);
                                uow.Save();
                                TournamentComments.Remove(comment);
                                LoadComments(CurrentCommentsPage, 3);
                                StatusService.Instance.SetStatusMessage($"Comment from {comment.User.Username} deleted");
                            }
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }
        public bool IsFavoriteTournament
        {
            get => isFavoriteTournament;
            set { isFavoriteTournament = value; OnPropertyChanged(); }
        }
        public byte[] FavoriteIcon
        {
            get { return favoriteIcon; }
            set { favoriteIcon = value; OnPropertyChanged(); }
        }
        public byte[] TournamentIcon
        {
            get { return tournamentIcon; }
            set { tournamentIcon = value; OnPropertyChanged(); }
        }
        public TournamentPageVM(Tournament tournament)
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;
            TournamentComments = new ObservableCollection<TournamentComment>();

            ShowedTournament = tournament;
            FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
            if (CurrentUser != null && CurrentUser.FavTournamentId == ShowedTournament.TournamentId)
            {
                IsFavoriteTournament = true;
                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
            }
            TournamentName = ShowedTournament.TournamentName;
            TournamentIcon = ShowedTournament.Img;
            if (ShowedTournament.Teams.Count() > 0)
            {
                Teams =new ObservableCollection<Team>(ShowedTournament.Teams);
            }
            if (ShowedTournament.Matches.Count() > 0)
            {
                Matches = new ObservableCollection<Match>(ShowedTournament.Matches);
                LoadMatches(CurrentMatchesPage, 4);
            }
            if (uow.TournamentComments.GetAll().Where(a => a.TournamentId == ShowedTournament.TournamentId).Count() > 0)
            {
                TournamentComments = new ObservableCollection<TournamentComment>(uow.TournamentComments.GetAll().Where(a => a.TournamentId == ShowedTournament.TournamentId));
                LoadComments(CurrentCommentsPage, 3);
            }
        }
        private void LoadComments(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.TournamentComments.Get(pageNumber, pageSize, a => a.CommentId, a => a.TournamentId == ShowedTournament.TournamentId);

                TournamentComments.Clear();
                foreach (var comment in result.items)
                {
                    TournamentComments.Add(comment);
                }

                amountOfCommentsPages = result.TotalPages;
                CurrentCommentsPage = pageNumber;
            }
            catch (Exception ex)
            {
                StatusService.Instance.SetStatusMessage($"Error loading comments: {ex.Message}");
            }
        }
        private void LoadMatches(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.Matches.Get(pageNumber, pageSize, a => a.MatchId, a => a.TournamentId == ShowedTournament.TournamentId);

                Matches.Clear();
                foreach (var comment in result.items)
                {
                    Matches.Add(comment);
                }

                amountOfCommentsPages = result.TotalPages;
                CurrentCommentsPage = pageNumber;
            }
            catch (Exception ex)
            {
                StatusService.Instance.SetStatusMessage($"Error loading matches: {ex.Message}");
            }
        }


        private void OnContentChanged()
        {
            OnPropertyChanged(nameof(CurrentContentView));
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsLogin));
        }
    }
}
