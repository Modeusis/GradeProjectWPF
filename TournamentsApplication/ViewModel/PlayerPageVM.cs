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

namespace TournamentsApplication.ViewModel
{
    internal class PlayerPageVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsLogin => UserService.Instance.Login;
        private Player showedPlayer;
        private byte[] showedPlayerImg;
        private byte[]? teamIcon;
        private byte[] favoriteIcon;
        private string showedPlayerName;
        private string teamName;
        private string showedPlayerRealName;
        private string showedPlayerPosition;
        private int showedPlayerAge;
        private bool isFavoritePlayer = false;
        private bool isHaveCurrentTeam = false;
        private Team? showedPlayerTeam;
        
        public ObservableCollection<Player>? TeamPlayers { get; set; }
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
                            if (IsFavoritePlayer == true)
                            {
                                User user = CurrentUser;
                                user.FavPlayerId = null;
                                IsFavoritePlayer = false;
                                StatusService.Instance.SetStatusMessage("Unfavorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                            else
                            {
                                User user = CurrentUser;
                                user.FavPlayerId = ShowedPlayer.PlayerId;
                                IsFavoritePlayer = true;
                                StatusService.Instance.SetStatusMessage("Favorite");
                                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
                                uow.Users.Update(user);
                                uow.Save();
                                UserService.Instance.RenewCurrentUser(user);
                            }
                        }
                        catch (Exception ex)
                        { 
                            
                        }
                        

                    }));
            }
        }
        private RelayCommand? itemClickCommand;
        public RelayCommand? ItemClickCommand
        {
            get
            {
                return itemClickCommand ??
                    (itemClickCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Player plr)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new PlayerPageView(plr));
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
                        if (showedPlayerTeam is Team team)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new TeamPageView(team));
                        }
                    }));
            }
        }
        public Player ShowedPlayer
        {
            get { return showedPlayer; }
            set { showedPlayer = value; OnPropertyChanged(); }
        }
        public byte[] ShowedPlayerImg
        {
            get { return showedPlayerImg; }
            set { showedPlayerImg = value; OnPropertyChanged(); }
        }
        public byte[] FavoriteIcon
        {
            get { return favoriteIcon; }
            set { favoriteIcon = value; OnPropertyChanged(); }
        }
        public byte[]? TeamIcon
        {
            get { return teamIcon; }
            set { teamIcon = value; OnPropertyChanged(); }
        }
        public string TeamName
        {
            get { return teamName; }
            set { teamName = value; OnPropertyChanged(); }
        }
        public string ShowedPlayerName
        {
            get { return showedPlayerName; }
            set { showedPlayerName = value; OnPropertyChanged(); }
        }
        public string ShowedPlayerRealName
        {
            get { return showedPlayerRealName; }
            set { showedPlayerRealName = value; OnPropertyChanged(); }
        }
        public string ShowedPlayerPosition
        {
            get { return showedPlayerPosition; }
            set { showedPlayerPosition = value; OnPropertyChanged(); }
        }
        public int ShowedPlayerAge
        {
            get { return showedPlayerAge; }
            set { showedPlayerAge = value; OnPropertyChanged(); }
        }
        public bool IsFavoritePlayer
        {
            get { return isFavoritePlayer; }
            set { isFavoritePlayer = value; OnPropertyChanged(); }
        }
        public bool IsHaveCurrentTeam
        {
            get { return isHaveCurrentTeam; }
            set { isHaveCurrentTeam = value; OnPropertyChanged(); }
        }
        public Team? ShowedPlayerTeam
        {
            get { return showedPlayerTeam; }
            set { showedPlayerTeam = value; OnPropertyChanged(); }
        }

        public PlayerPageVM(Player player)
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            ShowedPlayer = player;
            FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
            if (CurrentUser != null && CurrentUser.FavPlayerId == ShowedPlayer.PlayerId)
            {
                IsFavoritePlayer = true;
                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
            }
            ShowedPlayerName = ShowedPlayer.PlayerName;
            ShowedPlayerRealName = ShowedPlayer.PlayerRealName;
            ShowedPlayerPosition = ShowedPlayer.Position;
            ShowedPlayerAge = CalculateAge(ShowedPlayer.BirthDayDate);
            if (ShowedPlayer.Team is Team team)
            {
                IsHaveCurrentTeam = true;
                TeamPlayers = new ObservableCollection<Player>(team.Players);
                TeamIcon = team.TeamLogo;
                TeamName = team.TeamName;
                ShowedPlayerTeam = team;
            }
            if (ShowedPlayer.PlayerImg is byte[] img)
            {
                ShowedPlayerImg = img;
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

        public static int CalculateAge(DateTime birthDate, DateTime? currentDate = null)
        {
            DateTime today = currentDate ?? DateTime.Today;

            int age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
