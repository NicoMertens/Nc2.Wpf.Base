namespace Nc2.Wpf.Core.Demo
{
    using System;
    using System.Windows.Input;
    using Nc2.Wpf.Base.Commands;
    using Nc2.Wpf.Base.ViewModels;

    public class DemoViewModel : BaseViewModel
    {
        public DemoViewModel()
        {
            DoWhateverCommand = new Command(DoWhatever, CanDoWhatever);
        }
        
        public Command DoWhateverCommand { get; }

        private void DoWhatever(Object parameter)
        {
            // DO whatever
        }

        private Boolean CanDoWhatever(Object parameter)
        {
            return true;
        }

        private void DoSomething()
        {
            // Trigger the CanExecuteEvent
            DoWhateverCommand.Invalidate();
        }

        public Int32 Number
        {
            get => GetValue<Int32>();
            set => SetValue(value);
        }

        public DateTime Date
        {
            get => GetValue<DateTime>(() => DateTime.Now); // Set initial value
            set => SetValue(value);
        }

        public String FirstName
        {
            get => GetValue<String>();
            set
            {
                SetValue(value);
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public String LastName
        {
            get => GetValue<String>();
            set
            {
                SetValue(value);
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public String DisplayName
        {
            get => $"{FirstName} {LastName}";
        }
    }
}
