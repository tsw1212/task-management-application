namespace BlApi;

// Interface for managing engineers in the business logic layer.
public interface IEngineer
{
    // Creates a new engineer in the system.
    public int Create(BO.Engineer item);

    // Reads the details of an engineer based on their ID.
    public BO.Engineer? Read(int id);

    // Reads all engineers in the system, optionally applying a filter.
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

    // Updates the details of an existing engineer in the system.
    public void Update(BO.Engineer item);

    // Deletes an engineer from the system based on their ID.
    public void Delete(int id);
}
