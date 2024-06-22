using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class MilestoneInList
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Ailas { get; set; }
    public BO.Status? Status { get; set; }
    public double? CompletionPercentage { get; set; }
    public override string ToString() => this.ToStringProperty();


}
