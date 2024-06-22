using BlApi;
using BO;


namespace BlImplementation;
/// <summary>
/// Implementation of the <see cref="IMilestone"/> interface providing operations for milestones.
/// </summary>
public class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates milestones based on the start and end dates of the project.
    /// </summary>
    /// <exception cref="BlNoDatesForProject">Thrown if the program is missing a start or end date for the project.</exception>
    public void Create()
    {
        DateTime ?startProject = _dal.StartProject;
        DateTime? endProject = _dal.EndProject;
        if (startProject != null && endProject != null)
        {
            List<DO.Dependency > newDependencies = Tools.CreateMileStone(_dal.Dependency.ReadAll().ToList(), startProject, endProject);
            _dal.Dependency.Reset();
            foreach (DO.Dependency depent in newDependencies)
            {
                _dal.Dependency.Create(depent);
            }
            Tools.CalculationTimes(_dal.Dependency.ReadAll().ToList(), startProject ?? DateTime.Now, endProject??DateTime.Now);
            Tools.significantNames();
        }
        else { throw new BlNoDatesForProject("the program is missing a start or end date for the project"); }
      
    }

    /// <summary>
    /// Retrieves a milestone with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the milestone to be retrieved.</param>
    /// <returns>The business object representation of the retrieved milestone.</returns>
    /// <exception cref="BlDoesNotExistException">Thrown if the milestone does not exist or is not a milestone.</exception>
    public BO.Milestone? Read(int id)
    {
        try
        {
            DO.Task? doTask = _dal!.Task.Read(id);
            if (doTask == null || doTask.IsMilestone != true)
                throw new BO.BlDoesNotExistException($"Milestone with ID={id} does Not exist");

            return Tools.fromDoTaskToMilestone(doTask);
        }
        catch (Exception ex)
        {
            throw new BlDoesNotExistException("", ex);
        }
    }

    /// <summary>
    /// Updates the specified milestone with new information.
    /// </summary>
    /// <param name="id">The ID of the milestone to be updated.</param>
    /// <param name="alias">The new alias for the milestone.</param>
    /// <param name="description">The new description for the milestone.</param>
    /// <param name="remarks">The new remarks for the milestone.</param>
    /// <returns>The updated business object representation of the milestone.</returns>
    /// <exception cref="BO.InvalidInputException">Thrown if any of the input parameters are null or empty.</exception>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the milestone with the specified ID does not exist.</exception>
    public BO.Milestone Update(int id, string alias, string description, string? remarks)
    {
        if (alias == null || alias == "" || description == null || description == "" || remarks == "")
        { throw new BO.InvalidInputException("InvalidInput"); }

        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
            throw new BO.BlDoesNotExistException($"DoesNotExist milestone whith{id}");
        DO.Task? newTask = task with { Alias = alias, Description = description, Remarks = remarks };

        _dal.Task.Update(newTask);

        return new BO.Milestone();
    }

}
