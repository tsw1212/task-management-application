using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;

public class createMileseton
{
    public static bool iscreated { get; set; } =false;
}
public class AddOrUpdateEvent:EventArgs
{
    public string EventState { get; set; }
    public AddOrUpdateEvent (string eventState) {  EventState = eventState; } 
}


