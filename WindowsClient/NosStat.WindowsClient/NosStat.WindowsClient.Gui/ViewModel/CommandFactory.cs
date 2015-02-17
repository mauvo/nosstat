using System;
using System.Windows.Input;

namespace NosStat.WindowsClient.Gui.ViewModel
{
    public static class CommandFactory
    {
        public static ICommand CreateCommand<T>(Action<T> executeAction)
        {
            return CreateCommand(executeAction, arg => true);
        }

        public static ICommand CreateCommand<T>(Action<T> executeAction, Func<T, bool> canExecuteAction)
        {
            return new Command<T>(executeAction, canExecuteAction);
        }

        public static ICommand CreateCommand<T>(Action<T> executeAction, Func<T, bool> canExecuteAction, out Action canExecuteChangedAction)
        {
            Command<T> command = new Command<T>(executeAction, canExecuteAction);
            canExecuteChangedAction = command.OnCanExecuteChanged;
            return command;
        }

        public static ICommand CreateCommand(Action executeAction)
        {
            return CreateCommand(executeAction, () => true);
        }

        public static ICommand CreateCommand(Action executeAction, Func<bool> canExecuteAction)
        {
            return new Command(executeAction, canExecuteAction);
        }

        public static ICommand CreateCommand(Action executeAction, Func<bool> canExecuteAction, out Action canExecuteChangedAction)
        {
            Command command = new Command(executeAction, canExecuteAction);
            canExecuteChangedAction = command.OnCanExecuteChanged;
            return command;
        }

        private class Command<T> : ICommand
        {
            private Func<T, bool> _canExecuteAction;
            private Action<T> _executeAction;

            internal Command(Action<T> executeAction, Func<T, bool> canExecuteAction)
            {
                _canExecuteAction = canExecuteAction;
                _executeAction = executeAction;
            }

            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return _canExecuteAction((T)parameter);
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _executeAction((T)parameter);
            }

            #endregion

            internal void OnCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        private class Command : ICommand
        {
            private Func<bool> _canExecuteAction;
            private Action _executeAction;

            public Command(Action executeAction, Func<bool> canExecuteAction)
            {
                _canExecuteAction = canExecuteAction;
                _executeAction = executeAction;
            }

            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return _canExecuteAction();
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _executeAction();
            }

            #endregion

            public void OnCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
    }

}
