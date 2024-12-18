using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TournamentsApplication.Model;
using TournamentsApplication.Utility;
using TournamentsApplication.View;
using static System.Net.Mime.MediaTypeNames;

namespace TournamentsApplication.ViewModel
{
    internal class TournamentsVM : ViewModelBase
    {
        private UnitOfWork uow;
        public UserControl? CurrentContentView => ContentNavigationService.Instance.CurrentContentView;
        public User? CurrentUser => UserService.Instance.CurrentUser;
        public bool IsAdmin => UserService.Instance.Admin;
        Func<Tournament, bool> findFunc;
        private int currentTournamentsPage = 1;
        private int amountOfTournamentsPages = 1;
        public int CurrentTournamentsPage
        {
            get => currentTournamentsPage;
            set { currentTournamentsPage = value; OnPropertyChanged(); }
        }
        public int AmountOfTournamentsPages
        {
            get => amountOfTournamentsPages;
            set { amountOfTournamentsPages = value; OnPropertyChanged(); }
        }
        private string currentText;
        public string CurrentText
        {
            get => currentText;
            set
            { 
                currentText = value;
                OnPropertyChanged();
                findFunc = (txt => txt.TournamentName.Contains(currentText, StringComparison.OrdinalIgnoreCase));
                CurrentTournamentsPage = 1;
                LoadTournaments(CurrentTournamentsPage, 4);
            }
        }
        public ObservableCollection<Tournament> SortedTournaments { get; set; }
        private Tournament? _selectedItem;
        public Tournament? SelectedItem
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

        private RelayCommand? toTournamentPageCommand;
        public RelayCommand? ToTournamentPageCommand
        {
            get
            {
                return toTournamentPageCommand ??
                    (toTournamentPageCommand = new RelayCommand((obj) =>
                    {
                        if (obj is Tournament tournament) ContentNavigationService.Instance.SwitchCurrentContentView(new TournamentPageView(tournament));
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
                    if (CurrentTournamentsPage < AmountOfTournamentsPages)
                    {
                        CurrentTournamentsPage++;
                        LoadTournaments(CurrentTournamentsPage, 4);
                    }
                });
            }
        }
        
        private RelayCommand? deleteTournamentCommand;
        public RelayCommand? DeleteTournamentCommand
        {
            get
            {
                return deleteTournamentCommand ??= new RelayCommand((obj) =>
                {
                    if (obj is Tournament tournament)
                    {
                        StatusService.Instance.SetStatusMessage($"Deleting tournament: {tournament.TournamentName}");
                        uow.Tournaments.Delete(tournament);
                        uow.Save();
                        UserService.Instance.RenewCurrentUser(uow.Users.GetById(CurrentUser.UserId));
                        LoadTournaments(CurrentTournamentsPage, 4);
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
                    if (CurrentTournamentsPage > 1)
                    {
                        CurrentTournamentsPage--;
                        LoadTournaments(CurrentTournamentsPage, 4);
                    }
                });
            }
        }
        public TournamentsVM()
        {
            uow = new UnitOfWork(new ApplicationContext());
            ContentNavigationService.Instance.NavigationChanged += OnContentChanged;
            UserService.Instance.UserChanged += OnUserChanged;

            SortedTournaments = new ObservableCollection<Tournament>();

            if (uow.Tournaments.GetAll().Count() > 0)
            {
                LoadTournaments(CurrentTournamentsPage, 4);
            }
        }
        private void LoadTournaments(int pageNumber, int pageSize)
        {
            try
            {
                var result = uow.Tournaments.Get(pageNumber, pageSize, a => a.TournamentId, findFunc);

                SortedTournaments.Clear();
                foreach (var comment in result.items)
                {
                    SortedTournaments.Add(comment);
                }

                amountOfTournamentsPages = result.TotalPages;
                CurrentTournamentsPage = pageNumber;
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
