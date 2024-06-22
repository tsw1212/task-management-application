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

namespace PL.Milestone;

/// <summary>
/// Interaction logic for MilestoneWindow.xaml
/// </summary>
public partial class MilestoneWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Business Logic API
    static int idState = 0;
    public ObservableCollection<BO.MilestoneInList> milestoneList = null; // List of engineers
    public BO.Milestone ContentMilestone
    {
        get { return (BO.Milestone)GetValue(MilestoneProperty); }
        set { SetValue(MilestoneProperty, value); }
    }

    public static readonly DependencyProperty MilestoneProperty =
        DependencyProperty.Register("ContentMilestone", typeof(BO.Milestone), typeof(MilestoneWindow), new PropertyMetadata(null));
    public MilestoneWindow(ObservableCollection<BO.MilestoneInList> listMilestone, int Id = 0)
    {
        InitializeComponent();
        if (Id == 0)
        {
            ContentMilestone = new BO.Milestone();
            milestoneList = listMilestone;
        }
        else
        {
            ContentMilestone = s_bl.Milestone.Read(Id);
            idState = Id;
        }
    }

    private void save_click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (idState == 0)
            {
                //  s_bl.Milestone.Create(ContentMilestone);
                BO.MilestoneInList milestoneToAdd = Tools.fromMilestoneToMilestoneInList(ContentMilestone);
                milestoneList.Add(milestoneToAdd);
                MessageBox.Show("Engineer created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                s_bl.Milestone.Update(ContentMilestone.Id, ContentMilestone.Alias, ContentMilestone.Description, ContentMilestone.Remarks);
                BO.MilestoneInList MilestoneToUpdate = milestoneList.First(m => m.Id == idState);
                milestoneList.Remove(MilestoneToUpdate);
                BO.MilestoneInList milestoneToAdd = Tools.fromMilestoneToMilestoneInList(ContentMilestone);
                milestoneList.Add(milestoneToAdd);
                MessageBox.Show("Engineer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }
        catch (BlNullPropertyException ex)
        {
            MessageBox.Show($"You must fill in ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (InvalidInputException ex)
        {
            MessageBox.Show($"Invalid input: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (BlAlreadyExistsException ex)
        {
          //  MessageBox.Show($"An engineer with such ID already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
