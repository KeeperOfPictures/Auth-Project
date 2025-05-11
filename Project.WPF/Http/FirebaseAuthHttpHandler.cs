using Project.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Project.WPF.Http
{
    public class FirebaseAuthHttpHandler : DelegatingHandler
    {
        private readonly AuthenticationStore _authStore;

        public FirebaseAuthHttpHandler(AuthenticationStore authStore)
        {
            _authStore = authStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var firebaseAuthLink = await _authStore.GetFreshAuthAsync();

            if(firebaseAuthLink != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", firebaseAuthLink.FirebaseToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
