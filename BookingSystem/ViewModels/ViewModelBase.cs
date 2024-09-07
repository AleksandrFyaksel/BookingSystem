using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingSystem.ViewModels
{
    /// <summary>
    /// Базовый класс для ViewModel, реализующий INotifyPropertyChanged.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged для обновления привязанных свойств.
        /// </summary>
        /// <param name="propertyName">Имя измененного свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Метод изменения свойства с вызовом события PropertyChanged.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="prop">Ссылка на изменяемое свойство.</param>
        /// <param name="value">Новое значение.</param>
        /// <param name="propName">Имя свойства.</param>
        /// <returns>True, если значение изменилось; иначе - false.</returns>
        protected bool Set<T>(ref T prop, T value, [CallerMemberName] string propName = null)
        {
            if (Equals(prop, value))
                return false;

            prop = value;
            OnPropertyChanged(propName);
            return true;
        }
    }
}