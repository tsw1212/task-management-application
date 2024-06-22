

namespace Dal;
using DalApi;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    public DateTime ?StartProject { get=> DataSource.Config.startProject; set=> DataSource.Config.startProject=value; } 
    public DateTime ? EndProject { get =>  DataSource.Config.endProject; set => DataSource.Config.endProject = value; }
}
