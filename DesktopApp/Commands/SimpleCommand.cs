using System;
using System.Windows.Input;

namespace FaceDetector.Commands
{
    public class SimpleCommand
        : ICommand
    {
        private readonly Action<object> _action;
        private readonly Action<Exception> _errorHandler;
        private bool _canExecute;

        public SimpleCommand(Action<object> action, Action<Exception> errorHandler = null)
        {
            _canExecute = true;
            _action = action ?? throw new ArgumentNullException($"{nameof(action)} cannot be null");
            _errorHandler = errorHandler;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            try
            {
                _canExecute = false;
                CanExecuteChanged?.Invoke(this, new EventArgs());

                _action(parameter);
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

        public event EventHandler CanExecuteChanged;
    }
}
