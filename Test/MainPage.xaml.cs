using System;
using Windows.UI.Popups;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Test.ServiceReference1;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        ServiceReference1.EmployeeClient webservice = new ServiceReference1.EmployeeClient();
        public MainPage()
        {
            
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
           
        }

       void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            getEmployee();
        }

         async void getEmployee()
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                GridViewEmployee.ItemsSource = await webservice.GetProducListAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                
            }
        }

        private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                Employee newEmployee = new Employee();
                newEmployee.empID = Int32.Parse(TextBoxNo.Text);
                newEmployee.firstName = TextBoxName.Text;
                newEmployee.lastName = TextBoxSurName.Text;
                newEmployee.Age = Int32.Parse(TextBoxAge.Text);
                newEmployee.Address = TextBoxCity.Text;
                bool result = await webservice.AddEmployeeAsync(newEmployee);
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                if (result == true)
                {
                    MessageDialog messageDialog = new MessageDialog("Inserted successfully!");
                    await messageDialog.ShowAsync();
                    Reset();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Can't Insert");
                    await messageDialog.ShowAsync();
                }
                getEmployee();

            }
            catch (Exception ex)
            {

                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }

        private void Reset()
        {
            TextBoxName.Text = string.Empty;
            TextBoxSurName.Text = string.Empty;
            TextBoxCity.Text = string.Empty;
            TextBoxAge.Text = string.Empty;
        }

       

        private async void ButtonModify_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar.IsIndeterminate = true;
                ProgressBar.Visibility = Visibility.Visible;
                Employee newEmployee = new Employee();
                newEmployee.empID = (GridViewEmployee.SelectedItem as Employee).empID;
                newEmployee.firstName = TextBoxName.Text;
                newEmployee.lastName = TextBoxSurName.Text;
                newEmployee.Age = Int32.Parse(TextBoxAge.Text);
                newEmployee.Address = TextBoxCity.Text;
                bool result = await webservice.UpdateEmployeeAsync(newEmployee);
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                if (result == true)
                {
                    MessageDialog messageDialog = new MessageDialog("Edited successfully!");
                    await messageDialog.ShowAsync();
                    Reset();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("Can't Edit");
                    await messageDialog.ShowAsync();
                }
                getEmployee();

            }
            catch (Exception ex)
            {

                MessageDialog messageDialog = new MessageDialog("Choise Employee");
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
            }
        }

        private async void GridViewEmployee_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count !=0)
                {
                    Employee selectedEmployee = e.AddedItems[0] as Employee;
                    TextBoxName.Text = selectedEmployee.firstName;
                    TextBoxSurName.Text = selectedEmployee.lastName;
                    TextBoxCity.Text = selectedEmployee.Address;
                    TextBoxAge.Text = selectedEmployee.Age.ToString();
                }
            }
            catch (Exception ex)
            {

                MessageDialog messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                ProgressBar.Visibility = Visibility.Collapsed;
                ProgressBar.IsIndeterminate = false;
                

            }
        }

        private async void ButtonDelete1_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (GridViewEmployee.SelectedItem != null)
            {
                try
                {
                    ProgressBar.IsIndeterminate = true;
                    ProgressBar.Visibility = Visibility.Visible;
                    bool result = await webservice.DeleteEmployeeAsync((GridViewEmployee.SelectedItem as Employee).empID);
                    if (result == true)
                    {
                        MessageDialog messageDialog = new MessageDialog("Deleted successfully!");
                        await messageDialog.ShowAsync();
                        Reset();
                    }
                    else
                    {
                        MessageDialog messageDialog = new MessageDialog("Can't delete!");
                        await messageDialog.ShowAsync();
                    }
                    getEmployee();
                }
                catch (Exception ex)
                {

                    MessageDialog messageDialog = new MessageDialog(ex.Message);
                    await messageDialog.ShowAsync();
                    ProgressBar.Visibility = Visibility.Collapsed;
                    ProgressBar.IsIndeterminate = false;
                }
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Choice record to delete");
                await messageDialog.ShowAsync();
            }
        }
    }
}
