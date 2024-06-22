


namespace Dal;
using DalApi;
using System.Diagnostics;
using System.Xml.Linq;


sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementationcs();

    public ITask Task => new TaskImplementation();
    public DateTime?  StartProject { get => Config.cStartProject; set => Config.cStartProject = value; }
    public DateTime? EndProject { get => Config.cEndProject; set => Config.cEndProject = value; }
}
