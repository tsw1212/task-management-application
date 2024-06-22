using PL.Engineer;
using PL.UserEngineer;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }

     

        private void clickHandelEngineer(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }
        //private void clickInitDb(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult result = MessageBox.Show("are you sure you want to init db ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        s_bl.InitializeDB();

        //    }
        //}


        //private void clickResetDb(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult result = MessageBox.Show("are you sure you want to reset db ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        s_bl.ResetDB();
        //    }
        //}

        private void click_EngineerEntry(object sender, RoutedEventArgs e)
        {
            
            new engineerIdWindow().Show();
            this.Close();
        }

        private void click_AdminEntry(object sender, RoutedEventArgs e)
        {
           
            new AdminScreen().Show();
            this.Close();
        }
    }
}
