using System;
using System.Windows.Input;

namespace GitFlowVS.Extension.ViewModels
{
    public class CommandHandler : ICommand
    {
        private readonly Action action;
        private readonly bool canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }
    }
}