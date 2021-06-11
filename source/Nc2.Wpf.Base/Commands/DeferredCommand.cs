namespace Nc2.Wpf.Base.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class DeferredCommand : ICommand
    {
        public delegate Task ActionDelegate(CancellationToken cancellationToken);

        public DeferredCommand(TimeSpan timeOut, ActionDelegate action, Func<Boolean> canAction = null)
        {
            TimeOut = timeOut;
            Action = action;
            CanAction = canAction;
        }

        public event EventHandler CanExecuteChanged;

        public TimeSpan TimeOut { get; }
        public ActionDelegate Action { get; }
        public Func<Boolean> CanAction { get; }
        private ContextSource Context { get; set; }

        public void Invalidate()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public Boolean CanExecute(Object parameter)
        {
            return CanAction?.Invoke() ?? true;
        }

        public async void Execute(Object parameter)
        {
            if (Action is null)
            {
                return;
            }

            var context = Context;
            if (context is not null)
            {
                context.Cancel();
            }

            using (Context = context = new ContextSource())
            {
                try
                {
                    await Task.Delay(TimeOut, context.Token);
                    if (context.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await Action.Invoke(context.Token);
                }
                catch
                {
                    // DO NOTHING
                }
            }
        }

        public class ContextSource : IDisposable
        {
            public ContextSource()
            {
                CancellationTokenSource = new CancellationTokenSource();
            }

            private CancellationTokenSource CancellationTokenSource { get; set; }
            public CancellationToken Token => CancellationTokenSource.Token;
            public Boolean IsRunning => CancellationTokenSource is not null;

            public void Cancel()
            {
                if (IsRunning)
                {
                    lock (CancellationTokenSource)
                    {
                        if (IsRunning)
                        {
                            CancellationTokenSource.Cancel();
                        }
                    }
                }
            }

            public void Dispose()
            {
                if (IsRunning)
                {
                    lock (CancellationTokenSource)
                    {
                        CancellationTokenSource.Dispose();
                        CancellationTokenSource = null;
                    }
                }
            }
        }
    }
}
