namespace BlApi;

// The main interface for the business logic layer.
public interface IBl
{
    // Gets the interface for managing engineers in the system.
    public IEngineer Engineer { get; }

    // Gets the interface for managing milestones in the system.
    public IMilestone Milestone { get; }

    public IMilestoneInList MilestoneInList { get; }

    // Gets the interface for managing tasks in the system.
    public ITask Task { get; }
     
    public ITaskInList TaskInList { get; }
    public void InitializeDB();
    public void ResetDB();
 
}
