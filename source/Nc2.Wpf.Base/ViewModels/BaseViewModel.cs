namespace Nc2.Wpf.Base.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<String, Object> Values { get; } = new Dictionary<String, Object>();

        protected T GetValue<T>(Func<T> factory, [CallerMemberName] String propertyName = null)
        {
            if (!Values.ContainsKey(propertyName))
            {
                if (factory == null)
                {
                    return default;
                }

                Values[propertyName] = factory();
            }

            return (T)Values[propertyName];
        }

        protected T GetValue<T>([CallerMemberName] String propertyName = null)
        {
            return GetValue<T>(null, propertyName);
        }

        protected void SetValue(Object value, [CallerMemberName] String propertyName = null)
        {
            Values[propertyName] = value;
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
