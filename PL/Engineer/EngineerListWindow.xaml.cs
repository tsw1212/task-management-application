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

namespace PL.Engineer;

/// <summary>
/// Window class for managing a list of engineers in a WPF application.
/// </summary>
public partial class EngineerListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); // Business Logic API
    public BO.EngineerExperience experience { get; set; } = BO.EngineerExperience.All; // Engineer experience level

    /// <summary>
    /// Constructor for EngineerListWindow.
    /// </summary>
    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = new ObservableCollection<BO.Engineer>(s_bl?.Engineer.ReadAll())!;
    }

    /// <summary>
    /// Collection of engineers displayed in the window.
    /// </summary>
    public ObservableCollection<BO.Engineer> EngineerList
    {
        get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }

    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    /// <summary>
    /// Event handler for the selection change in the experience level dropdown.
    /// </summary>
    private void sortByEngineerExperience(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = new ObservableCollection<BO.Engineer>((experience == BO.EngineerExperience.All) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == experience)!);
    }

    /// <summary>
    /// Event handler for the click event of the add engineer button.
    /// </summary>
    private void AddEngineer_click(object sender, RoutedEventArgs e)
    {
        new EngineerWindow(EngineerList).ShowDialog();
    }

    /// <summary>
    /// Event handler for double-clicking an engineer in the engineer list.
    /// </summary>
    private void Engineer_dubbleClick(object sender, MouseButtonEventArgs e)
    {
        BO.Engineer? EngineerInList = (sender as ListView)?.SelectedItem as BO.Engineer;
        if (EngineerInList != null)
        {
            EngineerWindow ew = new EngineerWindow(EngineerList, EngineerInList.Id);
            ew.ShowDialog();
        }
    }
}
