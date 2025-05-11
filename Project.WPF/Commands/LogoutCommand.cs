using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using Project.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.WPF.Commands
{
    public class LogoutCommand : CommandBase
    {
        private readonly AuthenticationStore _authenticationStore;
        private readonly INavigationService _loginavigationService;

        public LogoutCommand(AuthenticationStore authenticationStore, INavigationService loginavigationService)
        {
            _authenticationStore = authenticationStore;
            _loginavigationService = loginavigationService;
        }

        public override void Execute(object parameter)
        {
            _authenticationStore.Logout();

            _loginavigationService.Navigate();
        }
    }
}
