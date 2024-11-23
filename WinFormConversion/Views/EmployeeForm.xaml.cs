using System.ComponentModel;
using WinFormConversion.Entities;
using WinFormConversion.ViewModels;

namespace WinFormConversion.Views;

public partial class EmployeeForm : ContentPage
{
	EmployeeViewModel _viewModel;
	public EmployeeForm()
	{
		InitializeComponent();
		_viewModel = new();
		BindingContext = _viewModel;
        EmployeeCollection.ItemsSource = _viewModel.Employees;
	}

    private void NewButton_Clicked(object sender, EventArgs e)
    {
        _viewModel.CreateNewEmployee();
    }
    private void EditButton_Clicked(object sender, EventArgs e)
    {
        _viewModel.EditEmployee();
    }
    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var res = await VerifyDeleteAlert();
        if (res)
            _viewModel.DeleteEmployee();
    }
    private async Task<bool> VerifyDeleteAlert()
    {
        if (_viewModel.SelectedEmployee != null)
        {
            string userDeleteMess = $"Are you sure you want to delete \'{_viewModel.SelectedEmployee.Username}\'?";
            bool result = await DisplayAlert("Employee Management", userDeleteMess, "Yes", "No");
            return result;
        }
        return false;
    }
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        string errorMessage = _viewModel.IsValidEmployee(FirstNameEntry.Text, LastNameEntry.Text, UserNameEntry.Text);
        if (!string.IsNullOrWhiteSpace(errorMessage))
            AlertNotValid(errorMessage);
        else
            _viewModel.SaveEmployee(FirstNameEntry.Text, LastNameEntry.Text, UserNameEntry.Text);
    }
    private async void AlertNotValid(string errorMessage)
    {
        await DisplayAlert("Employee Management", errorMessage, "OK");
    }
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        _viewModel.CancelEdit();
    }
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Application.Current?.Quit();
    }
}