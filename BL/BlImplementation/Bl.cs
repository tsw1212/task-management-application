using BlApi;
using BO;

namespace BlImplementation;

// Business logic implementation class that implements the IBl interface.
public class Bl : IBl
{
    // Property to access the engineer-related functionality in the system.
    public IEngineer Engineer => new EngineerImplementation();

    // Property to access the milestone-related functionality in the system.
    public IMilestone Milestone => new MilestoneImplementation();

    public IMilestoneInList MilestoneInList => new MilestoneInListImplementation();

    // Property to access the task-related functionality in the system.
    public ITask Task => new TaskImplementation();
    public ITaskInList TaskInList => new TaskInListlementation();
    //a function that inits the data base
    public void InitializeDB() => DalTest.Initialization.Do();
    //a function that resets the all data base
    public void ResetDB() => DalTest.Initialization.Reset();
    

}
