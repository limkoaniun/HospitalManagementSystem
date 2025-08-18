namespace HospitalManagementSystem;

public class Administrator : User
{
    public Administrator()
    {
        Role = "ADMIN";
    }

    public override void LoadUserFromData(string[] data)
    {
        // data.txt format: Role,ID,Password,FullName,Email
        Id = Convert.ToInt32(data[1]);
        Password = data[2];
        FullName = data[3];
        Email = data[4];
    }

    public override void RenderUserMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("DOTNET Hospital Management System");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Administrator Menu\n");
            Console.WriteLine($"Welcome to DOTNET Hospital Management System {FullName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List all doctors");
            Console.WriteLine("2. Check doctor details");
            Console.WriteLine("3. List all patients");
            Console.WriteLine("4. Check patient details");
            Console.WriteLine("5. Add doctor");
            Console.WriteLine("6. Add patient");
            Console.WriteLine("7. Logout");
            Console.WriteLine("8. Exit");

            Console.Write("\nEnter choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Console.WriteLine("List all doctors selected."); break;
                case "2": Console.WriteLine("Check doctor details selected."); break;
                case "3": Console.WriteLine("List all patients selected."); break;
                case "4": Console.WriteLine("Check patient details selected."); break;
                case "5": Console.WriteLine("Add doctor selected."); break;
                case "6": Console.WriteLine("Add patient selected."); break;
                case "7":
                    Console.WriteLine("Logging out...");
                    return; // back to login
                case "8":
                    Console.WriteLine("Exiting system...");
                    Environment.Exit(0);
                    break;
                default: Console.WriteLine("Invalid option. Try again."); break;
            }
        }
    }
}