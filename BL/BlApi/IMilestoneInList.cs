using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IMilestoneInList
{
    public IEnumerable<BO.MilestoneInList> ReadAll(Func<BO.MilestoneInList, bool>? filter = null);
    public BO.MilestoneInList? Read(int id);

}
