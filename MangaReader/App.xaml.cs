﻿using MangaReader.Services;
using MangaReader.Stores;
using MangaReader.Utilities;
using MangaReader.ViewModel;
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

        public ServiceProvider ServiceProvider { get { return _serviceProvider; } }

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainVM>()
            });

            services.AddSingleton<MangaService>();
            services.AddSingleton<GenreStore>();
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
        protected async override void OnStartup(StartupEventArgs e)
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
            else
            {
                ResourceDictionary lightModeResources = new()
                {
                    Source = new Uri("Themes/LightModeResources.xaml", UriKind.Relative)
                };
                Resources.MergedDictionaries.Add(lightModeResources);
            }

            //initialize the view models
            _serviceProvider.GetRequiredService<MangaDetailVM>();
            _serviceProvider.GetRequiredService<ReadSceneVM>();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            //initialize async functions in view models
            await _serviceProvider.GetRequiredService<MainVM>().InitializeAsync();

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
