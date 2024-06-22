using PL.task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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
/// Interaction logic for userEngineerWindow.xaml
/// </summary>
public partial class userEngineerWindow : Window
{
    int EId;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public string TaskTitle { get; set; }
    public string TaskDescription { get; set; }
    public BO.Engineer e;
    public bool TaskFlag
    {
        get { return (bool)GetValue(TaskFlagProperty); }
        set { SetValue(TaskFlagProperty, value); }
    }

    public static readonly DependencyProperty TaskFlagProperty =
        DependencyProperty.Register("TaskFlag", typeof(bool), typeof(taskWindow), new PropertyMetadata(null));
    public userEngineerWindow(int id)
    {
        EId = id;
        e = s_bl.Engineer.Read(id);
        TaskFlag = e.Task == null;
        TaskTitle = (e.Task?.Id).ToString() ?? "0";
        TaskDescription = e.Task?.Alias ?? "no task";
        InitializeComponent();

    }

    private void ViewEngineerTasks_Click(object sender, RoutedEventArgs e)
    {
        new TaskListToChooseWindoe(EId).Show();
    }

    private void ViewTask_Click(object sender, RoutedEventArgs ev)
    {
        new taskWindow(e.Task?.Id??0).ShowDialog();
    }
}
