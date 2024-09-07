using System;
using System.Windows.Input;

namespace BookingSystem.Commands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> executeMethod; 
        private readonly Func<T, bool> canExecute; 
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecute = null)
        {
            this.executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            if (parameter is T param) 
            {
                executeMethod(param);
            }
            else
            {
                throw new ArgumentException($"Invalid parameter type. Expected {typeof(T)}.");
            }
        }
    }
}