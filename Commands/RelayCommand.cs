using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InteractiveNeuralNetworks.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object>? _Execute;
        private Predicate<object>? _CanExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _CanExecute == null ? true : _CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _Execute?.Invoke(parameter);
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _Execute;
        private readonly Predicate<T>? _CanExecute;

        public RelayCommand(Action<T> execute) : this(execute, null) {}

        public RelayCommand(Action<T> execute, Predicate<T>? canExecute)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _CanExecute == null ? true : _CanExecute((T)parameter);
        }

        public void Execute(object? parameter)
        {
            _Execute?.Invoke((T)parameter);
        }  
    }
}
