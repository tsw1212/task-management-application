


using System.Data.SqlTypes;

namespace DalTest;
using DalApi;
using Dal;
using DO;
using System.Reflection.Metadata.Ecma335;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Globalization;
///A figure that can be assumed that the user enters normal values and not null, therefore in many places there is none?
///The program checks the correctness of the independent income and checks if it is possible to enter, change and update all types of entities
public class Program
{
    static readonly IDal s_dal = Factory.Get; //stage 4
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
            DisplayMainMenu();//Main main program for choosing which entity to handle

        }
        catch (Exception ex) { Console.WriteLine(ex); }
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
            Console.WriteLine("3. Dependency");

            Console.Write("Enter your choice: ");
            string ?input = Console.ReadLine();

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
                    DisplaySubMenu("Dependency");//calls a general function that refers to all the delete/add options and the like secondary main

                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine();
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
                Console.WriteLine("2. Adding a new object to the " + entityName + " list");
                Console.WriteLine("3. Object display by identifier");
                Console.WriteLine("4. The display of the list of all " + entityName);
                Console.WriteLine("5. Updating existing object data");
                Console.WriteLine("6. Deleting an existing object from a list");

                Console.Write("Enter your choice: ");
                string ?input = Console.ReadLine();

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
                        readAllGlobal(entityName);//A ReadAll function that receives the name of the entity and calls the ReadAll function according to the name of the entity
                        break;
                    case "5":
                        // Handle Update
                        updateGlobal(entityName);//A Update function that receives the name of the entity and calls the Update function according to the name of the entity
                        break;
                    case "6":
                        // Handle Delete
                        deleteGlobal(entityName);//A Delete function that receives the name of the entity and calls the Delete function according to the name of the entity
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
            case "Dependency":
                createDependency();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }
    ///A function that receives data from the user for the task and creates a new task

    static void createTask()
    {
        string description = GetInput("Enter the task description: ");
        string alias = GetInput("Enter the task alias: ");
        DO.Task task = new DO.Task(  0, description, alias,  DateTime.Now ,new TimeSpan(0) ,null,false);
        s_dal!.Task.Create(task);
    }

    static void createEngineer()//A function that receives data from the user for the Engineer and creates a new Engineer
    {
        string name = GetInput("Please enter the engineer's name: ");
        string email = GetInput("Please enter the engineer's email: ");
        string levelInput = GetInput("Please enter the engineer's experience level (Novice, AdvancedBeginner, Competent, Proficient, Expert): ");
        EngineerExperience level;
        Enum.TryParse(levelInput, out level);
        double cost = Convert.ToDouble(GetInput("Please enter the engineer's cost per hour: "));
        int id;
        int.TryParse(GetInput("Please enter the engineer's ID: "), out id);

        Engineer engineer = new (id,name, email, level, cost);
        try
        {

            s_dal!.Engineer.Create(engineer);
        }
        catch (Exception e) { Console.WriteLine(e); }
    }

    static void createDependency()//A function that receives data from the user for the Dependency and creates a new Dependency
    {
        int dependentTask;
        int.TryParse(GetInput("Enter the DependentTask: "), out dependentTask);
        int dependsTask;
        int.TryParse(GetInput("Enter the DependsTask: "), out dependsTask);

        Dependency dependency = new Dependency(dependentTask, dependsTask);
        s_dal!.Dependency.Create(dependency);
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
            case "Dependency":
                readDependency();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }

    static void readTask()//Receives from the Task id and if it exists it prints it
    {
        int idTask;
        int.TryParse((GetInput("Enter the Tasks id: ")), out idTask);
        Task ?taskRead = s_dal!.Task.Read(idTask);
        Console.WriteLine(taskRead);

    }

    static void readEngineer()//Receives from the Engineer id and if it exists it prints it
    {
        int idEngineer;
        int.TryParse(GetInput("Enter the Engineer id: "), out idEngineer);
        Engineer ?engineerRead = s_dal!.Engineer.Read(idEngineer);
        Console.WriteLine(engineerRead);
    }

    static void readDependency()//Receives from the Dependency id and if it exists it prints it
    {
        int idDependency;
        int.TryParse(GetInput("Enter the Dependency id: "), out idDependency);
        Dependency ?dependencyRead = s_dal!.Dependency.Read(idDependency);
        Console.WriteLine(dependencyRead);
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
            case "Dependency":
                readAllDependency();
                break;
            default:
                throw (new Exception("no such entity name"));
        }

    }

    static void readAllTask()//Prints all entities of Task
    {
        List<Task?> taskList = s_dal!.Task.ReadAll().ToList();
        foreach (var task in taskList)
        {
            Console.WriteLine(task);
        }
    }
    static void readAllEngineer()////Prints all entities of Engineer
    {
        List<Engineer?> engineerList = s_dal!.Engineer.ReadAll().ToList();
        foreach (var engineer in engineerList)
        {
            Console.WriteLine(engineer);
        }
    }
    static void readAllDependency()//Prints all entities of Dependency
    {
        List<Dependency?> dependencyList = s_dal!.Dependency.ReadAll().ToList();
        foreach (var dependency in dependencyList)
        {
            Console.WriteLine(dependency);
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
            case "Dependency":
                deleteDependency();
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
            s_dal!.Task.Delete(idTask);
        }
        catch (Exception e) { Console.WriteLine(e); }

    }
    static void deleteEngineer()//Gets the id from the Engineer and deletes it if it exists
    {
        try
        {
            int idEngineer;
            int.TryParse(GetInput("Enter the Engineer id: "), out idEngineer);
            s_dal!.Engineer.Delete(idEngineer);
        }
        catch (Exception e) { Console.WriteLine(e); }

    }

    static void deleteDependency()//Gets the id from the Dependency and deletes it if it exists
    {
        try
        {
            int idDependency;
            int.TryParse(GetInput("Enter the Dependency id: "), out idDependency);
            s_dal!.Dependency.Delete(idDependency);
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
            case "Dependency":
                updateDependency();
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
        Task ?task = s_dal!.Task.Read(idTask);
        if (task != null)
        {
            Console.WriteLine(task);

            Console.WriteLine("Enter Task Data:");

            // Input fields
            string? description = GetInput("Enter the task description: ");
            string? alias = GetInput("Enter the task alias: ");
            DateTime? start = GetNullDatTimeImput("Enter the task start date and time (YYYY-MM-DD HH:mm:ss): ");
            DateTime? scheduled = GetNullDatTimeImput("Enter the task scheduleded date and time (YYYY-MM-DD HH:mm:ss): ");
            DateTime? deadline = GetNullDatTimeImput("Enter the task deadline date and time (YYYY-MM-DD HH:mm:ss): ");
            DateTime? complete = GetNullDatTimeImput("Enter the task Enter the task complete date and time (YYYY-MM-DD HH:mm:ss): ");
            string? deliverables = GetInput("Enter the task deliverables: ");
            string? remarks = GetInput("Enter Remarks: ");
            int? engineerId = GetNullIntImput("Enter Engineer ID: ");
            EngineerExperience? complexityLevel = GetComplexityLevelInput("Please enter the engineer's experience level (  Novice, AdvancedBeginner, Competent, Proficient, Expert): ");

            //Task updatedTask = new Task
            //{   Id = idTask,
            //    StartDate= start ?? task.StartDate,
            //    ScheduledDate=scheduled ?? task.ScheduledDate,
            //    CompleteDate= complete ?? task.CompleteDate,
            //    DeadLineDate= deadline ?? task.DeadLineDate,
            //    Deliverables= deliverables != "" ? deliverables : task.Deliverables,
            //    Remarks = remarks != "" ? remarks : task.Remarks, EngineerId = engineerId ?? task.EngineerId, ComplexityLevel = complexityLevel ?? task.ComplexityLevel,
            //    Description = description ?? task.Description, Alias = alias != "" ? alias : task.Alias 
            //};
            Task updatedTask = new Task(
               idTask,
               alias != "" ? alias : task.Alias,
                description ?? task.Description,
              task.CreatedAtDate,
              task.RequierdEffortTime,
                engineerId ?? task.EngineerId,
              task.IsMilestone,
                start ?? task.StartDate,
               scheduled ?? task.ScheduledDate,
                deadline ?? task.DeadLineDate,
                complete ?? task.CompleteDate,
                 deliverables != "" ? deliverables : task.Deliverables,
                remarks != "" ? remarks : task.Remarks,
                complexityLevel ?? task.ComplexityLevel
               );

            s_dal!.Task.Update(updatedTask);

        }

    }
    ///Gets the id from the user and if it exists prints it and gets parameters to change creates a new Engineer with all the parameters if they haven't changed leaves the previous value
    static void updateEngineer()
    {
        int idEngineer;
        int.TryParse(GetInput("Enter the Engineer id: "), out idEngineer);
        Engineer ?engineer = s_dal!.Engineer.Read(idEngineer);
        if (engineer != null)
        {
            Console.WriteLine(engineer);
            // Input fields
            string? name = GetInput("Please enter the engineer's name: ");
            string? email = GetInput("Please enter the engineer's email: ");
            EngineerExperience? level = GetComplexityLevelInput("Please enter the engineer's experience level (  Novice, AdvancedBeginner, Competent, Proficient, Expert): ");
            double? cost = GetNullDoubleInput("Please enter the engineer's cost per hour: ");


            Engineer updatedEngineer = new(engineer.Id,name != ""?name:engineer.Name,email!=""?email:engineer.Email,level??engineer.Level,cost??engineer.Cost);
            s_dal!.Engineer.Update(updatedEngineer);
        }
    }
    ///Gets the id from the user and if it exists prints it and gets parameters to change creates a new Dependency with all the parameters if they haven't changed leaves the previous value
    static void updateDependency()
    {
        int idDependency;
        int.TryParse(GetInput("Enter the Dependency id: "), out idDependency);
        Dependency ?dependency = s_dal!.Dependency.Read(idDependency);
        if (dependency != null)
        {
            Console.WriteLine(dependency);
            // Input fields
            int? dependentTask=GetNullIntImput("Enter the DependentTask: ");
            int? dependsTask=GetNullIntImput("Enter the DependsTask: ");
            Dependency updatedDependency = new (dependency.Id, dependentTask ?? dependency.DependentTask, dependsTask ?? dependency.DependsTask);
            s_dal!.Dependency.Update(updatedDependency);
        }
    }
    ///A function with a print-to-screen parameter accepts a value from the user and returns it as a string
    static string GetInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
    //A function with a print-to-screen parameter that receives a int from the user and returns a variable with the content and if nothing is entered a null value
    public static int? GetNullIntImput(string message)
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
    /// A function with a print-to-screen parameter  that receives a DateTime from the user and returns a variable with the content and if nothing is entered a null value
    public static DateTime? GetNullDatTimeImput(string message)
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
        string ?input = GetInput(message);
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
        Console.Write(message);

        DateTime createdAtDate;
        DateTime.TryParseExact(GetInput("Enter the created at date and time (yyyy-MM-dd HH:mm:ss): "), "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out createdAtDate);
        return createdAtDate;
    }
    ///A function with a print-to-screen parameter accepts a value from the user and returns it as a EngineerExperience Enum
    static EngineerExperience GetComplexityLevelInput(string message)
    {
        Console.Write(message);
        string ?level = Console.ReadLine();
        EngineerExperience experienceLevel;
        Enum.TryParse(level, out experienceLevel);
        return experienceLevel;
    }
}







