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