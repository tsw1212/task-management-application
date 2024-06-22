using PL.Engineer;
using PL.Milestone;
using PL.task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL;

/// <summary>
/// Interaction logic for AdminScreen.xaml
/// </summary>
public partial class AdminScreen : Window
{

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private void CreatingScheduleChanged(object sender, EventArgs e)
    {
        createMileseton.iscreated = true;
        CreateScheduleFlag =createMileseton.iscreated;
    }

    public bool CreateScheduleFlag
    {
        get { return (bool)GetValue(CreateScheduleFlagProperty); }
        set { SetValue(CreateScheduleFlagProperty, value); }
    }

    public static readonly DependencyProperty CreateScheduleFlagProperty =
        DependencyProperty.Register("CreateScheduleFlag", typeof(bool), typeof(AdminScreen), new PropertyMetadata(null));
    public AdminScreen()
    {
        CreateScheduleFlag = createMileseton.iscreated;
        InitializeComponent();
    }

    private void click_engineers(object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().ShowDialog();
    }
    //public bool createScheduleFlag { get; set; } = createMileseton.iscreated;
   


    private void click_tasks(object sender, RoutedEventArgs e)
    {

            new taskListWindow().ShowDialog();
       
    }

    private void click_gant(object sender, RoutedEventArgs e)
    {
        new ganttChartWindow().ShowDialog();
    }
    
    private void click_InitDb(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("are you sure you want to init db ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            s_bl.InitializeDB();

        }
    }
    private void click_ResetDb(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("are you sure you want to reset db ", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            s_bl.ResetDB();
        }
    }

    private void click_createSchedule(object sender, RoutedEventArgs e)
    {
        var csw = new CreatingScheduleWindow();
        csw.CreatingScheduleChanged += CreatingScheduleChanged;
        csw.ShowDialog();
    }
   
    private void click_milestones(object sender, RoutedEventArgs e)
    {
      new MilestoneListWindow().ShowDialog();
    }
}
