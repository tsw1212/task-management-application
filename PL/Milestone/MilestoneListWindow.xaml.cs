using PL.Milestone;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.Milestone;

/// <summary>
/// Interaction logic for MailStoneListWindow.xaml
/// </summary>
public partial class MilestoneListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Business Logic API
    public BO.Status status { get; set; } = BO.Status.All; // Engineer experience level


    public MilestoneListWindow()
    {
        InitializeComponent();
        MilestoneList = new ObservableCollection<BO.MilestoneInList>(s_bl?.MilestoneInList.ReadAll())!;

    }
    public ObservableCollection<BO.MilestoneInList> MilestoneList
    {
        get { return (ObservableCollection<BO.MilestoneInList>)GetValue(MilestoneListProperty); }
        set { SetValue(MilestoneListProperty, value); }
    }

    public static readonly DependencyProperty MilestoneListProperty =
        DependencyProperty.Register("MilestoneList", typeof(IEnumerable<BO.MilestoneInList>), typeof(MilestoneListWindow), new PropertyMetadata(null));


    private void SortByMilestoneStatus(object sender, SelectionChangedEventArgs e)
    {
        MilestoneList = new ObservableCollection<BO.MilestoneInList>((status == BO.Status.All) ?
            s_bl?.MilestoneInList.ReadAll()! : s_bl?.MilestoneInList.ReadAll(item => item.Status == status)!);
    }
    private void AddMilestone_click(object sender, RoutedEventArgs e)
    {
        new MilestoneWindow(MilestoneList).ShowDialog();
    }
    private void Milestone_dubbleClick(object sender, MouseButtonEventArgs e)
    {
        BO.MilestoneInList? MilestoneInList = (sender as ListView)?.SelectedItem as BO.MilestoneInList;
        if (MilestoneInList != null)
        {
            MilestoneWindow ew = new MilestoneWindow(MilestoneList, MilestoneInList.Id);
            ew.ShowDialog();
        }
    }

    private void sortByMilestoneStatus(object sender, SelectionChangedEventArgs e)
    {
        MilestoneList = new ObservableCollection<BO.MilestoneInList>((status == BO.Status.All) ?
            s_bl?.MilestoneInList.ReadAll()! : s_bl.MilestoneInList.ReadAll(item => item.Status == status)!);
    }
}
