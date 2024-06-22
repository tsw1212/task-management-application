using BO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DalApi;


namespace BlImplementation
{
    public class MilestoneInListImplementation : BlApi.IMilestoneInList
    {
        private DalApi.IDal _dal = DalApi.Factory.Get;


        public IEnumerable<MilestoneInList> ReadAll(Func<MilestoneInList, bool>? filter = null)
        {
            var milestoneInLists = (from DO.Task doTask in _dal.Task.ReadAll()
                                select Tools.fromDoTaskToMilestoneInList(doTask));
            if (filter != null)
            {
                return milestoneInLists.Where(filter);
            }
            else
            {
                return milestoneInLists;
            }
        }
    public BO.MilestoneInList? Read(int id)
    {

        try
        {
            DO.Task? doTask = _dal!.Task.Read(id);
            if (doTask == null || doTask.IsMilestone != true)
                throw new BO.BlDoesNotExistException($"Milestone with ID={id} does Not exist");

            return Tools.fromDoTaskToMilestoneInList(doTask);
        }
        catch (Exception ex)
        {
            throw new BlDoesNotExistException("", ex);
        }
    }
    }

}
