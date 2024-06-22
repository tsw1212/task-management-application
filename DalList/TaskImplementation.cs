namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static Dal.DataSource;

//The interface implementation of the tasks entity
internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        Task newtTask = item with { Id = DataSource.Config.NextTaskId };
        DataSource.Tasks.Add(newtTask);
        return newtTask.Id;
    }
    /// <summary>
    /// the method checks if an item with such an id exsist.
    /// if it does it will be deleted from the list,
    /// other ways an Exception will be throwen
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Task? newTask = Read(id);
        if (newTask == null)
        { throw new DalDoesNotExistException($"Task with ID={id} does Not exist"); }
        DataSource.Tasks.Remove(newTask);

    }

    /// <summary>
    /// the method checks if an item with the requierde id exsist and returns it
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the item that was found or null if no matching item was found</returns>
    public Task? Read(int id)
    {
        Task? task = (from item in DataSource.Tasks
                              where item.Id == id
                              select item).FirstOrDefault();
        return task != null ? task : null;

    }

    /// <summary>
    /// the method returns the list of items We met the condition or all the xml if the filter = null
    /// </summary>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    /// <summary>
    /// the method checks if an item with such an id exsist in the xml
    /// if yes, it deletes it(by calling delete method),
    /// now the method will create a new item with the updated detailes
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Task item)
    {
        Task? newTask = Read(item.Id);
        if (newTask == null)
        { throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist"); }
        Delete(item.Id);
        DataSource.Tasks.Add(item);

    }
    /// <summary>
    /// a method that gets a condition and returns the first item who matches the condition 
    /// if ther is no matching item return null
    /// </summary>
    /// <param name="filter">a function with a condtion</param>
    /// <returns> the first item who matches the condition</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        Task? task = (from item in DataSource.Tasks
                                  where filter(item)
                                  select item).FirstOrDefault();
        return task != null ? task : null;
    }

    public void Reset()
    {
        DataSource.Tasks?.Clear();
    }
}
