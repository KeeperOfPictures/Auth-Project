using MVVMEssentials.Commands;
using Project.WPF.Queries;
using Project.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.WPF.Commands
{
    internal class LoadMessageCommand : AsyncCommandBase
    {
        private readonly HomeViewModel _homeViewModel;
        private readonly IGetMessageQuery _getMessageQuery;

        public LoadMessageCommand(HomeViewModel homeViewModel, IGetMessageQuery getMessageQuery)
        {
            _homeViewModel = homeViewModel;
            _getMessageQuery = getMessageQuery;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var messageResponse = await _getMessageQuery.Execute();

                _homeViewModel.Message = messageResponse.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load message. Please try later", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
    }
}
