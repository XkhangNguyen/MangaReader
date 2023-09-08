using System;
using System.Windows.Input;

namespace MangaReader.Utilities
{
    public struct Unit
    {
        // This is an empty struct, so there are no fields or properties.
        // It is used to represent a parameterless function.
    }
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Predicate<T?>? _canExecute;

        public RelayCommand(Action<T?> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (typeof(T) == typeof(Unit) && parameter == null)
            {
                // Allow execution for parameterless function
                return true;
            }
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object? parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
