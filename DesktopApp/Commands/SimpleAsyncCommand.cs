using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FaceDetector.Commands
{
    public class SimpleAsyncCommand
        : IAsyncCommand
    {
        private readonly Func<object, Task> _function;
        private readonly Action<Exception> _errorHandler;
        private bool _canExecute;

        public SimpleAsyncCommand(Func<object, Task> function, Action<Exception> errorHandler = null)
        {
            _canExecute = true;
            _function = function ?? throw new ArgumentNullException($"{nameof(function)} cannot be null");
            _errorHandler = errorHandler;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _ = ExecuteAsync(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public async Task ExecuteAsync(object parameter)
        {
            try
            {
                _canExecute = false;
                CanExecuteChanged?.Invoke(this, new EventArgs());

                await _function(parameter);
            }
            catch (Exception ex)
            {
                _errorHandler?.Invoke(ex);
            }
            finally
            {
                _canExecute = true;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
