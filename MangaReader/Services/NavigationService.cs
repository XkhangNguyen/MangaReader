using MangaReader.Utilities;
using System;

namespace MangaReader.Services
{
    public interface INavigationService
    {
        public ViewModelBase? CurrentView { get; }
        public ViewModelBase? PrevView { get; }
        public void NavigateTo<T>() where T : ViewModelBase;
        public void NavigateBack();
    }

    internal class NavigationService : ViewModelBase, INavigationService
    {
        private ViewModelBase? _currentView;
        private ViewModelBase? _previousView;
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

        public ViewModelBase? PrevView
        {
            get => _previousView;
            private set
            {
                _previousView = value;
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
            PrevView = CurrentView;
            CurrentView = viewModel;
        }

        public void NavigateBack()
        {
            if (PrevView != null)
            {
                CurrentView = PrevView; 
                PrevView = null;
            }
        }
    }
}
