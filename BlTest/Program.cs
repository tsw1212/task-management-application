using BlApi;
using BO;
using DalApi;
using DalTest;
using DO;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlTest;

internal class Program
{
    private static readonly Random s_rand = new();

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
   
   static void Main(string[] args)
    {
        try
        {
         
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
            {
                Initialization.Do();
            }
        

            DisplayMainMenu();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }



        static void DisplayMainMenu()//Main main program for choosing which entity to handle
        {

            bool exitMenu = false;

            while (!exitMenu)//As long as an option is not selected
            {
                Console.WriteLine("Main Menu - Select an entity you want to check:");
                Console.WriteLine("0. Exiting the main menu");
                Console.WriteLine("1. Task");
                Console.WriteLine("2. Engineer");
                Console.WriteLine("3. Milestone");

                Console.Write("Enter your choice: ");
                string? input = Console.ReadLine();

                switch (input)//A switch case program for choosing which entity to handle 1 to task 2 to engineer 3 to Dependency
                {
                    case "0":
                        exitMenu = true;
                        break;
                    case "1":
                        // Handle Task
                        DisplaySubMenu("Task");//calls a general function that refers to all the delete/add options and the like secondary main

                        break;
                    case "2":
                        // Handle Engineer
                        DisplaySubMenu("Engineer");//calls a general function that refers to all the delete/add options and the like secondary main

                        break;
                    case "3":
                        // Handle Dependency
                        DisplaySubMenu("Milestone");//calls a general function that refers to all the delete/add options and the like secondary main

                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }

        }

    }

    public static void DisplaySubMenu(string entityName)//A function that receives an entity-name string and executes for each delete fish option a subfunction that handles each delete fish option
    {
        try
        {
            bool exitMenu = false;

            while (!exitMenu)
            {
                Console.WriteLine("Select the method you want to execute:");
                Console.WriteLine("1. Exiting the main menu");

                if (entityName != "Milestone") { Console.WriteLine("2. Adding a new object to the " + entityName + " list"); }
                else { Console.WriteLine("2. Creating a project schedule"); }
                Console.WriteLine("3. Object display by identifier");
                Console.WriteLine("4. Updating existing object data");
                if (entityName != "Milestone")
                {
                    Console.WriteLine("5. The display of the list of all " + entityName);
                    Console.WriteLine("6. Deleting an existing object from a list");
                }

                Console.Write("Enter your choice: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        exitMenu = true;
                        break;
                    case "2":
                        // Handle Create
                        createGlobal(entityName);//A creation function that receives the name of the entity and calls the creation function according to the name of the entity
                        break;
                    case "3":
                        // Handle Read
                        readGlobal(entityName); //A Read function that receives the name of the entity and calls the Read function according to the name of the entity
                        break;
                    case "4":
                        // Handle ReadAll
                        updateGlobal(entityName);//A ReadAll function that receives the name of the entity and calls the ReadAll function according to the name of the entity
                        break;

                    case "5":
                        // Handle Update
                        if (entityName == "Milestone") { Console.WriteLine("Invalid choice. Please try again."); }
                        else
                        {
                            readAllGlobal(entityName);//A Update function that receives the name of the entity and calls the Update function according to the name of the entity
                        }
                        break;
                    case "6":
                        // Handle Delete
                        if (entityName == "Milestone") { Console.WriteLine("Invalid choice. Please try again."); }
                        else
                        {
                            deleteGlobal(entityName);//A Delete function that receives the name of the entity and calls the Delete function according to the name of the entity
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
        catch (Exception ex) { Console.WriteLine(ex); }
    }



    public static void createGlobal(string entityName)//A creation function that receives the name of the entity and calls the creation function according to the name of the entity
    {
        switch (entityName)
        {
            case "Task":
                createTask();
                break;
            case "Engineer":
                createEngineer();
                break;
            case "Milestone":
                createMilestones();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }

    static void createTask()
    {
        string description = GetInput("Enter the task description: ");
        string alias = GetInput("Enter the task alias: ");
        DateTime createdAtDate = DateTime.Now.AddDays(-s_rand.Next(0, 100));
        string deliverables = GetInput("Enter the task deliverables: ");
        string remarks = GetInput("Enter the task Remarks: ");
        int ? days = GetNullIntInput("Enter the number of days required to complete a task");
        BO.Task task = new BO.Task
        {
            Description = description,
            Alias = alias,
            CreatedAtDate = createdAtDate,
            Status = 0,
            RequierdEffortTime =days==null?null: TimeSpan.FromDays(days??0),
            Deliverables = deliverables,
            Remarks = remarks,
         
        };
        try
        {

            s_bl!.Task.Create(task);
        }
        catch (Exception e) { Console.WriteLine(e); }
    }
    static void createEngineer()//A function that receives data from the user for the Engineer and creates a new Engineer
    {
        string name = GetInput("Please enter the engineer's name: ");
        string email = GetInput("Please enter the engineer's email: ");
        BO.EngineerExperience level = GetComplexityLevelInput("Please enter the engineer's experience level (  Novice, AdvancedBeginner, Competent, Proficient, Expert): \"");
        double cost = Convert.ToDouble(GetInput("Please enter the engineer's cost per hour: "));
        int id;
        int.TryParse(GetInput("Please enter the engineer's ID: "), out id);
        BO.Engineer engineer = new BO.Engineer { Id = id, Name = name, Email = email, Level = level, Cost = cost };
        try
        {

            s_bl!.Engineer.Create(engineer);
        }
        catch (Exception e) { Console.WriteLine(e); }
    }

    ///A Read function that receives the name of the entity and calls the Read function according to the name of the entity
    public static void readGlobal(string entityName)
    {
        switch (entityName)
        {
            case "Task":
                readTask();
                break;
            case "Engineer":
                readEngineer();
                break;
            case "Milestone":
                readMilestone();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }

    static void readTask()//Receives from the Task id and if it exists it prints it
    {
        int idTask;
        int.TryParse((GetInput("Enter the Tasks id: ")), out idTask);
        BO.Task? taskRead = s_bl!.Task.Read(idTask);
        Console.WriteLine(taskRead);

    }

    static void readEngineer()//Receives from the Engineer id and if it exists it prints it
    {
        int idEngineer;
        int.TryParse(GetInput("Enter the Engineer id: "), out idEngineer);
        BO.Engineer? engineerRead = s_bl!.Engineer.Read(idEngineer);
        Console.WriteLine(engineerRead);
    }


    public static void readAllGlobal(string entityName)//A ReadAll function that receives the name of the entity and calls the ReadAll function according to the name of the entity
    {
        switch (entityName)
        {
            case "Task":
                readAllTask();
                break;
            case "Engineer":
                readAllEngineer();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }

    static void readAllTask()//Prints all entities of Task
    {
        List<BO.Task> taskList = s_bl!.Task.ReadAll(null).ToList();
        foreach (var task in taskList)
        {
            Console.WriteLine(task);
        }
    }
    static void readAllEngineer()////Prints all entities of Engineer
    {
        List<BO.Engineer > engineerList = s_bl.Engineer.ReadAll().ToList();
        foreach (var engineer in engineerList)
        {
            Console.WriteLine(engineer);
        }
    }

    public static void deleteGlobal(string entityName)//A Delete function that receives the name of the entity and calls the Delete function according to the name of the entity
    {
        switch (entityName)
        {
            case "Task":
                deleteTask();
                break;
            case "Engineer":
                deleteEngineer();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }

    static void deleteTask()//Gets the id from the Tasks and deletes it if it exists
    {
        try
        {
            int idTask;
            int.TryParse(GetInput("Enter the Tasks id: "), out idTask);
            s_bl!.Task.Delete(idTask);
        }
        catch (Exception e) { Console.WriteLine(e); }

    }
    static void deleteEngineer()//Gets the id from the Engineer and deletes it if it exists
    {
        try
        {
            int idEngineer;
            int.TryParse(GetInput("Enter the Engineer id: "), out idEngineer);
            s_bl!.Engineer.Delete(idEngineer);
        }
        catch (Exception e) { Console.WriteLine(e); }

    }

    public static void updateGlobal(string entityName)//A Update function that receives the name of the entity and calls the Update function according to the name of the entity
    {
        switch (entityName)
        {
            case "Task":
                updateTask();
                break;
            case "Engineer":
                updateEngineer();
                break;
            case "Milestone":
                updateMilestone();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }
    ///Gets the id from the user and if it exists prints it and gets parameters to change creates a new task with all the parameters if they haven't changed leaves the previous value
    static void updateTask()
    {
        int idTask;
        int.TryParse(GetInput("Enter the Tasks id: "), out idTask);
        BO.Task? task = s_bl!.Task.Read(idTask);
        if (task != null)
        {
            Console.WriteLine(task);
            Console.WriteLine("Enter Task Data:");
            // Input fields
            string? description = GetInput("Enter the task description: ");
            string? alias = GetInput("Enter the task alias: ");
            DateTime? start = GetNullDatTimeInput("Enter the task start date and time (YYYY-MM-DD HH:mm:ss): ");
            DateTime? scheduled = GetNullDatTimeInput("Enter the task scheduled date and time (YYYY-MM-DD HH:mm:ss): ");
            DateTime? forecas = GetNullDatTimeInput("Enter the task Enter the task forecas date and time (YYYY-MM-DD HH:mm:ss): ");
            DateTime? deadline = GetNullDatTimeInput("Enter the task deadline date and time (YYYY-MM-DD HH:mm:ss): ");
            DateTime? complete = GetNullDatTimeInput("Enter the task Enter the task complete date and time (YYYY-MM-DD HH:mm:ss): ");
            string? deliverables = GetInput("Enter the task deliverables: ");
            string? remarks = GetInput("Enter Remarks: ");
            int? engineerId = GetNullIntInput("Enter Engineer ID: ");
            BO.EngineerExperience? complexityLevel = GetComplexityLevelInput("Please enter the engineer's experience level (  Novice, AdvancedBeginner, Competent, Proficient, Expert): ");
            //BO.Status? status = GetStatusInput("enter status");
            string? EngineerName = GetInput("Enter the engineers name : ");
            BO.Task updatedTask = new BO.Task
            {
                Id = task.Id,
                Description = description != "" ? description : task.Description,
                Alias = alias != "" ? alias : task.Alias,
                CreatedAtDate=task.CreatedAtDate,
                RequierdEffortTime=task.RequierdEffortTime,
                Status = task.Status,
                Milestone = task.Milestone,
                StartDate = start ?? task.StartDate,
                ScheduledStartDate = scheduled ?? task.ScheduledStartDate,
                ForeCastDate = forecas ?? task.ForeCastDate,
                CompleteDate = complete ?? task.CompleteDate,
                DeadLineDate = deadline ?? task.DeadLineDate,
                Deliverables = deliverables != "" ? deliverables : task.Deliverables,
                Remarks = remarks != "" ? remarks : task.Remarks,
                Engineer = engineerId == null ? task.Engineer == null ? null : new EngineerInTask { Id = task.Engineer.Id, Name = task.Engineer.Name } : new EngineerInTask { Id = engineerId ?? 0, Name = EngineerName },
                ComplexityLevel = complexityLevel ?? task.ComplexityLevel,
            };
            s_bl!.Task.Update(updatedTask);

        }

    }
    ///Gets the id from the user and if it exists prints it and gets parameters to change creates a new Engineer with all the parameters if they haven't changed leaves the previous value
    static void updateEngineer()
    {
        int idEngineer;
        int.TryParse(GetInput("Enter the Engineer id: "), out idEngineer);
        BO.Engineer? engineer = s_bl!.Engineer.Read(idEngineer);
        if (engineer != null)
        {
            Console.WriteLine(engineer);
            // Input fields
            string? name = GetInput("Please enter the engineer's name: ");
            string? email = GetInput("Please enter the engineer's email: ");
            BO.EngineerExperience? level = GetComplexityLevelInput("Please enter the engineer's experience level (  Novice, AdvancedBeginner, Competent, Proficient, Expert): ");
            double? cost = GetNullDoubleInput("Please enter the engineer's cost per hour: ");
            int? taskId = GetNullIntInput("Enter task Id: ");
            string alias = GetInput("Enter the alias  ");
            TaskinEngineer? taskinEngineer = new TaskinEngineer { Id = taskId ?? 0, Alias = alias };

            BO.Engineer updatedEngineer = new BO.Engineer
            {
                Id = engineer.Id,
                Name = name != "" ? name : engineer.Name,
                Email = email != "" ? email : engineer.Email,
                Level = level ?? engineer.Level,
                Cost = cost ?? engineer.Cost,
                Task = taskId == null ? null : taskinEngineer
            };
            s_bl!.Engineer.Update(updatedEngineer);
        }
    }

    ///A function with a print-to-screen parameter accepts a value from the user and returns it as a string
    static string GetInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
    //A function with a print-to-screen parameter that receives a int from the user and returns a variable with the content and if nothing is entered a null value
    public static int? GetNullIntInput(string message)
    {

        int? inputMutable;
        string input = GetInput(message);
        bool success = int.TryParse(input, out int parsedValue);

        if (success)
        {
            inputMutable = parsedValue;
        }
        else
        {
            inputMutable = null;
        }
        return inputMutable;
    }
    public static int GetIntInput(string message)
    {

        int inputMutable;
        string input = GetInput(message);
        bool success = int.TryParse(input, out int parsedValue);

        if (success)
        {
            inputMutable = parsedValue;
        }
        else
        {
            throw new BlNullPropertyException("null input");
        }
        return inputMutable;
    }
    /// A function with a print-to-screen parameter  that receives a DateTime from the user and returns a variable with the content and if nothing is entered a null value
    public static DateTime? GetNullDatTimeInput(string message)
    {

        DateTime? inputMutable;
        string? input = GetInput(message);
        bool success = DateTime.TryParse(input, out DateTime parsedValue);

        if (success)
        {
            inputMutable = parsedValue;
        }
        else
        {
            inputMutable = null;
        }
        return inputMutable;
    }
    ///A function with a print-to-screen parameter that receives a double from the user and returns a variable with the content and if nothing is entered a null value
    public static double? GetNullDoubleInput(string message)
    {

        double? inputMutable;
        string? input = GetInput(message);
        bool success = double.TryParse(input, out double parsedValue);

        if (success)
        {
            inputMutable = parsedValue;
        }
        else
        {
            inputMutable = null;
        }
        return inputMutable;
    }

    ///A function with a print-to-screen parameter accepts a value from the user and returns it as a bool
    static bool GetBoolInput(string message)
    {
        Console.Write(message);
        string? input = Console.ReadLine();
        return bool.Parse(input!);
    }
    ///A function with a print-to-screen parameter accepts a value from the user and returns it as a DateTime
    static DateTime GetDateTimeInput(string message)
    {

        DateTime createdAtDate;
        DateTime.TryParseExact(GetInput(message), "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out createdAtDate);
        return createdAtDate;
    }
    ///A function with a print-to-screen parameter accepts a value from the user and returns it as a EngineerExperience Enum
    static BO.EngineerExperience GetComplexityLevelInput(string message)
    {
        Console.Write(message);
        string? level = Console.ReadLine();
        BO.EngineerExperience experienceLevel;
        Enum.TryParse(level, out experienceLevel);
        return experienceLevel;
    }
    //static BO.Status GetStatusInput(string message)
    //{
    //    Console.Write(message);
    //    string? status = Console.ReadLine();
    //    BO.Status statusLevel;
    //    Enum.TryParse(status, out statusLevel);
    //    return statusLevel;
    //}
    static void createMilestones()
    {
        try
        {
            Console.WriteLine("Enter the programs start date and time (yyyy-MM-dd):");
            string? userInput = Console.ReadLine();
            if (!(DateTime.TryParse(userInput, out DateTime startDateTime) && startDateTime > DateTime.Now))
                throw new InvalidInputException("Invalid date format or date must be later than the current date.");
            Console.WriteLine("Enter the programs end date and time (yyyy-MM-dd):");
            userInput = Console.ReadLine();
            if (!(DateTime.TryParse(userInput, out DateTime endDateTime) && startDateTime > DateTime.Now))
                throw new InvalidInputException("Invalid date format or date must be later than the current date.");
            if (startDateTime > endDateTime)
                throw new InvalidInputException(" the endDateTime smaller then the startDateTime");
            Tools.InitDateScheduleTime(startDateTime, endDateTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        s_bl.Milestone.Create();
    }
    static void readMilestone()
    {
        int idMilestone;
        int.TryParse((GetInput("Enter the Milestone id: ")), out idMilestone);
        BO.Milestone? milestoneRead = s_bl!.Milestone.Read(idMilestone);
        Console.WriteLine(milestoneRead);

    }
    static void updateMilestone()
    {
        int idMilestone;
        int.TryParse((GetInput("Enter the Milestone id: ")), out idMilestone);
        Console.WriteLine(s_bl.Milestone.Read(idMilestone));
        Console.WriteLine("Enter Milestone Data:");
        string? remarks = GetInput("Enter Remarks: ");
        string? description = GetInput("Enter the Milestone description: ");
        string? alias = GetInput("Enter the Milestone alias: ");
        s_bl.Milestone.Update(idMilestone, alias, description, remarks);
    }

}
