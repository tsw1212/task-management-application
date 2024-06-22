namespace Dal;
using DalApi;
using DO;

using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// Receives an engineer type item if the ID card does not exist adds to the document and if so throws an error
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Task item)
    {
        List<Task> xmlTask = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task newTask = item with { Id = Config.NextTaskId };
        xmlTask.Add(newTask);
        XMLTools.SaveListToXMLSerializer<Task>(xmlTask, "tasks");
      
        return newTask.Id;
    }
    /// <summary>
    /// the method checks if an item with such an id exsist.
    /// if it does it will be deleted from the xml,
    /// other ways an Exception will be throwen
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        List<Task> xmlTask = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? newTask = Read(id);
        if (newTask == null)
        { throw new DalDoesNotExistException($"Task with ID={id} does Not exist"); }
        xmlTask.Remove(newTask);
        XMLTools.SaveListToXMLSerializer<Task>(xmlTask, "tasks");
    }

    /// <summary>
    /// the method checks if an item with the requierde id exsist and returns it
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the item that was found or null if no matching item was found</returns>
    public Task? Read(int id)
    {
        List<Task> xmlTask = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = (from item in xmlTask
                      where item.Id == id
                      select item).FirstOrDefault();
        return task != null ? task : null;
    }
    /// <summary>
    /// a method that gets a condition and returns the first item who matches the condition 
    /// if ther is no matching item return null
    /// </summary>
    /// <param name="filter">a function with a condtion</param>
    /// <returns> the first item who matches the condition</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> xmlTask = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = (from item in xmlTask
                      where filter(item)
                      select item).FirstOrDefault();
        return task != null ? task : null;
    }
    /// <summary>
    /// the method returns the list of items We met the condition or all the xml if the filter = null
    /// </summary>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> xmlTask = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (filter != null)
        {
            return from item in xmlTask
                   where filter(item)
                   select item;
        }
        return from item in xmlTask
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
        List<Task> xmlTask = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? newTask = Read(item.Id);
        if (newTask == null)
        { throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist"); }
        Delete(item.Id);
        xmlTask = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        xmlTask.Add(item);
        XMLTools.SaveListToXMLSerializer<Task>(xmlTask, "tasks");
    }
    public void Reset()
    {
        XMLTools.ResetFile("tasks","Task");
    }

}
