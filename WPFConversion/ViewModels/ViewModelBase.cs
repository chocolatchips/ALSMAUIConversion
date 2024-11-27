using System;
using System.ComponentModel;
using System.Windows;
using WPFConversion.Views;
using IView = WPFConversion.Views.IView;

namespace WPFConversion.ViewModels
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
            CloseCommand = new DelegateCommand(Close, CanClose);

            _View = view;

            if (view != null)
            {
                View.ViewModel = this;
            }
        }

        #region Close
        public DelegateCommand CloseCommand { get; }

        private bool CanClose() => true;

        /// <inheritdoc/>
        public void Close()
        {
            View?.Close();
        }
        #endregion

        /// <summary>
        /// Sets a value to a field. If it's a new value, raises PropertyChanged event.
        /// Returns whether the field value changed.
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

        /// <summary>
        /// Uses <see cref="TriggerAlert(string, string)"/>" to display exception alert to user./>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="methodName"></param>
        public void ShowError(Exception ex, string methodName)
        {
            TriggerAlert("Error", $"ERROR - {GetType().Name}.{methodName}\n{ex.Message}");
        }

        /// <summary>
        /// Event triggered to display an alert with a title and message.
        /// Used for user alerts.
        /// </summary>
        public event Action<string, string> AlertTriggered;
        /// <summary>
        /// Invokes <see cref="AlertTriggered"/> for subscribed events.
        /// Used to alert user with a title and message.
        /// </summary>
        /// <param name="title">Title of alert.</param>
        /// <param name="message">Message of alert.</param>
        protected void TriggerAlert(string title, string message)
        {
            AlertTriggered?.Invoke(title, message);
        }

        /// <summary>
        /// Event triggered to display an alert with a title and message.
        /// Used for user confirmation alerts.
        /// </summary>
        public event Func<string, string, Task<bool>>? VerificationTriggered;
        /// <summary>
        /// Invokes <see cref="VerificationTriggered"/> for subscribed eventst.
        /// Used to request user verification with a title and message.
        /// </summary>
        /// <param name="title">Title of verification alert.</param>
        /// <param name="message">Message of verification alert.</param>
        /// <returns>A <see cref="Task{Boolean}"/> of async function.
        /// Returns true if user confirms; otherwise false.</returns>
        protected async Task<bool> TriggerVerificationAlert(string title, string message)
        {
            if (VerificationTriggered != null)
            {
                return await VerificationTriggered.Invoke(title, message);
            }
            return false;
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
