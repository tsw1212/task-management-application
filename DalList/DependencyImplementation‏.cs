namespace Dal;

using DalApi;
using DO;


//The implementation of the interface of the dependency entity between the tasks
internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// the method gets the ditails of creates a new dependency by using the cunstroctor
    /// </summary>
    /// <param name="item"></param>
    /// <returns>the new dependencys id</returns>
    public int Create(Dependency item)
    {
        Dependency newDependency = item with { Id = DataSource.Config.NextDependencyId };
        DataSource.Dependencies.Add(newDependency);
        return newDependency.Id;

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
        Dependency? newDependency = Read(id);
        if (newDependency == null)
        { throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist"); }
        DataSource.Dependencies.Remove(newDependency);

    }


    /// <summary>
    /// the method checks if an item with the requierde id exsist and returns it
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the item that was found or null if no matching item was found</returns>
    public Dependency? Read(int id)
    {
        Dependency? dependency = (from item in DataSource.Dependencies
                                 where item.Id == id
                                  select item).FirstOrDefault();
        return dependency != null ? dependency : null;
    }

    /// <summary>
    /// the method returns the list of items
    /// </summary>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;
    }


    /// <summary>
    /// the method checks if an item with such an id exsist in the list
    /// if yes, it deletes it(by calling delete method),
    /// now the method will create a new item wuth the updated detailes
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Dependency item)
    {
        Dependency? newDependency = Read(item.Id);
        if (newDependency == null)
        { throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist"); }
        Delete(item.Id);
        DataSource.Dependencies?.Add(item);

    }
    /// <summary>
    /// a method that gets a condition and returns the first item who matches the condition 
    /// if ther is no matching item
    /// </summary>
    /// <param name="filter">a function with a condtion</param>
    /// <returns> the first item who matches the condition</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        Dependency? dependency = (from item in DataSource.Dependencies
                                  where filter(item)
                                  select item).FirstOrDefault();
        return dependency != null ? dependency : null;
    }
    public void AddDependency(Dependency dependency)
    {
        DataSource.Dependencies?.Add(dependency);
    }
    public void Reset()
    {
        DataSource.Dependencies?.Clear();
    }
}
   
