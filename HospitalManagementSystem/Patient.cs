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
            Console.WriteLine("DOTNET Hospital Management System");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Patient Menu\n");
            Console.WriteLine($"Welcome to DOTNET Hospital Management System {FullName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit System");

            Console.Write("\nEnter choice: ");
            var choice = Console.ReadLine();

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
                default: Console.WriteLine("Invalid option. Try again."); break;
            }
        }
    }
}