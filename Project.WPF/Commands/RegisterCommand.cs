using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using Project.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.WPF.Commands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private readonly INavigationService _loginNavigationService;

        public RegisterCommand(RegisterViewModel registerViewModel, FirebaseAuthProvider firebaseAuthProvider, INavigationService loginNavigationService)
        {
            _registerViewModel = registerViewModel;
            _firebaseAuthProvider = firebaseAuthProvider;
            _loginNavigationService = loginNavigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            var password = _registerViewModel.Password;
            var confitmPassword = _registerViewModel.ConfirmPassword;

            if (password != confitmPassword)
            {
                MessageBox.Show("Passwords must match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                await _firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(
                    _registerViewModel.Email,
                    password,
                    _registerViewModel.Username);

                MessageBox.Show("Successfully registered!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                _loginNavigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Registration failed. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}
