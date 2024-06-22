namespace Dal;

using DalApi;
using DO;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using System.Xml.Linq;

// Internal class implementing the IDependency interface
internal class DependencyImplementation : IDependency
{
    // Method to create a new Dependency
    public int Create(Dependency item)
    {

        // Create a new Dependency with a unique ID
        Dependency newDependency = item with { Id = Config.NextDependencyId };

        // Add the new Dependency to the XML representation
        AddDependency(newDependency);


        // Return the ID of the created Dependency
        return newDependency.Id;
    }

    // Method to delete a Dependency by ID
    public void Delete(int id)
    {
        // Load dependencies from XML file
        XElement xmlDependency = XMLTools.LoadListFromXMLElement("dependencies");

        // Find the Dependency with the specified ID or throw an exception
        XElement? XmlElement = xmlDependency.Descendants("dependency")
            .FirstOrDefault(elmn => Convert.ToInt32(elmn.Attribute("Id")!.Value).Equals(id))
            ?? throw new DalDoesNotExistException(($"Dependency with ID={id} does Not exist"));

        // Remove the found Dependency from the XML representation
        XmlElement.Remove();

        // Save the updated XML representation
        XMLTools.SaveListToXMLElement(xmlDependency, "dependencies");
    }

    // Method to read a Dependency by ID
    public Dependency? Read(int id)
    {
        // Load dependencies from XML file
        XElement xmlDependency = XMLTools.LoadListFromXMLElement("dependencies");

        // Find the Dependency with the specified ID
        XElement? XmlElement = xmlDependency.Descendants("")
            .FirstOrDefault(elmn => Convert.ToInt32(elmn.Attribute("Id")!.Value).Equals(id));
        // If the Dependency is not found, return null; otherwise, create and return a new Dependency
        if (XmlElement == null)
            return null;
        return new Dependency(
            Convert.ToInt32(XmlElement.Attribute("Id")?.Value),
            Convert.ToInt32(XmlElement.Attribute("DependentTask")?.Value),
            Convert.ToInt32(XmlElement.Attribute("DependsTask")?.Value)
        );
    }

    // Method to read a Dependency based on a filter
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        // Load dependencies from XML file
        XElement xmlDependency = XMLTools.LoadListFromXMLElement("dependencies");

        // Use LINQ to find the first Dependency that satisfies the filter
        Dependency? dependency = (from item in xmlDependency.Descendants("dependency")
                                  where filter(new Dependency(
                                      Convert.ToInt32(item.Attribute("Id")?.Value),
                                      Convert.ToInt32(item.Attribute("DependentTask")?.Value),
                                      Convert.ToInt32(item.Attribute("DependsTask")?.Value)))
                                  select new Dependency(
                                      Convert.ToInt32(item.Attribute("Id")?.Value),
                                      Convert.ToInt32(item.Attribute("DependentTask")?.Value),
                                      Convert.ToInt32(item.Attribute("DependsTask")?.Value))).FirstOrDefault();

        // Return the found Dependency or null
        return dependency != null ? dependency : null;
    }

    // Method to read all Dependencies, optionally filtered
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        // Load dependencies from XML file
        XElement xmlDependency = XMLTools.LoadListFromXMLElement("dependencies");

        // Use LINQ to select Dependencies based on the filter (if provided)
        if (filter != null)
        {
            return (from item in xmlDependency.Descendants("dependency")
                    where filter(new Dependency(
                        Convert.ToInt32(item.Attribute("Id")?.Value),
                        Convert.ToInt32(item.Attribute("DependentTask")?.Value),
                        Convert.ToInt32(item.Attribute("DependsTask")?.Value)))
                    select new Dependency(
                        Convert.ToInt32(item.Attribute("Id")?.Value),
                        Convert.ToInt32(item.Attribute("DependentTask")?.Value),
                        Convert.ToInt32(item.Attribute("DependsTask")?.Value)));
        }
        else
        {
            // If no filter is provided, select all Dependencies
            var t = from item in xmlDependency.Descendants("dependency")
                    select new Dependency(
                        Convert.ToInt32(item.Attribute("Id")?.Value),
                        Convert.ToInt32(item.Attribute("DependentTask")?.Value),
                        Convert.ToInt32(item.Attribute("DependsTask")?.Value));
            return t;
        }
    }

    // Method to update a Dependency
    public void Update(Dependency item)
    {
        // Load dependencies from XML file
        XElement xmlDependency = XMLTools.LoadListFromXMLElement("dependencies");

        // Find the Dependency with the specified ID or throw an exception
        XElement? XmlElement = xmlDependency.Descendants("dependency")
            .FirstOrDefault(elmn => Convert.ToInt32(elmn.Attribute("Id")!.Value).Equals(item.Id))
            ?? throw new DalDoesNotExistException(($"Dependency with ID={item.Id} does Not exist"));

        // Update the DependentTask and DependsTask attributes of the found Dependency
        XmlElement.Attribute("DependentTask")!.Value = item.DependentTask.ToString();
        XmlElement.Attribute("DependsTask")!.Value = item.DependsTask.ToString();

        // Save the updated XML representation
        XMLTools.SaveListToXMLElement(xmlDependency, "dependencies");
    }
    public void AddDependency(Dependency item)
    {
        XElement xmlDependency = XMLTools.LoadListFromXMLElement("dependencies");

        xmlDependency.Add(new XElement("dependency",
           new XAttribute("Id", item.Id),
           new XAttribute("DependentTask", item.DependentTask),
           new XAttribute("DependsTask", item.DependsTask)));
        XMLTools.SaveListToXMLElement(xmlDependency, "dependencies");

    }
    public void Reset()
{
    XMLTools.ResetFile("dependencies", "dependency");
}
}
