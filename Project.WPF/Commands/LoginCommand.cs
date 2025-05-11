using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using Project.WPF.Stores;
using Project.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly AuthenticationStore _authenticationStore;
        private readonly INavigationService _homeNavigationService;

        public LoginCommand(LoginViewModel loginViewModel, AuthenticationStore authenticationStore, INavigationService homeNavigationService)
        {
            _loginViewModel = loginViewModel;
            _authenticationStore = authenticationStore;
            _homeNavigationService = homeNavigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
               await _authenticationStore.Login(_loginViewModel.Email, _loginViewModel.Password);
                
                MessageBox.Show("Successfully logged in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _homeNavigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Login failed. Please check your information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
