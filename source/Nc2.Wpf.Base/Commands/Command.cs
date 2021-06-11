namespace Nc2.Wpf.Base.Commands
{
    using System;
    using System.Windows.Input;

    public class Command : ICommand
    {
        public Command(Action<Object> executeAction, Func<Object, Boolean> canExecuteAction)
        {
            ExecuteActionWithParameter = executeAction;
            CanExecuteActionWithParameter = canExecuteAction;
        }

        public Command(Action executeAction, Func<Boolean> canExecuteAction)
        {
            ExecuteAction = executeAction;
            CanExecuteAction = canExecuteAction;
        }

        public event EventHandler CanExecuteChanged;

        private Action<Object> ExecuteActionWithParameter { get; }
        private Func<Object, Boolean> CanExecuteActionWithParameter { get; }

        public Action ExecuteAction { get; }
        public Func<Boolean> CanExecuteAction { get; }

        public Boolean CanExecute(Object parameter)
        {
            if (CanExecuteAction is not null)
            {
                return CanExecuteAction.Invoke();
            }

            if (CanExecuteActionWithParameter is not null)
            {
                return CanExecuteActionWithParameter.Invoke(parameter);
            }

            return true;
        }

        public void Execute(Object parameter)
        { 
            if (ExecuteAction is not null)
            {
                ExecuteAction.Invoke();
                return;
            }

            if (ExecuteActionWithParameter is not null)
            {
                ExecuteActionWithParameter.Invoke(parameter);
                return;
            }
        }

        public void Invalidate()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
