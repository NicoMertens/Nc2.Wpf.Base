namespace Nc2.Wpf.Base.Demo
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Nc2.Wpf.Base.Commands;
    using Nc2.Wpf.Base.ViewModels;

    public class DeferredCommandViewModel : BaseViewModel
    {
        public DeferredCommandViewModel()
        {
            DeferredCommand = new DeferredCommand(TimeSpan.FromSeconds(3), DoWhatever);
            DeferredCommand.CommandExecuting += DeferredCommand_CommandExecuting;
            DeferredCommand.CommandExecuted += DeferredCommand_CommandExecuted;
        }

        public DeferredCommand DeferredCommand { get; }
        public Int32 DeferredCommandExecutionCounter
        {
            get => GetValue<Int32>();
            set => SetValue(value);
        }

        public String DeferredCommandState
        {
            get => GetValue<String>(() => "...");
            set => SetValue(value);
        }

        private Task DoWhatever(CancellationToken cancellationToken)
        {
            DeferredCommandExecutionCounter++;
            return Task.CompletedTask;
        }

        private void DeferredCommand_CommandExecuted(Object sender, EventArgs e)
        {
            DeferredCommandState = "Waiting";
        }

        private void DeferredCommand_CommandExecuting(Object sender, EventArgs e)
        {
            DeferredCommandState = "Running";
        }

    }
}