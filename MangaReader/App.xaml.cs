using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;
using MangaReader.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Windows;

namespace MangaReader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public IConfiguration Configuration { get; private set; }

        public ServiceProvider ServiceProvider { get { return _serviceProvider; } }

        public App()
        {
            IServiceCollection services = new ServiceCollection();


            services.AddSingleton(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainVM>()
            });

            services.AddSingleton<MangaService>();
            services.AddSingleton<MangaStore>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IChapterIteratorService, ChapterIteratorService>();
            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
            services.AddSingleton<MainVM>();
            services.AddSingleton<LoadSceneVM>();
            services.AddSingleton<MangasDisplayVM>();
            services.AddSingleton<MangaDetailVM>();
            services.AddSingleton<ReadSceneVM>();

            _serviceProvider = services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            bool isDarkThemeEnabled = IsSystemDarkThemeEnabled();

            if (isDarkThemeEnabled)
            {
                ResourceDictionary darkModeResources = new()
                {
                    Source = new Uri("Themes/DarkModeResources.xaml", UriKind.Relative)
                };
                Resources.MergedDictionaries.Add(darkModeResources);
            }

            //initialize the viewmodels
            _serviceProvider.GetRequiredService<MangaDetailVM>();
            _serviceProvider.GetRequiredService<ReadSceneVM>();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        private static bool IsSystemDarkThemeEnabled()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key != null)
            {
                object? registryValue = key.GetValue("AppsUseLightTheme");
                int themeValue;

                if (registryValue != null && registryValue is int intValue)
                {
                    themeValue = intValue;
                    return themeValue == 0;
                }
            }
            return true;
        }
    }

}
