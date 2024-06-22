using BO;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for CreatingScheduleWindow.xaml
    /// </summary>
    /// 
   
    public partial class CreatingScheduleWindow : Window
    {
       public event EventHandler CreatingScheduleChanged;
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;

        public CreatingScheduleWindow()
        {
            InitializeComponent();
        }

        private void click_saveAndCreate(object sender, RoutedEventArgs e)
        {
            if(StartDate==null|| EndDate==null)
            {
                MessageBox.Show("You need to fill all the dates.", "Missing Dates", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                try
                {
                    Tools.InitDateScheduleTime(StartDate??DateTime.Now, EndDate?? DateTime.Now);
                    s_bl.Milestone.Create();
                    MessageBox.Show("Operation completed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    createMileseton.iscreated=true;
                    CreatingScheduleChanged(true, EventArgs.Empty);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($" {ex.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
         
        }
    }
}
