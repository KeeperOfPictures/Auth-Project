using Firebase.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVMEssentials.Services;
using MVVMEssentials.Stores;
using MVVMEssentials.ViewModels;
using Project.WPF.Queries;
using Project.WPF.Stores;
using Project.WPF.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using Refit;
using Project.WPF.Http;

namespace Project.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, serviceCollection) =>
                {
                    var firebaseApiKey = context.Configuration.GetValue<string>("FIREBASE_API_KEY");

                    serviceCollection.AddSingleton(new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey)));

                    serviceCollection.AddTransient<FirebaseAuthHttpHandler>();

                    serviceCollection.AddRefitClient<IGetMessageQuery>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(context.Configuration.GetValue<string>("MESSAGE_API_BASE_URL")))
                    .AddHttpMessageHandler<FirebaseAuthHttpHandler>();

                    serviceCollection.AddSingleton<NavigationStore>();
                    serviceCollection.AddSingleton<ModalNavigationStore>();
                    serviceCollection.AddSingleton<AuthenticationStore>();

                    serviceCollection.AddSingleton<NavigationService<RegisterViewModel>>(
                        services => new NavigationService<RegisterViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new RegisterViewModel(
                            services.GetRequiredService<FirebaseAuthProvider>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>())));

                    serviceCollection.AddSingleton<NavigationService<LoginViewModel>>(
                        services => new NavigationService<LoginViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new LoginViewModel(
                            services.GetRequiredService<AuthenticationStore>(),
                            services.GetRequiredService<NavigationService<RegisterViewModel>>(),
                            services.GetRequiredService<NavigationService<HomeViewModel>>(),
                            services.GetRequiredService<NavigationService<PasswordResetViewModel>>())));

                    serviceCollection.AddSingleton<NavigationService<HomeViewModel>>(
                        services => new NavigationService<HomeViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => HomeViewModel.LoadViewModel(
                            services.GetRequiredService<AuthenticationStore>(),
                            services.GetRequiredService<IGetMessageQuery>(),
                            services.GetRequiredService<NavigationService<ProfileViewModel>>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>())));

                    serviceCollection.AddSingleton<NavigationService<PasswordResetViewModel>>(
                        services => new NavigationService<PasswordResetViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new PasswordResetViewModel(
                            services.GetRequiredService<FirebaseAuthProvider>(),
                            services.GetRequiredService<NavigationService<LoginViewModel>>())));

                    serviceCollection.AddSingleton<NavigationService<ProfileViewModel>>(
                        services => new NavigationService<ProfileViewModel>(
                        services.GetRequiredService<NavigationStore>(),
                        () => new ProfileViewModel(
                            services.GetRequiredService<AuthenticationStore>(),
                            services.GetRequiredService<NavigationService<HomeViewModel>>())));


                    serviceCollection.AddSingleton<MainViewModel>();

                    serviceCollection.AddSingleton<MainWindow>((services) => new MainWindow()
                    {
                        DataContext = services.GetRequiredService<MainViewModel>()
                    }); 
                })
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await Initialize();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private async Task Initialize()
        {
            var authenticationStore = _host.Services.GetService<AuthenticationStore>();

            try
            {
                await authenticationStore.Initialize();

                if (authenticationStore.IsAuthenticated)
                {
                    INavigationService navigationService = _host.Services.GetRequiredService<NavigationService<HomeViewModel>>();
                    navigationService.Navigate();
                }
                else
                {
                    INavigationService navigationService = _host.Services.GetRequiredService<NavigationService<LoginViewModel>>();
                    navigationService.Navigate();
                }
            }
            catch (FirebaseAuthException)
            {
                INavigationService navigationService = _host.Services.GetRequiredService<NavigationService<LoginViewModel>>();
                navigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load app.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
