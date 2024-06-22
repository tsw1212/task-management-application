using DalApi;
using DO;
using System.Data.Common;
using System.Security.Cryptography;

namespace DalTest;
//Creation of 20 tasks 5 engineers and 40 depending between an engineer
//and a regular programmer
public static class Initialization
{
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();
    private static void creatEngineer()
    {
        string[] EngineerNames =
        {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var _name in EngineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000);//Identity tax lottery
            while (s_dal!.Engineer.Read(_id) != null);

            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 4);//The worker level lottery

            string _email = _id + "@gmail.com";//Create an email address

            double _cost = s_rand.Next(75, 150) * 0.12 * 10;  //Salary lottery in the appropriate range for every engineer
            Engineer newEng = new(_id, _name, _email, _level, _cost);

            int id = s_dal!.Engineer.Create(newEng);//Creating the object using a function create
        }

    }

    //Creating an task
    private static void creatTask()
    {

        int i = 0;
        string[] engineerTasks =
         {
            //"Design a new database schema for a web application",
            //"Develop an algorithm to optimize resource allocation in a manufacturing process",
            //"Implement a software module to control a robotic arm",
            //"Write unit tests for an existing codebase",
            //"Perform load testing on a web server",
            //"Troubleshoot and fix a bug in a production system",
            //"Create a technical specification document for a new feature",
            //"Design and implement a data visualization dashboard",
            //"Optimize the performance of a database query",
            //"Conduct a code review for a peer's code",
            //"Integrate a third-party API into an existing application",
            //"Implement a machine learning algorithm for predictive modeling",
            //"Configure and deploy a cloud infrastructure using AWS",
            //"Develop a responsive user interface for a mobile app",
            //"Design and implement a scalable microservices architecture",
            //"Write documentation for a software library",
            //"Perform a security audit on a web application",
            //"Optimize the memory usage of a software application",
            //"Set up and configure a continuous integration/continuous deployment (CI/CD) pipeline",
            //"Design and implement a real-time chat application",
            //"Create a test plan for a software release",
            //"Implement a recommendation system based on user preferences",
            //"Perform database optimization for a high-traffic website",
            //"Write a script to automate a repetitive task",
            //"Design and implement a fault-tolerant distributed system",
            //"Create a responsive email template for a marketing campaign",
            //"Implement a natural language processing (NLP) algorithm for sentiment analysis",
            //"Perform performance tuning for a database server",
            //"Integrate a payment gateway into an e-commerce platform",
            //"Design and implement an authentication and authorization system",
            "Develop a machine learning model for image recognition",
            "Implement a caching mechanism to improve application performance",
            "Create a data pipeline for processing and analyzing large datasets",
            "Perform system capacity planning for a growing infrastructure",
            "Design and implement an automated testing framework",
            "Optimize the network configuration for a distributed system",
            "Implement a data encryption algorithm for sensitive information",
            "Create a fault detection mechanism for a distributed system",
            "Develop a monitoring system for tracking application performance",
            "Design and implement a content management system ",
        };
        string[] _deliverablesTask = {"Code implementation for a new feature",
            //"Bug fixing and troubleshooting",
            //"Unit tests for code coverage",
            //"Code documentation and commenting",
            //"Technical design documentation",
            //"Code review and peer programming",
            //"Integration testing with other modules",
            //"Performance optimization of code",
            //"Database schema design and implementation",
            //"API development and integration",
            //"User interface design and implementation",
            //"Creation of user stories and acceptance criteria",
            //"Deployment and release management",
            //"Creation of software requirements specifications",
            //"System architecture design",
            //"Creation of test plans and test cases",
            //"Security testing and vulnerability assessment",
            //"Data migration and database management",
            //"Collaboration with cross-functional teams",
            //"Continuous integration and delivery setup",
            //"Documentation of user manuals and guides",
            //"Third-party library and tool evaluation",
            //"Technical support for end-users",
            //"Performance profiling and analysis",
            //"Creation of software development guidelines",
            //"Version control and code repository management",
            //"Adherence to coding standards and best practices",
            //"Agile project management and sprint planning",
            //"Software quality assurance and testing",
            //"Creation of system and process documentation",
            //"Implementation of software analytics and monitoring",
            "Integration with external systems and APIs",
            "Creation of software installation packages",
            "Creation of technical training materials",
            "Code refactoring and optimization",
            "Troubleshooting and root cause analysis",
            "Software maintenance and bug triaging",
            "Implementation of data validation and sanitization",
            "Creation of automated test scripts",
            "Performance benchmarking and load testing",
            "Software localization and internationalization" };
        foreach (var _description in engineerTasks)
        {
            string[] words = _description.Split(' ');
            string _alias = words[0];
            DateTime _create = DateTime.Now.AddDays(-s_rand.Next(0, 50));
            TimeSpan timeSpan = TimeSpan.FromDays(s_rand.Next(10, 50));
            var engineerList = s_dal.Engineer.ReadAll().ToList();
            int engineerId = engineerList[ s_rand.Next(0, engineerList.Count())].Id;
            i++;
            DO.Task newTask = new DO.Task ( 0,  _description, _alias,  _create , timeSpan, engineerId);
            int id = s_dal!.Task.Create(newTask);
        }

    }
    private static void creatDependency()
    {     
        List<DO.Task?> tasks = s_dal!.Task.ReadAll().ToList();
        //for (int i = 0; i < 40; i++)
        //{

        //    int indexTask1 = s_rand.Next(0, tasks.Count - 1);
        //    int _dependentTask = tasks[indexTask1]!.Id;
        //    int indexTask2 = s_rand.Next(0, tasks.Count - 1);
        //    while (indexTask2 == indexTask1)
        //    {
        //        indexTask2 = s_rand.Next(0, tasks.Count - 1);
        //    }
        //    int _dependsTask = tasks[indexTask2]!.Id;
        //    Dependency newDependency = new(0, _dependentTask, _dependsTask);
        //    int id = s_dal!.Dependency.Create(newDependency);
        //}.

        //בגלל שכל חישוב התלויות העמיס על המחשב יצרנו בעצמנו את התלויות
        for (int i = 0; i < 5; i++)
        {
            Dependency newDependency = new(0, tasks[i]?.Id ?? 0, tasks[i + 4]?.Id ?? 0);
            int id = s_dal!.Dependency.Create(newDependency);
        }
        Dependency newDependency1 = new(0, tasks[9]?.Id ?? 0, tasks[6]?.Id ?? 0);
        s_dal!.Dependency.Create(newDependency1);
        Dependency newDependency2 = new(0, tasks[1]?.Id??0, tasks[2]?.Id??0);
        s_dal!.Dependency.Create(newDependency2);





    }

    public static void Do() //stage 4
    {
        s_dal = DalApi.Factory.Get;

        creatEngineer();
        creatTask();
        creatDependency();
    }
    public static void Reset() //stage 5
    {
        s_dal = DalApi.Factory.Get;

        s_dal.Dependency.Reset();
        s_dal.Task.Reset();
        s_dal.Engineer.Reset();
    }

}
