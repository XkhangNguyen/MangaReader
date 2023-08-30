using MangaReader.Utilities;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MangaReader.Services
{
    public interface INavigationService
    {
        public ViewModelBase? CurrentView { get; }
        public void NavigateTo<T>() where T : ViewModelBase;
    }

    internal class NavigationService : ViewModelBase, INavigationService
    {
        private ViewModelBase? _currentView;
        private readonly Func<Type, ViewModelBase> _viewModelFactory;

        public ViewModelBase? CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }
        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}
