

namespace DalApi;

public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    //IDate ProjectDate { get; }
    
    DateTime? StartProject { get;  set; }
    DateTime? EndProject { get; set; }    
}
