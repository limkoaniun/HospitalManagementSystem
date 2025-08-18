namespace HospitalManagementSystem;

public class Doctor : User
{
    public Doctor()
    {
        Role = "DOCTOR";
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
            Console.WriteLine("Doctor Menu\n");
            Console.WriteLine($"Welcome to DOTNET Hospital Management System {FullName}\n");

            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List doctor details");
            Console.WriteLine("2. List patients");
            Console.WriteLine("3. List appointments");
            Console.WriteLine("4. Check particular patient");
            Console.WriteLine("5. List appointments with patient");
            Console.WriteLine("6. Logout");
            Console.WriteLine("7. Exit");

            Console.Write("\nEnter choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Console.WriteLine("List doctor details selected."); break;
                case "2": Console.WriteLine("List patients selected."); break;
                case "3": Console.WriteLine("List appointments selected."); break;
                case "4": Console.WriteLine("Check particular patient selected."); break;
                case "5": Console.WriteLine("List appointments with patient selected."); break;
                case "6":
                    Console.WriteLine("Logging out...");
                    return;
                case "7":
                    Console.WriteLine("Exiting system...");
                    Environment.Exit(0);
                    break;
                default: Console.WriteLine("Invalid option. Try again."); break;
            }
        }
    }
}