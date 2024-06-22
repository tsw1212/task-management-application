

namespace BlImplementation;
using BO;

using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Implementation of the <see cref="IEngineer"/> interface providing operations for engineers.
/// </summary>
public class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Creates a new engineer based on the provided business object.
    /// </summary>
    /// <param name="boEngineer">The business object representing the engineer to be created.</param>
    /// <returns>The ID of the newly created engineer.</returns>
    /// <exception cref="BO.BlNullPropertyException">Thrown if any required property of the engineer is null.</exception>
    /// <exception cref="BO.InvalidInputException">Thrown if any input parameter is invalid.</exception>
    /// <exception cref="BO.BlAlreadyExistsException">Thrown if an engineer with the same ID already exists.</exception>
    public int Create(BO.Engineer boEngineer)
    {
        if (boEngineer.Name == null || boEngineer.Email == null || boEngineer?.Cost == null)
            throw new BO.BlNullPropertyException("you can not send a null property");
        string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(emailPattern);
        Match match = regex.Match(boEngineer.Email);
        if (boEngineer.Id < 0 || boEngineer.Cost < 0 || boEngineer.Name == ""|| !match.Success)
        {
            throw new BO.InvalidInputException("Invalid input");
        }
        DO.Engineer doEngineer = Tools.EngineerfromBoToDo(boEngineer);
        try
        {
            int ideng = _dal.Engineer.Create(doEngineer);
            return ideng;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Deletes the engineer with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to be deleted.</param>
    /// <exception cref="BO.BlDeletionImpossible">Thrown if the engineer has associated tasks.</exception>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the engineer with the specified ID does not exist.</exception>
    public void Delete(int id)
    {
        if (Tools.TaskInEngineer(id) != null)
        {
            throw new BO.BlDeletionImpossible($" Can't delete engineer with ID={id} ");
        }
        else
        {
            try
            { 
            _dal.Engineer.Delete(id);
        }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Student with ID={id} already exists", ex);
            }
        }
    }
    /// <summary>
    /// Retrieves the engineer with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to be retrieved.</param>
    /// <returns>The business object representation of the retrieved engineer.</returns>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the engineer with the specified ID does not exist.</exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engneer with ID={id} does Not exist");
        return Tools.EngineerFromDoToBo(doEngineer);
    }

    /// <summary>
    /// Retrieves all engineers, optionally filtered by the provided filter function.
    /// </summary>
    /// <param name="filter">An optional filter function to apply on the engineers.</param>
    /// <returns>The collection of business object representations of engineers.</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter=null)
    {

        var listEngineer = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                            select Tools.EngineerFromDoToBo(doEngineer));
        if (filter != null)
        {
            return listEngineer.Where(filter);
        }
        else
        {
            return listEngineer;
        }
     
    }
    /// <summary>
    /// Updates the engineer with new information.
    /// </summary>
    /// <param name="boEngineer">The business object representation of the engineer to be updated.</param>
    /// <exception cref="BO.BlNullPropertyException">Thrown if any required property of the engineer is null.</exception>
    /// <exception cref="BO.InvalidInputException">Thrown if any input parameter is invalid.</exception>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the engineer with the specified ID does not exist.</exception>
    public void Update(BO.Engineer boEngineer)
    {
        if(boEngineer.Name == null || boEngineer.Email == null|| boEngineer?.Cost ==null)
            throw new BO.BlNullPropertyException("you can not send a null property");
        string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        Regex regex = new Regex(emailPattern);
        Match match = regex.Match(boEngineer.Email); if (boEngineer.Id < 0 || boEngineer.Cost < 0 || boEngineer.Name == "" || !match.Success)
        {
            throw new BO.InvalidInputException("Invalid input");
        }
        DO.Engineer doEngineer =Tools.EngineerfromBoToDo(boEngineer);

        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={boEngineer.Id} already exists", ex);
        }
    }           
}