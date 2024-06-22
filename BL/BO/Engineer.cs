using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Engineer
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string Email { get; set; }
    public EngineerExperience‏ Level { get; set; }
    public double Cost { get; set; }
    public BO.TaskinEngineer? Task {  get; set; }
    public override string ToString() => this.ToStringProperty();
}
