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
    internal class MatchPageVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsLogin => UserService.Instance.Login;
        public bool IsAdmin => UserService.Instance.Admin;
        private Match match;
        private Team firstTeam;
        private Team secondTeam;
        private Team? tmpTeamWinner;
        private string tmpFScore;
        private string tmpSScore;
        private Statistics tmpStatistics;
        private string tmpStatisticPlayerName;
        private string winnerTeamName;
        private int? scoreFirstTeam;
        private int? scoreSecondTeam;
        private string kills;
        private string deaths;
        public string Deaths
        {
            get { return deaths; }
            set
            {
                deaths = value;
                OnPropertyChanged(nameof(Deaths));
            }
        }

        private string assists;
        public string Assists
        {
            get { return assists; }
            set
            {
                assists = value;
                OnPropertyChanged(nameof(Assists));
            }
        }

        private bool isMatchFinished = false;
        private bool isChanging = false;
        private bool isChangingStatistics = false;
        public string WinnerTeamName
        {
            get { return winnerTeamName; }
            set { winnerTeamName = value; OnPropertyChanged(); }
        }
        public Team? TmpTeamWinner
        {
            get { return tmpTeamWinner; }
            set { tmpTeamWinner = value; OnPropertyChanged(); }
        }

        public string TmpFScore
        {
            get { return tmpFScore; }
            set { tmpFScore = value; OnPropertyChanged(); }
        }

        public string TmpSScore
        {
            get { return tmpSScore; }
            set { tmpSScore = value; OnPropertyChanged(); }
        }
        private RelayCommand? finishMatchCommand;
        private RelayCommand? dismissMatchCommand;
        private RelayCommand? startFinishCommand;

        public RelayCommand FinishMatchCommand
        {
            get
            {
                return finishMatchCommand ??= new RelayCommand((obj) =>
                {
                    try
                    {
                        if (string.IsNullOrEmpty(TmpFScore) || string.IsNullOrEmpty(TmpSScore) || TmpTeamWinner == null)
                        {
                            throw new Exception("Fill all fields");
                        }
                        int tmpFirst;
                        int tmpSecond;
                        if (!int.TryParse(TmpFScore,out tmpFirst) || !int.TryParse(TmpSScore, out tmpSecond))
                        {
                            throw new Exception("Only numbers");
                        }
                        bool isFirst = false;
                        if (TmpTeamWinner.TeamId == Match.FirstTeam.TeamId)
                        {
                            isFirst = true;
                        }
                        if (isFirst && tmpFirst <= tmpSecond)
                        {
                            throw new Exception("Winner cant have less or equal amount of rounds than another team");
                        }
                        Match.WinnerId = TmpTeamWinner.TeamId;
                        Match.ScoreFirstTeam = tmpFirst;
                        Match.ScoreSecondTeam = tmpSecond;
                        Match.Status = true;
                        uow.Matches.Update(Match);
                        uow.Save();
                        ContentNavigationService.Instance.SwitchCurrentContentView(new MatchPageView(uow.Matches.GetById(Match.MatchId)));
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }
        public RelayCommand DismissMatchCommand
        {
            get
            {
                return dismissMatchCommand ??= new RelayCommand((obj) =>
                {
                    try
                    {
                        IsChanging = false;
                        TmpTeamWinner = null;
                        TmpFScore = "";
                        TmpSScore = "";
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }
        public RelayCommand StartFinishCommand
        {
            get
            {
                return startFinishCommand ??= new RelayCommand((obj) =>
                {
                    try
                    {
                        IsChanging = true;
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage(e.Message);
                    }
                });
            }
        }

        public Statistics TmpStatistics
        {
            get { return tmpStatistics; }
            set { tmpStatistics = value; OnPropertyChanged(); }
        }

        public string TmpStatisticPlayerName
        {
            get { return tmpStatisticPlayerName; }
            set { tmpStatisticPlayerName = value; OnPropertyChanged(); }
        }
        public Match Match
        {
            get { return match; }
            set { match = value; OnPropertyChanged(); }
        }
        public Team FirstTeam
        {
            get { return firstTeam; }
            set { firstTeam = value; OnPropertyChanged(); }
        }
        public Team SecondTeam
        {
            get { return secondTeam; }
            set { secondTeam = value; OnPropertyChanged(); }
        }
        public int? ScoreFirstTeam
        {
            get => scoreFirstTeam;
            set { scoreFirstTeam = value; OnPropertyChanged(); }
        }
        public int? ScoreSecondTeam
        {
            get => scoreSecondTeam;
            set { scoreSecondTeam = value; OnPropertyChanged(); }
        }
        public string Kills
        {
            get => kills;
            set { kills = value; OnPropertyChanged(); }
        }
        public bool IsMatchFinished
        {
            get => isMatchFinished;
            set { isMatchFinished = value; OnPropertyChanged(); }
        }
        public bool IsChanging
        {
            get => isChanging;
            set { isChanging = value; OnPropertyChanged(); }
        }
        public bool IsChangingStatistics
        {
            get => isChangingStatistics;
            set { isChangingStatistics = value; OnPropertyChanged(); }
        }
        private RelayCommand? saveStatisticsCommand;
        public RelayCommand? SaveStatisticsCommand
        {
            get
            {
                return saveStatisticsCommand ??
                    (saveStatisticsCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(Kills) || string.IsNullOrEmpty(Deaths) || string.IsNullOrEmpty(Assists))
                            {
                                throw new Exception("Fill all fields");
                            }
                            int tmpKills;
                            int tmpDeaths;
                            int tmpAssists;
                            if (!int.TryParse(Kills, out tmpKills) || !int.TryParse(Deaths, out tmpDeaths) || !int.TryParse(Assists, out tmpAssists))
                            {
                                throw new Exception("Only numbers");
                            }
                            if (tmpKills < 0 || tmpDeaths < 0 || tmpAssists < 0)
                            {
                                throw new Exception("Numbers cant be below zero");
                            }
                            TmpStatistics.PlayerDeaths = tmpDeaths;
                            TmpStatistics.PlayerKills = tmpKills;
                            TmpStatistics.PlayerAssists = tmpAssists;
                            if (tmpDeaths == 0)
                            {
                                TmpStatistics.PlayerKD = "P";
                            }
                            else
                            {
                                double kd = Math.Round((double)tmpKills/ tmpDeaths, 1);
                                TmpStatistics.PlayerKD = kd.ToString();
                            }
                            uow.Statistics.Update(TmpStatistics);
                            uow.Save();
                            ContentNavigationService.Instance.SwitchCurrentContentView(new MatchPageView(uow.Matches.GetById(Match.MatchId)));
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }
        private RelayCommand? deleteMatchCommand;
        public RelayCommand? DeleteMatchCommand
        {
            get
            {
                return deleteMatchCommand ??= new RelayCommand((obj) =>
                {
                    try
                    {
                        uow.Matches.Delete(Match);
                        uow.Save();
                        StatusService.Instance.SetStatusMessage($"Match {Match.FirstTeam.TeamName} vs {Match.SecondTeam.TeamName} deleted");
                        ContentNavigationService.Instance.SwitchCurrentContentView(new MatchesView());
                    }
                    catch (Exception e)
                    {
                        StatusService.Instance.SetStatusMessage($"Error: {e.Message}");
                    }
                });
            }
        }
        private RelayCommand? dismissStatisticsCommand;
        public RelayCommand? DismissStatisticsCommand
        {
            get
            {
                return dismissStatisticsCommand ??
                    (dismissStatisticsCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Team team)
                        {
                            TmpStatistics = null;
                            Kills = "";
                            Deaths = "";
                            Assists = "";
                            TmpStatisticPlayerName = "";
                            IsChangingStatistics = false;
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
        private RelayCommand? toPlayerPageCommand;
        public RelayCommand? ToPlayerPageCommand
        {
            get
            {
                return toPlayerPageCommand ??
                    (toPlayerPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Player player)
                        {
                            ContentNavigationService.Instance.SwitchCurrentContentView(new PlayerPageView(player));
                        }
                    }));
            }
        }
        private Statistics? _selectedFirstTeamStatistic;
        public Statistics? SelectedFirstTeamStatistic
        {
            get => _selectedFirstTeamStatistic;
            set
            {
                if (_selectedFirstTeamStatistic != value)
                {
                    _selectedFirstTeamStatistic = value;
                    OnPropertyChanged();
                }
            }
        }
        private Statistics? _selectedSecondTeamStatistic;
        public Statistics? SelectedSecondTeamStatistic
        {
            get => _selectedSecondTeamStatistic;
            set
            {
                if (_selectedSecondTeamStatistic != value)
                {
                    _selectedSecondTeamStatistic = value;
                    OnPropertyChanged();
                }
            }
        }
        private RelayCommand? changeSecondTeamStatisticsCommand;
        public RelayCommand? ChangeSecondTeamStatisticsCommand
        {
            get
            {
                return changeSecondTeamStatisticsCommand ??
                    (changeSecondTeamStatisticsCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (SelectedSecondTeamStatistic != null)
                            {
                                IsChangingStatistics = true;
                                TmpStatistics = SelectedSecondTeamStatistic;
                                TmpStatisticPlayerName = TmpStatistics.Player.PlayerName;
                            }
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }

        private RelayCommand? changeFirstTeamStatisticsCommand;
        public RelayCommand? ChangeFirstTeamStatisticsCommand
        {
            get
            {
                return changeFirstTeamStatisticsCommand ??
                    (changeFirstTeamStatisticsCommand = new RelayCommand((obj) =>
                    {
                        try
                        {
                            if (SelectedFirstTeamStatistic != null)
                            {
                                IsChangingStatistics = true;
                                TmpStatistics = SelectedFirstTeamStatistic;
                                TmpStatisticPlayerName = TmpStatistics.Player.PlayerName;
                            }
                        }
                        catch (Exception e)
                        {
                            StatusService.Instance.SetStatusMessage(e.Message);
                        }
                    }));
            }
        }

        public ObservableCollection<Statistics> FirstTeamStatistics { get; set; }
        public ObservableCollection<Statistics> SecondTeamStatistics { get; set; }
        public ObservableCollection<Team> Participants { get; set; }
        public MatchPageVM(Match match)
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            FirstTeamStatistics = new ObservableCollection<Statistics>();
            SecondTeamStatistics = new ObservableCollection<Statistics>();
            Participants = new ObservableCollection<Team>();

            Match = uow.Matches.GetAll().FirstOrDefault(m => m.MatchId == match.MatchId);
            ScoreFirstTeam = 0;
            ScoreSecondTeam = 0;
            IsMatchFinished = Match.Status;
            if (IsMatchFinished && Match.WinnerId is int winId)
            {
                ScoreFirstTeam = Match.ScoreFirstTeam;
                ScoreSecondTeam = Match.ScoreSecondTeam;
                WinnerTeamName = uow.Teams.GetById(winId).TeamName;
            }
            FirstTeam = Match.FirstTeam;
            Participants.Add(FirstTeam);
            SecondTeam = Match.SecondTeam;
            Participants.Add(SecondTeam);
            if (uow.Statistics.GetAll().Where(a => a.MatchId == Match.MatchId && a.TeamId == FirstTeam.TeamId).Count() != 5 && FirstTeam.Players.Count() == 5)
            {
                Statistics statistics;
                foreach (Player plr in FirstTeam.Players)
                {
                    statistics = new Statistics();
                    statistics.MatchId = Match.MatchId;
                    statistics.TeamId = FirstTeam.TeamId;
                    statistics.PlayerId = plr.PlayerId;
                    statistics.PlayerKills = 0;
                    statistics.PlayerDeaths = 0;
                    statistics.PlayerAssists = 0;
                    statistics.PlayerKD = "0";
                    uow.Statistics.Add(statistics);
                    uow.Save();
                    FirstTeamStatistics.Add(uow.Statistics.GetAll()
                                       .Where(a => a.PlayerId == statistics.PlayerId && a.MatchId == statistics.MatchId && a.TeamId == statistics.TeamId)
                                       .FirstOrDefault());
                }
            }
            else
            {
                foreach (Statistics statistics in uow.Statistics.GetAll().Where(a => a.MatchId == Match.MatchId && a.TeamId == FirstTeam.TeamId))
                {
                    FirstTeamStatistics.Add(statistics);
                }
            }
            if (uow.Statistics.GetAll().Where(a => a.MatchId == Match.MatchId && a.TeamId == SecondTeam.TeamId).Count() != 5 && SecondTeam.Players.Count() == 5)
            {
                Statistics statistics;
                foreach (Player plr in SecondTeam.Players)
                {
                    statistics = new Statistics();
                    statistics.MatchId = Match.MatchId;
                    statistics.TeamId = SecondTeam.TeamId;
                    statistics.PlayerId = plr.PlayerId;
                    statistics.PlayerKills = 0;
                    statistics.PlayerDeaths = 0;
                    statistics.PlayerAssists = 0;
                    statistics.PlayerKD = "-";
                    uow.Statistics.Add(statistics);
                    uow.Save();
                    SecondTeamStatistics.Add(uow.Statistics.GetAll()
                                       .Where(a => a.PlayerId == statistics.PlayerId && a.MatchId == statistics.MatchId && a.TeamId == statistics.TeamId)
                                       .FirstOrDefault());
                }
            }
            else
            {
                foreach (Statistics statistics in uow.Statistics.GetAll().Where(a => a.MatchId == Match.MatchId && a.TeamId == SecondTeam.TeamId))
                {
                    SecondTeamStatistics.Add(statistics);
                }
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
            OnPropertyChanged(nameof(IsAdmin));
        }
    }
}
