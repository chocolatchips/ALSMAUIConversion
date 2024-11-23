using WPSConversion.ViewModels;

namespace WPSConversion.Views
{
    public interface IView
    {
        /// <summary>Associated ViewModel</summary>
        IViewModel ViewModel { get; set; }

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
