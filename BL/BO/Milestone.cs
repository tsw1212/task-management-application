
using DO;

namespace BO;

public class Milestone
{
    public int Id { get; set; }
    public string Description {  get; set; }
    public string Alias {  get; set; }
    public DateTime CreatedAtDate { get; set; }
    public BO.Status? Status {  get; set; }
    public DateTime? ForeCastDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public DateTime? DeadLineDate { get; set; }
    public double? CompletionPercentage {  get; set; }
    public string? Remarks {  get; set; }
    public  List<BO.TaskInList>? Dependencies { get; set; }
    public override string ToString() => this.ToStringProperty();


}
