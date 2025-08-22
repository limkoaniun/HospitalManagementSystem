namespace HospitalManagementSystem;

internal class Program
{
    private UserRepository userRepository;

    private Program()
    {
        userRepository = new UserRepository();
    }

    private User? Login()
    {
        Console.Clear();

        // Header box
        Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
        Console.WriteLine("│              DOTNET Hospital Management System              │");
        Console.WriteLine("├─────────────────────────────────────────────────────────────┤");
        Console.WriteLine("│                          Login                              │");
        Console.WriteLine("└─────────────────────────────────────────────────────────────┘");
        Console.WriteLine();

        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out var idInput))
        {
            Console.WriteLine("Invalid ID format.\n");
            return null;
        }

        Console.Write("Password: ");
        var password = Console.ReadLine();

        var currentUser = userRepository.GetUserById(idInput);
        if (currentUser == null || currentUser.Password != password)
        {
            Console.WriteLine("Invalid credentials.\n");
            return null;
        }

        Console.WriteLine("Valid Credentials\n");
        return currentUser;
    }

    private void Run()
    {
        while (true)
        {
            var currentUser = Login();

            if (currentUser == null)
            {
                Console.WriteLine("Invalid credentials, try again.\n");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                continue;
            }

            Console.WriteLine("Welcome, {0} ({1})\n", currentUser.FullName, currentUser.Role);
            currentUser.Run(); // goes into Doctor/Admin/Patient menu
        }
    }

    private static void Main()
    {
        var program = new Program();
        program.Run();
    }
}