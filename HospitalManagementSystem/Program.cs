namespace HospitalManagementSystem;

internal class Program
{
    private readonly List<User> users;

    private readonly string USERS_DATA =
        "/Users/koanlin/RiderProjects/HospitalManagementSystem/HospitalManagementSystem/data.txt";

    private Program()
    {
        users = new List<User>();
        LoadUsers();
    }

    // GetUserById
    private User? GetUserById(int id)
    {
        return users.FirstOrDefault(x => x.Id == id);
    }

    private void LoadUsers()
    {
        var lines = File.ReadAllLines(USERS_DATA);

        // load users
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 5) continue;

            User user;

            var role = parts[0];

            if (role == "ADMIN")
            {
                user = new Administrator();
            }
            else if (role == "DOCTOR")
            {
                user = new Doctor();
            }
            else
            {
                user = new Patient();
            }

            // Format: Role,ID,Password,FullName,Email
            user.Id = Convert.ToInt32(parts[1]);
            user.Password = parts[2];
            user.FullName = parts[3];
            user.Email = parts[4];
            users.Add(user);
        }
    }

    private void Login()
    {
        while (true)
        {
            Console.Write("ID: ");
            int idInput = Convert.ToInt32(Console.ReadLine());

            Console.Write("Password: ");
            var password = Console.ReadLine();

            var currentUser = GetUserById(idInput);

            if (currentUser == null)
            {
                Console.WriteLine("Invalid credentials, try again.\n");
                continue;
            }

            Console.WriteLine($"Welcome, {currentUser.FullName} ({currentUser.Role})\n");
            currentUser.Run(); // use of polymorphic
        }
    }

    private void Run()
    {
        while (true)
        {
            Login();
        }
    }

    private static void Main()
    {
        var program = new Program();
        program.Run();
    }
}