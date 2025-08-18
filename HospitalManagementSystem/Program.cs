namespace HospitalManagementSystem;

internal class Program
{
    private const string USERS_DATA =
        "/Users/koanlin/Dev/projects/HospitalManagementSystem/HospitalManagementSystem/data.txt";

    private static void Main()
    {
        var users = new List<User>();
        var lines = File.ReadAllLines(USERS_DATA);

        // load users
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var user = User.CreateUserFromData(parts);
            if (user != null) users.Add(user);
        }

        // login loop
        while (true)
        {
            Console.Write("ID: ");
            var idInput = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            var password = Console.ReadLine() ?? "";

            var currentUser = User.LoginWithUserId(users, idInput, password);

            if (currentUser == null)
            {
                Console.WriteLine("Invalid credentials, try again.\n");
                continue;
            }

            Console.WriteLine($"Welcome, {currentUser.FullName} ({currentUser.Role})\n");
            currentUser.RenderUserMenu(); // use of polymorphic!!
        }
    }
}