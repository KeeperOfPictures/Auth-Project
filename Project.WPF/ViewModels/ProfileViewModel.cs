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
    public class ProfileViewModel : ViewModelBase
    {
        private readonly AuthenticationStore _authenticationStore;

        public string Username => _authenticationStore.CurrentUser?.DisplayName ?? string.Empty;
        public string Email => _authenticationStore.CurrentUser?.Email ?? string.Empty;
        public bool IsEmailVerified => _authenticationStore.CurrentUser?.IsEmailVerified ?? false;

        public ICommand SendEmailVerificationCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        public ProfileViewModel(AuthenticationStore authenticationStore, INavigationService homeNavigationService)
        {
            _authenticationStore = authenticationStore;
            SendEmailVerificationCommand = new SendVerificationEmailCommand(authenticationStore);
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
        }
    }
}
