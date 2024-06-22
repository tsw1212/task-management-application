using BO;
using PL.Engineer;
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

namespace PL.task;

/// <summary>
/// Interaction logic for taskListWindow.xaml
/// </summary>
public partial class taskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Business Logic API
    public BO.Status status { get; set; } = BO.Status.All; // task status
    private void TaskChanged(object sender, EventArgs e)
    {
        AddOrUpdateEvent changeEvent=e as AddOrUpdateEvent;

        if (changeEvent.EventState == "Add")
        {
            TaskList.Add(sender as BO.TaskInList);
        }
        else
        {
            BO.TaskInList taskToUpdate=sender as BO.TaskInList;
            BO.TaskInList TasktoUpdate = TaskList.First(t => t.Id == taskToUpdate.Id);
            TaskList.Remove(TasktoUpdate);
            TaskList.Add(taskToUpdate);
        }
    }
    public ObservableCollection<BO.TaskInList> TaskList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(taskListWindow), new PropertyMetadata(null));
    public taskListWindow()
    {
        InitializeComponent();
        TaskList = new ObservableCollection<BO.TaskInList>(s_bl?.TaskInList.ReadAll())!;
    }
    private void SortByMilestoneStatus(object sender, SelectionChangedEventArgs e)
    {
        TaskList = new ObservableCollection<BO.TaskInList>((status == BO.Status.All) ?
            s_bl?.TaskInList.ReadAll()! : s_bl?.TaskInList.ReadAll(item => item.Status == status)!);
    }
    private void Task_doubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.TaskInList? TaskInList = (sender as ListView)?.SelectedItem as BO.TaskInList;
        if (TaskInList != null)
        {
            taskWindow ew = new taskWindow(TaskInList.Id);
            ew.TaskChanged += TaskChanged;
            ew.ShowDialog();
        }
    }
    private void AddTask_click(object sender, RoutedEventArgs e)
    {
        taskWindow ew = new taskWindow();
        ew.TaskChanged += TaskChanged;
        ew.ShowDialog();
    }
}
