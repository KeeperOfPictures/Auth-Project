﻿using Firebase.Auth;
using MVVMEssentials.Commands;
using MVVMEssentials.Services;
using MVVMEssentials.ViewModels;
using Project.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.WPF.ViewModels
{
    public class PasswordResetViewModel : ViewModelBase
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ICommand SendPasswordResetEmailCommand { get; }
        public ICommand NavigateLoginCommand { get; }

        public PasswordResetViewModel(FirebaseAuthProvider firebaseAuthProvider, INavigationService loginNavigationService)
        {
            SendPasswordResetEmailCommand = new SendPasswordRessetEmailCommand(this, firebaseAuthProvider, loginNavigationService);
            NavigateLoginCommand = new NavigateCommand(loginNavigationService);
        }
    }
}
