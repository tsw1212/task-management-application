using System.Collections;
using System.Reflection;
using System.Text;
namespace BO;
/// <summary>
/// Utility class containing various static methods for handling tasks and dependencies.
/// </summary>
public static class Tools
{
    private static DalApi.IDal _dal = DalApi.Factory.Get;
    static BlApi.IBl s_bl = BlApi.Factory.Get();
    // <summary>
    /// Retrieves a list of dependent tasks for a given task ID.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <returns>A list of dependent tasks represented as <see cref="TaskInList"/>.</returns>
    public static List<BO.TaskInList> depndentTesks(int id)
    {
        var listId = _dal!.Dependency.ReadAll((d) => d.DependentTask == id);
        var dependencies = (from d in listId.ToList()
                            let task = _dal!.Task.Read(d.DependsTask)
                            select new TaskInList
                            {
                                Id = task.Id,
                                Ailas = task.Alias,
                                Description = task.Description,
                                Status = myStatus(task)
                            }).ToList();
        return dependencies;
    }

    /// <summary>
    /// Calculates and returns the status of a task based on its various date properties.
    /// </summary>
    /// <param name="task">The task for which to determine the status.</param>
    /// <returns>The status of the task as <see cref="BO.Status"/>.</returns>
    public static Status myStatus(DO.Task task)
    {
        return (BO.Status)(task.ScheduledDate == null ? 0
                                                     : task.DeadLineDate?.AddDays(-5) == DateTime.Now ? 2
                                                     : task.StartDate == null ? 1
                                                    : task.CompleteDate == null ? 3
                                                    : 4);
    }
    internal static BO.TaskInList TaskInListFromTask(DO.Task doTask)
    {
        return new TaskInList { Id = doTask.Id, Ailas = doTask.Alias, Description = doTask.Description, Status = Tools.myStatus(doTask) };
    }

    /// <summary>
    /// Creates a milestone based on the provided dependencies, start date, and end date.
    /// </summary>
    /// <param name="dependencies">List of dependencies.</param>
    /// <param name="startPro">Start date of the project.</param>
    /// <param name="endPro">End date of the project.</param>
    /// <returns>List of dependencies representing the created milestone.</returns>
    internal static List<DO.Dependency> CreateMileStone(List<DO.Dependency?> dependencies, DateTime? startPro, DateTime? endPro)
    {
        if (dependencies == null)
            dependencies = new List<DO.Dependency?>();
        List<DO.Dependency> newDependencies = new List<DO.Dependency>();
        int count = 0;

        DO.Task firstMilestone = new(
         0,
         $"M{count++}",
         $"MStart",
         DateTime.Now,
         new TimeSpan(0),
         null,
         true,//ismilestone
         startPro
         );
        int idfirstMilestone = _dal.Task.Create(firstMilestone);

        var list = dependencies.GroupBy(d => d?.DependentTask).ToList();
        var sortedList = list.OrderBy(comparer => comparer.Key);
        foreach (DO.Task? task in _dal.Task.ReadAll())
        {
            if (task != null)
            {
                var isTaskIdInList = sortedList.Any(group => group.Any(item => item?.DependentTask == task.Id));
                if (!isTaskIdInList && task?.Id != idfirstMilestone)
                {
                    newDependencies.Add(new DO.Dependency()
                    {
                        DependentTask = task?.Id ?? 0,//אף פעם לא יגיע לאופציה השניה כי יש תנאי שבודק אם המשימה null 
                        DependsTask = idfirstMilestone
                    });
                }
            }
        }
        foreach (var group in list)
        {
            bool flagExsistMileStone = false;
            int id = 0;
            var allMilestones = _dal.Task.ReadAll(task => task.IsMilestone).ToList();

            foreach (var m in allMilestones)
            {
                List<DO.Dependency>? allDependencies = newDependencies.Where(d => d.DependentTask == m!.Id)?.ToList();
                flagExsistMileStone = AreGroupsEqual(allDependencies, group.ToList());
                if (flagExsistMileStone)
                {
                    id = m!.Id;
                    break;

                }
            }
            if (!flagExsistMileStone)
            {

                DO.Task milestone = new DO.Task(
                  0,
                $"M{count++}",
                $"M{count}",
                DateTime.Now,
               TimeSpan.Zero,
               null,
                true);
                id = _dal.Task.Create(milestone);

                foreach (var depend in group)
                {
                    if (depend?.DependsTask != null)
                    {
                        newDependencies.Add(new DO.Dependency()
                        {
                            DependentTask = id,
                            DependsTask = depend?.DependsTask ?? 0//אף פעם לא יגיע לאופציה השניה כי יש תנאי שבודק לפני שהוא לא null
                        });
                    }
                }
            }
            if (group.Key != null)
            {
                newDependencies.Add(new DO.Dependency()
                {
                    DependentTask = group.Key ?? 0,//אף פעם לא יגיע לאופציה השניה כי יש תנאי שבודק לפני שהוא לא null
                    DependsTask = id
                });
            }

        }
        DO.Task endMilestone = new(
                                 0,
                                  $"M{count++}",
                                   $"MEnd",
                                    DateTime.Now,
                                     new TimeSpan(0),
                                      null,
                                      true,
                                         endPro,
                                         endPro);
        int idendMilestone = _dal.Task.Create(endMilestone);

        var DependonsonEnd = (from DO.Task t in _dal.Task.ReadAll()
                              where ((_dal.Dependency.Read((de) => de.DependsTask == t.Id) == null ? true : false) && !t.IsMilestone)
                              select t.Id).ToList();
        foreach (var tId in DependonsonEnd)
        {
            newDependencies.Add(new DO.Dependency()
            {
                DependentTask = idendMilestone,
                DependsTask = tId,
            });
        }
        return newDependencies;
    }

    /// <summary>
    /// Calculates the completion percentage of a list of tasks.
    /// </summary>
    /// <param name="listTask">The list of tasks for which to calculate the completion percentage.</param>
    /// <returns>The completion percentage as a double.</returns>
    internal static double completionPercentage(List<TaskInList> listTask)
    {
        double sum = 0;
        for (int i = 0; i < listTask.Count; i++)
        {
            sum += ((int?)listTask[i].Status) == null ? 0 : (int)listTask[i]!.Status! * 100 / 4;
        }
        return sum / listTask.Count;

    }
    /// <summary>
    /// Converts a business object representation of a task to a data object representation.
    /// </summary>
    /// <param name="boTask">The business object task to convert.</param>
    /// <returns>The data object representation of the task.</returns>
    internal static DO.Task TaskfromBoToDo(BO.Task boTask)
    {

        return new DO.Task
        (
           boTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            boTask.RequierdEffortTime,
            boTask.Engineer?.Id ?? null,
            false,
            boTask.StartDate,
            boTask.ScheduledStartDate,
            boTask.DeadLineDate,
            boTask.CompleteDate,
            boTask.Deliverables,
            boTask.Remarks,
           boTask.ComplexityLevel == null ? null : (DO.EngineerExperience)boTask.ComplexityLevel
        );
    }
internal static BO.MilestoneInList fromDoTaskToMilestoneInList(DO.Task task)
    {
        return new BO.MilestoneInList
        {
            Id = task.Id,
            Description = task.Description,
            Ailas = task.Alias,
            Status = Tools.myStatus(task),
            CompletionPercentage = Tools.completionPercentage(Tools.depndentTesks(task.Id))
        };
    }

    /// <summary>
    /// Converts a data object representation of a task to a business object representation.
    /// </summary>
    /// <param name="doTask">The data object task to convert.</param>
    /// <returns>The business object representation of the task.</returns>
    internal static BO.Task TaskFromDoToBo(DO.Task doTask)
    {
        return new Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = Tools.myStatus(doTask),
            Milestone = milestoneInTask(doTask.Id),
            StartDate = doTask.StartDate,
            ScheduledStartDate = doTask.ScheduledDate,
            ForeCastDate = doTask.StartDate?.Add(doTask?.RequierdEffortTime ?? new TimeSpan(0)),
            CompleteDate = doTask!.CompleteDate,
            DeadLineDate = doTask.DeadLineDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = EngineerInTask(doTask.EngineerId),
            ComplexityLevel = doTask.ComplexityLevel == null ? null : (BO.EngineerExperience)doTask.ComplexityLevel
        };
    }
    /// <summary>
    /// Retrieves information about an engineer associated with a task.
    /// </summary>
    /// <param name="id">The ID of the engineer.</param>
    /// <returns>The <see cref="EngineerInTask"/> information if available; otherwise, null.</returns>
    internal static BO.EngineerInTask? EngineerInTask(int? id)
    {
        if (id == null)
            return null;
        var engineerInTask = _dal.Engineer.ReadAll()
                      .Where(e => e!.Id == id)
                      .Select(en => new EngineerInTask { Id = id ?? 0, Name = en?.Name ?? "" }).First();
        return engineerInTask;
    }

    /// <summary>
    /// Converts a business object representation of an engineer to a data object representation.
    /// </summary>
    /// <param name="boEngineer">The business object engineer to convert.</param>
    /// <returns>The data object representation of the engineer.</returns>
    internal static DO.Engineer EngineerfromBoToDo(BO.Engineer boEngineer)
    {
        return new DO.Engineer(
        boEngineer.Id, boEngineer.Name, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level, boEngineer.Cost);
    }
    /// <summary>
    /// Converts a data object representation of an engineer to a business object representation.
    /// </summary>
    /// <param name="doEngineer">The data object engineer to convert.</param>
    /// <returns>The business object representation of the engineer.</returns>
    /// <summary>
    /// Converts a DO.Engineer object to a BO.Engineer object.
    /// </summary>
    /// <param name="doEngineer">The DO.Engineer object to convert.</param>
    /// <returns>The corresponding BO.Engineer object.</returns>
    internal static BO.Engineer EngineerFromDoToBo(DO.Engineer doEngineer)
    {
        return new BO.Engineer
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = TaskInEngineer(doEngineer.Id)
        };
    }

    /// <summary>
    /// Retrieves the tasks associated with an engineer.
    /// </summary>
    /// <param name="id">The ID of the engineer.</param>
    /// <returns>The tasks associated with the engineer.</returns>
    internal static TaskinEngineer? TaskInEngineer(int id)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()
                where doTask.EngineerId == id
                select new TaskinEngineer
                {
                    Id = doTask.Id,
                    Alias = doTask.Alias
                }).FirstOrDefault();
    }

    /// <summary>
    /// Checks if a task depends on another task.
    /// </summary>
    /// <param name="id">The ID of the task to check.</param>
    /// <returns>True if the task depends on another task; otherwise, false.</returns>
    internal static bool DependsTask(int id)
    {
        var flagDependsTask = _dal.Dependency.ReadAll()
            .FirstOrDefault(d => d!.DependentTask == id);

        return (flagDependsTask != null);
    }
    public static void addDependency(int idDependsTask, int idDependentTask)
    {
        DO.Dependency dependency=new DO.Dependency() { DependentTask=idDependentTask ,DependsTask= idDependsTask };
        _dal.Dependency.AddDependency(dependency);
    }
    internal static BO.MilestoneInTask? milestoneInTask(int id)
    {
        BO.MilestoneInTask? milestoneInTask = Tools.depndentTesks(id)
                                .Where(d => _dal.Task!.Read(d.Id)!.IsMilestone)
                                .Select(d => new BO.MilestoneInTask()
                                {
                                    Id = d.Id,
                                    Alias = d.Ailas
                                }).FirstOrDefault();

        return milestoneInTask;

    }
    /// <summary>
    /// Converts a business object representation of a milestone to a data object representation.
    /// </summary>
    /// <param name="doTask">The data object task representing the milestone.</param>
    /// <returns>The business object representation of the milestone.</returns>
    internal static BO.Milestone fromDoTaskToMilestone(DO.Task doTask)
    {
        return new BO.Milestone
        {
            Id = doTask.Id,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = Tools.myStatus(doTask),
            ForeCastDate = doTask.StartDate?.Add(doTask?.RequierdEffortTime ?? new TimeSpan(0)),
            CompleteDate = doTask!.CompleteDate,
            DeadLineDate = doTask.DeadLineDate ?? DateTime.Now,
            CompletionPercentage = Tools.completionPercentage(Tools.depndentTesks(doTask.Id)),
            Remarks = doTask.Remarks,
            Dependencies = Tools.depndentTesks(doTask.Id),
        };
    }
    /// <summary>
    /// Generates a string representation of an entity using its properties.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity for which to generate the string representation.</param>
    /// <returns>The string representation of the entity properties.</returns>
    internal static string ToStringProperty<T>(this T entity)
    {
        StringBuilder sb = new StringBuilder();

        // Get all properties of the entity using reflection
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.GetValue(entity) == null)
            {
                sb.AppendLine($"{property.Name}: null");
            }
            // Check if the property is a collection
            else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                // Handle collections
                IEnumerable collection = (IEnumerable)property.GetValue(entity);
                if (collection != null)
                {
                    sb.AppendLine($"{property.Name}:");
                    foreach (var item in collection)
                    {
                        sb.AppendLine($"- {item}");
                    }
                }
            }
            else
            {
                // Handle regular properties
                if (property.PropertyType == typeof(TimeSpan))
                {
                    // Special handling for TimeSpan
                    var timeSpanValue = (TimeSpan)property.GetValue(entity);
                    sb.AppendLine($"{property.Name}: {FormatTimeSpan(timeSpanValue)}");
                }
                else
                {
                    sb.AppendLine($"{property.Name}: {property.GetValue(entity)}");
                }
            }
        }

        return sb.ToString();
    }

    private static string FormatTimeSpan(TimeSpan timeSpan)
    {
        // Customize the format as needed
        return $"{timeSpan.Days} days, {timeSpan.Hours} hours, {timeSpan.Minutes} minutes, {timeSpan.Seconds} seconds";
    }
    /// <summary>
    /// Calculates and updates task deadlines based on project end date.
    /// </summary>
    /// <param name="endPro">The project end date.</param>
    /// <param name="startPro">The project start date.</param>
    /// <param name="tasks">The list of tasks.</param>
    internal static void CalculationTimes(List<DO.Dependency?> dependencies, DateTime startPro, DateTime stendPro)
    {

        DO.Task? firstMilestone = _dal.Task.Read((t) => t.IsMilestone && t.Description == "MStart");
        int idFirstMilestone = firstMilestone?.Id ?? throw new BlDoesNotExistException("start Milestone doesnt exsist");
        DO.Task? endMilestone = _dal.Task.Read((t) => t.IsMilestone && t.Description == "MEnd");
        int idEndMilestone = endMilestone?.Id ?? throw new BlDoesNotExistException("End milston doesnt exsist");
        UpdateDeadLineDateTime(idEndMilestone, idFirstMilestone, dependencies);
        UpdateScheduledDateTime(idFirstMilestone, idEndMilestone, dependencies);
    }
    /// <summary>
    /// Recursively updates the DeadLineDate property of dependent tasks based on the given task's information.
    /// </summary>
    /// <param name="idOfTask">The ID of the task.</param>
    /// <param name="idOfStartMilestone">The ID of the start milestone.</param>
    /// <param name="dependenciesList">The list of dependencies for the task.</param>
    private static void UpdateDeadLineDateTime(int? idOfTask, int idOfStartMilestone, List<DO.Dependency?> dependenciesList)
    {
        if (idOfTask == idOfStartMilestone)
            return;

        DO.Task? currentTask = _dal.Task.Read(idOfTask ?? throw new BlNullPropertyException("There is no value")) ?? throw new BlNullPropertyException($"Task with {idOfTask} ID does not exist");
        var dependenciesListId = _dal.Dependency.ReadAll((d) => d.DependentTask == idOfTask)
            .Select(d => d?.DependsTask).ToList();

        if (dependenciesListId != null)
        {
            foreach (var taskId in dependenciesListId)
            {
                DO.Task task = _dal.Task.Read(taskId ?? throw new BlNullPropertyException("There is no value")) ?? throw new BlNullPropertyException($"Task with {idOfTask} ID does not exist");
                DateTime? deadLineDate = currentTask?.DeadLineDate - currentTask?.RequierdEffortTime;
                if (!task.IsMilestone || (task.IsMilestone && (task.DeadLineDate == null || deadLineDate < task.DeadLineDate)))
                {
                    DO.Task updatedTask = task with { DeadLineDate = deadLineDate };
                    _dal.Task.Update(updatedTask);
                }

                UpdateDeadLineDateTime(taskId, idOfStartMilestone, dependenciesList);
            }
        }
    }

    /// <summary>
    /// Recursively updates the ScheduledDate property of dependent tasks based on the given task's information.
    /// </summary>
    /// <param name="idOfTask">The ID of the task.</param>
    /// <param name="idOfEndMilestone">The ID of the end milestone.</param>
    /// <param name="dependenciesList">The list of dependencies for the task.</param>
    private static void UpdateScheduledDateTime(int? idOfTask, int idOfEndMilestone, List<DO.Dependency?> dependenciesList)
    {
        if (idOfTask == idOfEndMilestone)
            return;

        DO.Task? currentTask = _dal.Task.Read(idOfTask ?? throw new BlNullPropertyException("There is no value")) ?? throw new BlNullPropertyException($"Task with {idOfTask} ID does not exist");
        var dependenciesListId = _dal.Dependency.ReadAll((d) => d?.DependsTask == idOfTask)
            .Select(d => d?.DependentTask).ToList();

        if (dependenciesListId != null)
        {
            foreach (var taskId in dependenciesListId)
            {
                DO.Task task = _dal.Task.Read(taskId ?? throw new BlNullPropertyException("There is no value")) ?? throw new BlNullPropertyException($"Task with {idOfTask} ID does not exist");
                DateTime? scheduledDate = currentTask?.ScheduledDate + currentTask?.RequierdEffortTime;


                if (currentTask.DeadLineDate + task.RequierdEffortTime > task.DeadLineDate)
                    throw new BlInvalidDataInTheSchedule("There is not enough time to finish the task");
                if (scheduledDate > task.DeadLineDate)
                    throw new BlInvalidDataInTheSchedule("There is not enough time to finish the task");


                if (!task.IsMilestone || (task.IsMilestone && (task.ScheduledDate == null || scheduledDate > task.ScheduledDate)))
                {
                    DO.Task updatedTask = task with { ScheduledDate = scheduledDate };
                    _dal.Task.Update(updatedTask);
                }


                UpdateScheduledDateTime(taskId, idOfEndMilestone, dependenciesList);
            }
        }
    }


    /// <summary>
    /// Checks whether two lists of dependencies are equal.
    /// </summary>
    /// <param name="list1">The first list of dependencies to compare.</param>
    /// <param name="list2">The second list of dependencies to compare.</param>
    /// <returns>True if the lists are equal, false otherwise.</returns>
    internal static bool AreGroupsEqual(List<DO.Dependency>? list1, List<DO.Dependency?> list2)
    {
        // Check if both lists are null
        if (list1 == null && list2 == null)
        {
            return true;
        }

        // Check if one list is null while the other is not
        if ((list1 == null && list2 != null) || (list2 == null && list1 != null))
        {
            return false;
        }

        // Check if both lists are the same instance or both are null
        if (ReferenceEquals(list1, list2))
        {
            return true;
        }

        // Check if any of the lists is null or their counts are different
        if (list1 == null || list2 == null || list1.Count != list2.Count)
        {
            return false;
        }

        // Sort both lists to ensure the order of elements is the same for comparison
        list1.Sort((d1, d2) => d1.Id.CompareTo(d2.Id));
        list2.Sort((d1, d2) => d1?.Id ?? 0.CompareTo(d2?.Id));//אם הוא ערך null אז הוא אף פעם לא שווה 

        // Compare each element in both lists
        for (int i = 0; i < list1.Count; i++)
        {
            // Return false if any elements are not equal
            if (list1[i].DependsTask != list2[i]?.DependsTask)
            {
                return false;
            }
        }

        // All elements are equal
        return true;
    }

    /// <summary>
    /// Initializes the start and end dates for project scheduling.
    /// </summary>
    /// <param name="start">The start date of the project.</param>
    /// <param name="end">The end date of the project.</param>
    public static void InitDateScheduleTime(DateTime startDateTime, DateTime endDateTime)
    {
        if (startDateTime < DateTime.Now)
            throw new InvalidInputException(" date must be later than the current date.");
        if (endDateTime < DateTime.Now)
            throw new InvalidInputException(" date must be later than the current date.");
        if (startDateTime > endDateTime)
            throw new InvalidInputException(" the endDateTime smaller then the startDateTime");
        _dal.StartProject = startDateTime;
        _dal.EndProject = endDateTime;
    }
    public static void significantNames()
    {
        var a = _dal.Task.ReadAll(t => t.IsMilestone).ToList();
        foreach (var t in a)
        {
            var depndents = depndentTesks(t?.Id ?? 0);
            string newAlias = "";
            foreach (var s in depndents)
            {
                newAlias += s.Ailas;
            }
            s_bl.Milestone.Update(t?.Id ?? 0, newAlias == "" ? t!.Alias : newAlias, t?.Description ?? "", t?.Remarks);

        }
    }
    public static MilestoneInList fromMilestoneToMilestoneInList(Milestone m)
    {
        MilestoneInList newMilestoneInList = new() { Id = m.Id , Description=m.Description,Ailas=m.Alias, Status=m.Status, CompletionPercentage=m.CompletionPercentage };
        return newMilestoneInList;
    }
}






