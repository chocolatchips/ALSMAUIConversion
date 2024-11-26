using System.Diagnostics;
using WinFormConversion.ViewModels;

namespace WinFormConversion.Views;

public partial class EmployeeView : ContentPage, IView
{
    public IViewModel ViewModel { get => BindingContext as IViewModel; set => BindingContext = value; }

    public EmployeeView()
	{
        ViewModel = new EmployeeViewModel(this);
        Debug.WriteLine($"BindingContext -> {BindingContext?.GetType().Name}");
        SetAlerts();
        InitializeComponent();
    }

    private void SetAlerts()
    {
        ((EmployeeViewModel)ViewModel).AlertTriggered += async (title, message) =>
        {
            await DisplayAlert(title, message, "OK");
        };
        ((EmployeeViewModel)ViewModel).VerificationTriggered += async (title, message) =>
        {
            return await DisplayAlert(title, message, "Yes", "No");
        };
    }
    public void Close()
    {
        Application.Current?.Quit();
    }

    public bool? ShowDialog()
    {
        throw new NotImplementedException();
    }
}