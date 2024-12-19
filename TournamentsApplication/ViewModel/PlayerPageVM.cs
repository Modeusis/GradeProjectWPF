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
        public bool IsAdmin => UserService.Instance.Admin;
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
        private double aKD;
        private bool isChanging = false;
        private bool isFavoritePlayer = false;
        private bool isHaveCurrentTeam = false;
        private Team? showedPlayerTeam;

        private byte[]? newPlayerImg;
        private string newPlayerName;
        private string newFullPlayerName;
        private string newPositionName;
        private string newBirthdayDate;
        public byte[]? NewPlayerImg
        {
            get { return newPlayerImg; }
            set { newPlayerImg = value; OnPropertyChanged(); }
        }
        public string NewPlayerName
        {
            get { return newPlayerName; }
            set { newPlayerName = value; OnPropertyChanged(); }
        }
        public string NewFullPlayerName
        {
            get { return newFullPlayerName; }
            set { newFullPlayerName = value; OnPropertyChanged(); }
        }
        public string NewPositionName
        {
            get { return newPositionName; }
            set { newPositionName = value; OnPropertyChanged(); }
        }
        public string NewBirthdayDate
        {
            get { return newBirthdayDate; }
            set { newBirthdayDate = value; OnPropertyChanged(); }
        }
        public bool IsChanging
        {
            get => isChanging;
            set
            {
                isChanging = value; OnPropertyChanged();
            }
        }
        private Team selectedTeam;
        public Team SelectedTeam
        {
            get => selectedTeam;
            set { selectedTeam = value; OnPropertyChanged(); }
        }

        private RelayCommand selectPlayerLogoCommand;
        public RelayCommand SelectPlayerLogoCommand
        {
            get
            {
                return selectPlayerLogoCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var openedImage = ImageConverter.OpenAndLoadImage();
                        if (openedImage is byte[] imgByte)
                        {
                            NewPlayerImg = imgByte;
                        }
                    }
                    catch (Exception ex)
                    {
                        StatusService.Instance.SetStatusMessage(ex.Message);
                    }
                });
            }
        }
        public ObservableCollection<Player>? TeamPlayers { get; set; }
        public double AKD 
        {
            get { return aKD; }
            set { aKD = value; OnPropertyChanged(); }
        }
        private RelayCommand? editChangeTeamCommand;
        public RelayCommand? EditChangeTeamCommand
        {
            get
            {
                return editChangeTeamCommand ??= new RelayCommand((obj) =>
                {
                    NewPlayerImg = ShowedPlayer.PlayerImg;
                    IsChanging = true;
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
                            StatusService.Instance.SetStatusMessage(ex.ToString());
                        }
                        

                    }));
            }
        }
        private RelayCommand? dismissPlayerConfirmCommand;
        public RelayCommand? DismissPlayerConfirmCommand
        {
            get
            {
                return dismissPlayerConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        IsChanging = false;

                        NewPlayerName = string.Empty;
                        NewFullPlayerName = string.Empty;
                        NewPositionName = string.Empty;
                        NewPlayerImg = null; 
                        NewBirthdayDate = string.Empty;

                        StatusService.Instance.SetStatusMessage("Changes dismissed successfully");
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }

        private RelayCommand? addPlayerConfirmCommand;
        public RelayCommand? AddPlayerConfirmCommand
        {
            get
            {
                return addPlayerConfirmCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        bool isUpdated = false;
                        Player tmpPlayer = ShowedPlayer;

                        if (!string.IsNullOrEmpty(NewPlayerName) && NewPlayerName != ShowedPlayer.PlayerName)
                        {
                            tmpPlayer.PlayerName = NewPlayerName;
                            isUpdated = true;
                        }

                        if (!string.IsNullOrEmpty(NewFullPlayerName) && NewFullPlayerName != ShowedPlayer.PlayerRealName)
                        {
                            tmpPlayer.PlayerRealName = NewFullPlayerName;
                            isUpdated = true;
                        }

                        if (!string.IsNullOrEmpty(NewPositionName) && NewPositionName != ShowedPlayer.Position)
                        {
                            tmpPlayer.Position = NewPositionName;
                            isUpdated = true;
                        }

                        if (NewPlayerImg != ShowedPlayer.PlayerImg)
                        {
                            tmpPlayer.PlayerImg = NewPlayerImg;
                            isUpdated = true;
                        }
                        if (SelectedTeam != null && ShowedPlayer.CurTeamId != SelectedTeam.TeamId)
                        {
                            tmpPlayer.CurTeamId = SelectedTeam.TeamId;
                            isUpdated = true;
                        }
                        if (!string.IsNullOrEmpty(NewBirthdayDate))
                        {
                            DateTime tmpDate;
                            if (!DateTime.TryParse(NewBirthdayDate, out tmpDate))
                            {
                                throw new ArgumentException("Invalid birth date");
                            }
                            tmpPlayer.BirthDayDate = tmpDate.ToUniversalTime();
                        }

                        if (isUpdated)
                        {
                            ShowedPlayer.UpdatedAt = DateTime.UtcNow;
                            uow.Players.Update(ShowedPlayer);
                            uow.Save();
                            ContentNavigationService.Instance.SwitchCurrentContentView(new PlayerPageView(uow.Players.GetById(ShowedPlayer.PlayerId)));
                            StatusService.Instance.SetStatusMessage("Player updated successfully");
                        }
                        else
                        {
                            StatusService.Instance.SetStatusMessage("No changes were made");
                        }
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
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
        public ObservableCollection<Team> AvailableTeams { get; set; }
        public PlayerPageVM(Player player)
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;
            ShowedPlayer = uow.Players.GetById(player.PlayerId);
            int amountOfMatches = uow.Statistics.GetAll().Where(a => a.PlayerId == ShowedPlayer.PlayerId).Count();
            double AKDSum = 0;
            if (amountOfMatches == 0)
            {
                AKD = 0;
            }
            else
            {
                foreach (var item in uow.Statistics.GetAll().Where(a => a.PlayerId == ShowedPlayer.PlayerId))
                {
                    if (item.PlayerKD == "P")
                    {
                        AKDSum += 1.5;
                        continue;
                    }
                    double tmpAKD = double.Parse(item.PlayerKD);
                    AKDSum += tmpAKD;
                }
                AKD = (double)AKDSum / amountOfMatches;
            }
            
            FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starEmpty.png");
            if (CurrentUser != null && CurrentUser.FavPlayerId == ShowedPlayer.PlayerId)
            {
                IsFavoritePlayer = true;
                FavoriteIcon = ImageConverter.LoadImageAsByteArray("pack://application:,,,/Resources/Images/starFill.png");
            }
            AvailableTeams = new ObservableCollection<Team>(uow.Teams.GetAll().Where(a => a.Players.Count() < 5)
                .Except(uow.Teams.GetAll().Where(a => a.TeamId == ShowedPlayer.CurTeamId)));
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
