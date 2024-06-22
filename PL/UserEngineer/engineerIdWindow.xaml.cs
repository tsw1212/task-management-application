using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.UserEngineer;

/// <summary>
/// Interaction logic for engineerIdWindow.xaml
/// </summary>
public partial class engineerIdWindow : Window
{
    public int Id { get; set; }
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public engineerIdWindow()
    {
        InitializeComponent();
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
       try
        {
            s_bl.Engineer.Read(Id);
            new userEngineerWindow(Id).Show();
            this.Close();
        }
        catch (Exception ex) 
        {
            MessageBox.Show("A user with such an ID does not exist in the system", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
