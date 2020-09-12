namespace Nc2.Wpf.Base.Commands
{
    using System;
    using System.Windows.Input;

    public class Command : ICommand
    {
        public Command(Action<Object> executeAction, Func<Object, Boolean> canExecuteAction)
        {
            ExecuteAction = executeAction;
            CanExecuteAction = canExecuteAction;
        }

        public event EventHandler CanExecuteChanged;

        private Action<Object> ExecuteAction { get; }
        private Func<Object, Boolean> CanExecuteAction { get; }

        public Boolean CanExecute(Object parameter) 
            => CanExecuteAction?.Invoke(parameter) ?? true;

        public void Execute(Object parameter) 
            => ExecuteAction?.Invoke(parameter);

        public void Invalidate()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
