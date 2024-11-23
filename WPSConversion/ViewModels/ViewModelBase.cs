using System;
using System.ComponentModel;
using System.Windows;
using WPSConversion.Views;

namespace WPSConversion.ViewModels
{
    public class ViewModelBase<TView> : IViewModel
        where TView : class, IView
    {
        private readonly TView _View;
        /// <summary>Associated view</summary>
        protected TView View => _View;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase(TView view)
        {
            CloseCommand = new DelegateCommand(Close);

            _View = view;

            if (view != null)
            {
                View.ViewModel = this;
            }
        }

        #region Close
        public DelegateCommand CloseCommand { get; }

        /// <inheritdoc/>
        public void Close()
        {
            View?.Close();
        }
        #endregion

        /// <summary>
        /// Sets a value to a field. If it's a new value, raises PropertyChanged event.
        /// Returs whether the field value changed.
        /// </summary>
        /// <typeparam name="T">Field type</typeparam>
        /// <param name="field">Backing field variable (not the public property)</param>
        /// <param name="value">New value to set</param>
        /// <param name="propertyName">Name of the public property (use nameof)</param>
        /// <returns>Whether the field value changed</returns>
        protected bool SetNotifyableProperty<T>(ref T field, T value, string propertyName)
        {
            bool valueChanged = !Equals(field, value);

            if (valueChanged)
            {
                field = value;
                RaisePropertyChanged(propertyName);
            }

            return valueChanged;
        }

        /// <inheritdoc/>
        public bool? ShowDialog()
        {
            return View?.ShowDialog();
        }

        public void ShowError(Exception ex, string methodName)
        {
            MessageBox.Show(View as Window, $"ERROR - {GetType().Name}.{methodName}\n{ex.Message}");
        }

        /// <summary>
        /// Raises PropertyChanged for the speficied property name
        /// </summary>
        /// <param name="propertyName">Name of the property whos value changed</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
