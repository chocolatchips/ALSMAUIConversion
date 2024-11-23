using System.ComponentModel;

namespace WPSConversion.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Closes the window
        /// </summary>
        void Close();

        /// <summary>
        /// Shows the window in a modal way
        /// </summary>
        /// <returns>Dialog result</returns>
        bool? ShowDialog();
    }
}
