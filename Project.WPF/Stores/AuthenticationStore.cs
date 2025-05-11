using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.WPF.Stores
{
    public class AuthenticationStore
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private FirebaseAuthLink _authLink;

        public User? CurrentUser => _authLink?.User;
        public bool IsAuthenticated => (!_authLink?.IsExpired()) ?? false;

        public async Task Initialize()
        {
            var firebaseAuthJson = Properties.Settings.Default.FirebaseAuth;
            if (String.IsNullOrEmpty(firebaseAuthJson)) return;
            var firebaseAuth = JsonSerializer.Deserialize<FirebaseAuth>(firebaseAuthJson); 

            if(firebaseAuth == null) return;
            
            _authLink = new FirebaseAuthLink(_firebaseAuthProvider, firebaseAuth);

            await GetFreshAuthAsync();
        }

        public AuthenticationStore(FirebaseAuthProvider firebaseAuthProvider)
        {
            _firebaseAuthProvider = firebaseAuthProvider;
        }

        public async Task Login(string email, string password)
        {
            _authLink = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
            SaveAuthenticationState();
        }
       

        public void Logout()
        {
            _authLink = null;
            ClearAuthenticationState();
        }
        private void SaveAuthenticationState()
        {
            var firebaseAuthLinkJson = JsonSerializer.Serialize(_authLink);
            Properties.Settings.Default.FirebaseAuth = firebaseAuthLinkJson;
            Properties.Settings.Default.Save();
        }

        private static void ClearAuthenticationState()
        {
            Properties.Settings.Default.FirebaseAuth = null;
            Properties.Settings.Default.Save();
        }

        public async Task<FirebaseAuthLink> GetFreshAuthAsync()
        {
            if (_authLink == null)
                return null;

            _authLink = await _authLink?.GetFreshAuthAsync();
            SaveAuthenticationState();
            return _authLink;
        }

        public async Task SendEmailVerificationEmail()
        {
            if (_authLink == null) throw new Exception("User is not authenticated");

            await GetFreshAuthAsync();

            await _firebaseAuthProvider.SendEmailVerificationAsync(_authLink.FirebaseToken);
        }
    }
}
