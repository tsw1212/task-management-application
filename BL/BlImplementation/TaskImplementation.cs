namespace BlImplementation;
using System;
using System.Collections.Generic;
using BlApi;
using BO;
using DalApi;


/// <summary>
/// Implementation of the <see cref="ITask"/> interface providing CRUD operations for tasks.
/// </summary>
public class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new task based on the provided business object.
    /// </summary>
    /// <param name="boTask">The business object representing the task to be created.</param>
    /// <returns>The ID of the newly created task.</returns>
    public int Create(BO.Task boTask)
    {
        if (boTask.Alias == null)
            throw new BO.BlNullPropertyException("you can not send a null property");
        if (boTask.Id < 0 || boTask.Alias == "")
            throw new BO.InvalidInputException("Invalid input");
        try
        {
            DO.Task doTask = Tools.TaskfromBoToDo(boTask);
            int taskId = _dal.Task.Create(doTask);
            return taskId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Deletes a task with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the task to be deleted.</param>
    public void Delete(int id)
    {
        if (Tools.DependsTask(id))
            throw new BO.BlDeletionImpossible($"Their is atask(or more) who depent on the task with the ID={id}");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} already exists", ex);
        }

    }

    /// <summary>
    /// Retrieves a task with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the task to be retrieved.</param>
    /// <returns>The business object representation of the retrieved task.</returns>
    public Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null || doTask.IsMilestone)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return Tools.TaskFromDoToBo(doTask);


    }

    /// <summary>
    /// Retrieves all tasks, optionally filtered by a specified condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply to the tasks.</param>
    /// <returns>The collection of business object representations of tasks.</returns>

    public IEnumerable<Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {

        var listTask = _dal.Task.ReadAll()
                 .Where(doTask => !doTask?.IsMilestone ?? false)
                .Select(doTask => Tools.TaskFromDoToBo(doTask));
        return filter == null ? listTask : listTask.Where(filter);
    }

    /// <summary>
    /// Updates an existing task with the provided business object.
    /// </summary>
    /// <param name="boTask">The business object representing the task to be updated.</param>
    public void Update(Task boTask)
    {
        if (boTask?.Engineer?.Id != null)
        {
            if (_dal.Engineer.Read(boTask?.Engineer?.Id ?? 0) == null)
                throw new BO.InvalidInputException("Invalid input");
        }
        if (boTask.Id < 0)
            throw new BO.InvalidInputException("Invalid input");
        try
        {
            if (boTask != null)
            {
                DO.Task doTask = Tools.TaskfromBoToDo(boTask);
                _dal.Task.Update(doTask);
            }

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask?.Id} already exists", ex);
        }
    }
}
