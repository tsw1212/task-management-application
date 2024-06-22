using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using System.Xml.Linq;

internal class EngineerImplementationcs : IEngineer
{/// <summary>
/// Receives an engineer type item if the ID card does not exist adds to the document and if so throws an error
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
/// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer item)
    {
        List<Engineer> xmlEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? newEngineer = Read(item.Id);
        if (newEngineer != null)
        { throw new DalAlreadyExistsException($"Engineer with ID={newEngineer.Id} does exist"); }
        xmlEngineer.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(xmlEngineer, "engineers");
        return item.Id;
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
        List<Engineer> xmlEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? newEngineer = Read(id);
        if (newEngineer == null)
        { throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist"); }
        xmlEngineer.Remove(newEngineer);
        XMLTools.SaveListToXMLSerializer<Engineer>(xmlEngineer, "engineers");
    }
    /// <summary>
    /// a method that gets a id and return first item who matches the id
    /// if ther is no matching item return null
    /// </summary>
    /// <param name="filter">a function with a condtion</param>
    /// <returns> the first item who matches the condition</returns>

    public Engineer? Read(int id)
    {
        List<Engineer> xmlEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineer = xmlEngineer.FirstOrDefault(Engineer=> Engineer.Id== id);
        return engineer != null ? engineer : null;
    }

    /// <summary>
    /// a method that gets a condition and returns the first item who matches the condition 
    /// if ther is no matching item return null
    /// </summary>
    /// <param name="filter">a function with a condtion</param>
    /// <returns> the first item who matches the condition</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> xmlEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineer = xmlEngineer.FirstOrDefault(Engineer=> filter(Engineer));
        return engineer != null ? engineer : null;
    }
    /// <summary>
    /// the method returns the list of items We met the condition or all the list if the filter = null
    /// </summary>

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> xmlEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");

        if (filter != null)
        {
            return from item in xmlEngineer
                   where filter(item)
                   select item;
        }
        return from item in xmlEngineer
               select item;
    }
    /// <summary>
    /// the method checks if an item with such an id exsist in the xml
    /// if yes, it deletes it(by calling delete method),
    /// now the method will create a new item with the updated detailes
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Engineer item)
    {
        List<Engineer> xmlEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? newEngineer = Read(item.Id);
        if (newEngineer == null)
        { throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist"); }
        Delete(item.Id);
        xmlEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        xmlEngineer.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(xmlEngineer, "engineers");

    }
    public void Reset()
    {
        XMLTools.ResetFile("engineers", "Engineer");
    }

}
