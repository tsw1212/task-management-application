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

namespace PL.UserEngineer
{
    /// <summary>
    /// Interaction logic for TaskListToChooseWindoe.xaml
    /// </summary>
    public partial class TaskListToChooseWindoe : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Business Logic API
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //לא מושלם צריך לחשוב על הדרך
            //BO.Task ? taskToUpdate = sender as BO.Task;
            var t = (sender).GetType();
           // BO.Task ? taskToUpdate=s_bl.Task.Read()
            //taskToUpdate.CompleteDate= DateTime.Now;
            //s_bl.Task.Update(taskToUpdate);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //לא מושלם צריך לחשוב על הדרך

            //BO.Task taskToUpdate = sender as BO.Task;
            //taskToUpdate.CompleteDate = null;
            //s_bl.Task.Update(taskToUpdate);
        }

        public ObservableCollection<BO.Task> TasksOfEnginner
        {
            get { return (ObservableCollection<BO.Task>)GetValue(TasksOfEnginnerProperty); }
            set { SetValue(TasksOfEnginnerProperty, value); }
        }

        public static readonly DependencyProperty TasksOfEnginnerProperty =
            DependencyProperty.Register("TasksOfEnginner", typeof(ObservableCollection<BO.Task>), typeof(TaskListToChooseWindoe), new PropertyMetadata(null));
        public TaskListToChooseWindoe(int idEngineer)
        {
            TasksOfEnginner= new ObservableCollection<BO.Task>(s_bl.Task.ReadAll(t=>t.Engineer?.Id== idEngineer));
            InitializeComponent();
        }
    }
}
