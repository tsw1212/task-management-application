

namespace BO;

public class TaskInList
{
   public int Id {  get; set; }
   public BO.Status? Status { get; set; }
   public string Description { get; set; }
    public string Ailas { get; set; }
    public override string ToString() => this.ToStringProperty();

}
