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

	private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var selectedEmployee = e.CurrentSelection.FirstOrDefault() as Employee;
		if (selectedEmployee != null)
		{
		}
	}


    private void NewButton_Clicked(object sender, EventArgs e)
    {
        _viewModel.CreateNewEmployee();
    }
    private void EditButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Edit CLicked");
    }
    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Delete CLicked");
    }
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Save CLicked");
        _viewModel.SaveEmployee(FirstNameEntry.Text, LastNameEntry.Text, UserNameEntry.Text);
    }
    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("Cancel CLicked");
    }
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Application.Current?.Quit();
    }
}