using System;
using System.Windows.Input;

namespace Fruithread.Commands
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action executeAction, Func<bool> canExecuteFunc = null)
        {
            if (executeAction == null) throw new ArgumentNullException(nameof(executeAction));
            ExecuteAction = executeAction;
            CanExecuteFunc = canExecuteFunc;
        }

        private Func<bool> CanExecuteFunc { get; }

        private Action ExecuteAction { get; }

        public bool CanExecute(object parameter) => CanExecuteFunc?.Invoke() ?? true;

        public void Execute(object parameter) => ExecuteAction();

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}