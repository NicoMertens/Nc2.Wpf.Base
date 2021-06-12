namespace Nc2.Wpf.Base.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class DeferredCommand : ICommand
    {
        public delegate Task ActionCallback(CancellationToken cancellationToken);
        public delegate Boolean CanActionCallback();

        public DeferredCommand(TimeSpan timeOut, ActionCallback action, CanActionCallback canAction = null)
        {
            TimeOut = timeOut;
            Action = action;
            CanAction = canAction;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler CommandExecuting;
        public event EventHandler CommandExecuted;
        public event EventHandler<Exception> ExceptionThrowed;

        public TimeSpan TimeOut { get; }
        public ActionCallback Action { get; }
        public CanActionCallback CanAction { get; }
        public Boolean IsRunning => ExecutingCounter > 0;
        private CancellationTokenSource CancellationTokenSource { get; set; }
        

        private Object ContextLock { get; } = new Object();
        private Object ExecutingCounterLock { get; } = new Object();
        private Int32 ExecutingCounter { get; set; }

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
            if (Action is null || !CanExecute(parameter))
            {
                return;
            }

            try
            {
                IncrementExecutionCounter();
                var localContext = CreateContext();
                try
                {
                    await Task.Delay(TimeOut, localContext.Token).ContinueWith(task => { }); // ContinueWith avoids TaskCancelledException
                    if (!localContext.Token.IsCancellationRequested)
                    {
                        await Action.Invoke(localContext.Token);
                    }
                }
                finally
                {
                    DisposeContext(localContext);
                }
            }
            catch (Exception exception)
            {
                if (exception is not TaskCanceledException)
                {
                    OnExceptionThrowed(exception);
                }
            }
            finally
            {
                DecrementExecutionCounter();
            }
        }

        public void Cancel()
        {
            lock (ContextLock)
            {
                if (CancellationTokenSource is not null)
                {
                    CancellationTokenSource.Cancel();
                }
            }
        }

        private CancellationTokenSource CreateContext()
        {
            lock (ContextLock)
            {
                if (CancellationTokenSource is not null)
                {
                    CancellationTokenSource.Cancel();
                }

                return CancellationTokenSource = new CancellationTokenSource();
            }
        }

        private void DisposeContext(CancellationTokenSource cancellationTokenSouce)
        {
            lock (ContextLock)
            {
                cancellationTokenSouce.Dispose();
                if (CancellationTokenSource == cancellationTokenSouce)
                {
                    CancellationTokenSource = null;
                }
            }
        }

        private void IncrementExecutionCounter()
        {
            lock (ExecutingCounterLock)
            {
                ExecutingCounter++;
                if (ExecutingCounter == 1)
                {
                    OnCommandExecuting();
                }
            }
        }

        private void DecrementExecutionCounter()
        {
            lock (ExecutingCounterLock)
            {
                ExecutingCounter--;
                if (ExecutingCounter == 0)
                {
                    OnCommandExecuted();
                }
            }
        }

        protected virtual void OnCommandExecuting()
        {
            CommandExecuting?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnCommandExecuted()
        {
            CommandExecuted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnExceptionThrowed(Exception exception)
        {
            ExceptionThrowed?.Invoke(this, exception);
        }
    }
}
