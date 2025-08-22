namespace HospitalManagementSystem;

public class Patient : User
{
    public Patient()
    {
        Role = "PATIENT";
    }

    public override void Run()
    {
        while (true)
        {
            Console.Clear();

            string welcome = $"Welcome to DOTNET Hospital Management System {FullName}";

            // Header box
            Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│              DOTNET Hospital Management System              │");
            Console.WriteLine("├─────────────────────────────────────────────────────────────┤");
            Console.WriteLine("│                          Patient Menu                       │");
            Console.WriteLine("└─────────────────────────────────────────────────────────────┘");
            Console.WriteLine();

            Console.WriteLine(welcome);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit system");
            Console.WriteLine();

            Console.Write("Enter choice: ");
            var choice = (Console.ReadLine() ?? string.Empty).Trim();

            switch (choice)
            {
                case "1": Console.WriteLine("List patient details selected."); break;
                case "2": Console.WriteLine("List my doctor details selected."); break;
                case "3": Console.WriteLine("List all appointments selected."); break;
                case "4": Console.WriteLine("Book appointment selected."); break;
                case "5":
                    Console.WriteLine("Returning to login...");
                    return;
                case "6":
                    Console.WriteLine("Exiting system...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}