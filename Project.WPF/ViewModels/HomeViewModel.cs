using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using Project.WPF.Commands;
using Project.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.WPF.ViewModels
{

    public class HomeViewModel : ViewModelBase
    {
        private readonly AuthenticationStore _authenticationStore;

        private string _message;

        public string Username => _authenticationStore.CurrentUser?.DisplayName ?? "Unknown";

        public string Message
        {
            get { return _message; }
            set {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ICommand LoadMessageCommand { get; }
        public ICommand NavigateProfileCommand { get; }
        public ICommand LogoutCommand { get; }

        public HomeViewModel(AuthenticationStore authenticationStore, Queries.IGetMessageQuery getMessageQuery, INavigationService profileNavigationService, INavigationService loginavigationService)
        {
            _authenticationStore = authenticationStore;

            LoadMessageCommand = new LoadMessageCommand(this, getMessageQuery);
            NavigateProfileCommand = new NavigateCommand(profileNavigationService);
            LogoutCommand = new LogoutCommand(authenticationStore, loginavigationService);
        }

        public static HomeViewModel LoadViewModel(AuthenticationStore authenticationStore, Queries.IGetMessageQuery getMessageQuery, INavigationService profileNavigationService, INavigationService loginavigationService)
        {
            var homeViewModel = new HomeViewModel(authenticationStore, getMessageQuery, profileNavigationService, loginavigationService);
            homeViewModel.LoadMessageCommand.Execute(null);
            return homeViewModel;
        }
    }
}
