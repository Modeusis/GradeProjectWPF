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
    internal class PlayersVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsAdmin => UserService.Instance.Admin;
        Func<Player, bool> findFunc;
        Func<Player, object> orderFunc = a => a.PlayerId;
        private int currentPlayersPage = 1;
        private int amountOfPlayersPages = 1;
        private bool isOrdered = false;
        public bool IsOrdered
        {
            get => isOrdered;
            set
            {
                isOrdered = value;
                OnPropertyChanged();
                CurrentPlayersPage = 1;
                LoadPlayers(CurrentPlayersPage, 6);
            }
        }
        public int CurrentPlayersPage
        {
            get => currentPlayersPage;
            set { currentPlayersPage = value; OnPropertyChanged(); }
        }
        public int AmountOfPlayersPages
        {
            get => amountOfPlayersPages;
            set { amountOfPlayersPages = value; OnPropertyChanged(); }
        }
        private string currentText;
        public string CurrentText
        {
            get => currentText;
            set
            {
                currentText = value;
                OnPropertyChanged();
                findFunc = (txt => txt.PlayerName.Contains(currentText, StringComparison.OrdinalIgnoreCase));
                CurrentPlayersPage = 1;
                LoadPlayers(CurrentPlayersPage, 6);
            }
        }
        private RelayCommand? currentOrderCommand;
        public RelayCommand? CurrentOrderCommand
        {
            get
            {
                return currentOrderCommand ??= new RelayCommand((obj) =>
                {
                    if (!IsOrdered)
                    {
                        orderFunc = a => a.PlayerName;
                        IsOrdered = true;
                    }
                    else
                    {
                        orderFunc = a => a.PlayerId;
                        IsOrdered = false;
                    }
                });
            }
        }

        public ObservableCollection<Player> SortedPlayers { get; set; }
        private Player? _selectedItem;
        public Player? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
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
                        if (obj is Player Player) ContentNavigationService.Instance.SwitchCurrentContentView(new PlayerPageView(Player));
                    }));
            }
        }
        private RelayCommand? nextPageCommand;
        public RelayCommand? NextPageCommand
        {
            get
            {
                return nextPageCommand ??= new RelayCommand((obj) =>
                {
                    if (CurrentPlayersPage < AmountOfPlayersPages)
                    {
                        CurrentPlayersPage++;
                        LoadPlayers(CurrentPlayersPage, 6);
                    }
                });
            }
        }

        private RelayCommand? deletePlayerCommand;
        public RelayCommand? DeletePlayerCommand
        {
            get
            {
                return deletePlayerCommand ??= new RelayCommand((obj) =>
                {
                    if (obj is Player Player)
                    {
                        StatusService.Instance.SetStatusMessage($"Deleting Player: {Player.PlayerName}");
                        uow.Players.Delete(Player);
                        uow.Save();
                        UserService.Instance.RenewCurrentUser(uow.Users.GetById(CurrentUser.UserId));
                        LoadPlayers(CurrentPlayersPage, 6);
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
                    if (CurrentPlayersPage > 1)
                    {
                        CurrentPlayersPage--;
                        LoadPlayers(CurrentPlayersPage, 6);
                    }
                });
            }
        }
        public PlayersVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            SortedPlayers = new ObservableCollection<Player>();

            if (uow.Players.GetAll().Count() > 0)
            {
                LoadPlayers(CurrentPlayersPage, 6);
            }
        }
        private void LoadPlayers(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.Players.Get(pageNumber, pageSize, orderFunc, findFunc);

                SortedPlayers.Clear();
                foreach (var comment in result.items)
                {
                    SortedPlayers.Add(comment);
                }

                amountOfPlayersPages = result.TotalPages;
                CurrentPlayersPage = pageNumber;
            }
            catch (Exception ex)
            {
                StatusService.Instance.SetStatusMessage($"Error loading comments: {ex.Message}");
            }
        }
        private void OnContentChanged()
        {
            OnPropertyChanged(nameof(CurrentContentView));
        }
        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsAdmin));
        }
    }
}

