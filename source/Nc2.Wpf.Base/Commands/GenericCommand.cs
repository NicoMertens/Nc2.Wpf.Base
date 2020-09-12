namespace Nc2.Wpf.Base.Commands
{
    using System;
    using System.Windows.Input;

    public class Command<T> : ICommand
    {
        public Command(Action<T> executeAction, Func<T, Boolean> canExecuteAction)
        {
            ExecuteAction = executeAction;
            CanExecuteAction = canExecuteAction;
        }

        public event EventHandler CanExecuteChanged;

        private Action<T> ExecuteAction { get; }
        private Func<T, Boolean> CanExecuteAction { get; }

        public Boolean CanExecute(Object parameter) 
            => CanExecuteAction?.Invoke((T)parameter) ?? true;
        public void Execute(Object parameter) 
            => ExecuteAction?.Invoke((T)parameter);

        public void Invalidate()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
