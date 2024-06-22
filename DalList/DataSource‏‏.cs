

namespace Dal;
internal static class DataSource‏‏
{
    //Creating a running variable that produces serial numbers in order starting from the number 1000
    internal static class Config
    {
        //A running variable for the task ID number
        internal const int startTaskId = 1000;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }


        //A running variable for the dependency identification number between the tasks
        internal const int startDependencyId = 100;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        internal static DateTime? startProject = null;

        internal static DateTime? endProject = null;
    }

    //Creating three linked lists, one for each entity
    internal static List<DO.Task?> Tasks { get; } = new();
    internal static List<DO.Engineer?> Engineers { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();



}
