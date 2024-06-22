namespace BlApi;

// Interface for managing tasks in the business logic layer.
public interface ITask
{
    // Creates a new task in the system and returns its ID.
    public int Create(BO.Task item);

    // Reads the details of a task based on its ID.
    public BO.Task? Read(int id);

    // Retrieves all tasks in the system, optionally filtered by a provided condition.
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter);

    // Updates the details of an existing task in the system.
    public void Update(BO.Task item);

    // Deletes a task from the system based on its ID.
    public void Delete(int id);
}
