

namespace BO;

public class TaskinEngineer
{
  public  int Id {  get; set; }
    public string Alias { get; set; }
    public TaskinEngineer(int id, string alias)
    {
        Id = id;
        Alias = alias;
    }
    public override string ToString() => this.ToStringProperty();

    public TaskinEngineer() { }
}
