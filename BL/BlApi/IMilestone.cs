namespace BlApi;

// Interface for managing milestones in the business logic layer.
public interface IMilestone
{
    // Creates a new milestone in the system.
    public void Create();

    // Reads the details of a milestone based on its ID.
    public BO.Milestone? Read(int id);

    // Updates the details of an existing milestone in the system.
    // Returns the updated milestone.
    public BO.Milestone Update(int id, string alias, string description, string? remarks);
}
