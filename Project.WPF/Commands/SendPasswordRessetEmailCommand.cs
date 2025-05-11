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
    public class SendPasswordRessetEmailCommand : AsyncCommandBase
    {
        private readonly PasswordResetViewModel _passwordResetViewModel;
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private readonly INavigationService _loginNavigationService;

        public SendPasswordRessetEmailCommand(PasswordResetViewModel passwordResetViewModel, FirebaseAuthProvider firebaseAuthProvider, INavigationService loginNavigationService)
        {
            _passwordResetViewModel = passwordResetViewModel;
            _firebaseAuthProvider = firebaseAuthProvider;
            _loginNavigationService = loginNavigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            var email = _passwordResetViewModel.Email;
            try
            {
                await _firebaseAuthProvider.SendPasswordResetEmailAsync(email);

                MessageBox.Show("Successfully sent password reset. Check your email to reset.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _loginNavigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to reset password. Please try again later", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
