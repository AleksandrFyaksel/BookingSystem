using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingSystem.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> executeMethod;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncRelayCommand(Func<object, Task> executeMethod, Func<object, bool> canExecute = null)
        {
            this.executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        private async Task ExecuteAsync(object parameter) 
        {
            try
            {
                await executeMethod(parameter);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
            }
        }
    }
}