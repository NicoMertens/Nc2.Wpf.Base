# Introduction 
    Commands, ViewModels, Converters

# BooleanToVisibilityConverter

    <converters:BooleanToVisibilityConverter x:Key="BooleanToHiddenConverter" 
                                             Invert="False" 
                                             Collapsed="False"/>
    <converters:BooleanToVisibilityConverter x:Key="BooleanToCollapsedConverter" 
                                             Invert="False" 
                                             Collapsed="True"/>
    <converters:BooleanToVisibilityConverter x:Key="InvertedBooleanToHiddenConverter" 
                                             Invert="True" 
                                             Collapsed="False"/>
    <converters:BooleanToVisibilityConverter x:Key="InvertedBooleanToCollapsedConverter" 
                                             Invert="True" 
                                             Collapsed="True"/>

# BooleanToColorConverter

    <converters:BooleanToColorConverter x:Key="ColorConverter" 
                                        Invert="False" 
                                        TrueColor="Lime" 
                                        FalseColor="Red" 
                                        NullColor="Blue" />
    <converters:BooleanToColorConverter x:Key="InvertedColorConverter" 
                                        Invert="True" 
                                        TrueColor="Lime" 
                                        FalseColor="Red"/>     

# BooleanToValueConverter

    <converters:BooleanToValueConverter x:Key="ValueConverter" Invert="False"
                                        TrueValue="{x:Static FontWeights.Bold}"
                                        FalseValue="{x:Static FontWeights.Normal}"
                                        NullValue="{x:Static FontWeights.Light}"/>
    <converters:BooleanToValueConverter x:Key="InvertedValueConverter" Invert="True"
                                        TrueValue="{x:Static FontWeights.Bold}"
                                        FalseValue="{x:Static FontWeights.Normal}"/>

# BaseViewModel

    using System;
    using Nc2.Wpf.Base.ViewModels;

    public class DemoViewModel : BaseViewModel
    {
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

# Command

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

# Deferred Command

Executes a command after a timeout. Further calls replace the pending command.

DeferredCommand = new DeferredCommand(TimeSpan.FromSeconds(3), DoWhatever, CanDoWhatever);