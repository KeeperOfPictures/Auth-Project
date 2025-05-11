using MVVMEssentials.Commands;
using Project.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.WPF.Commands
{
    public class SendVerificationEmailCommand : AsyncCommandBase
    {

        private readonly AuthenticationStore _authenticationStore;

        public SendVerificationEmailCommand(AuthenticationStore authenticationStore)
        {
            _authenticationStore = authenticationStore;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _authenticationStore.SendEmailVerificationEmail();

                MessageBox.Show("Successfully sent email verification.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed sent email verification.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
