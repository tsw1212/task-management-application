using BlApi;
using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

public class TaskInListlementation:ITaskInList
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public BO.TaskInList read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        return Tools.TaskInListFromTask(doTask) ;
    }
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? filter = null)
    {

        var listTask = (from DO.Task doTask in _dal.Task.ReadAll()
                            select Tools.TaskInListFromTask(doTask));
        if (filter != null)
        {
            return listTask.Where(filter);
        }
        else
        {
            return listTask;
        }

    }
}
