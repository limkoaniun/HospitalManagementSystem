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
        Console.Write("ID: ");
        var idInput = Convert.ToInt32(Console.ReadLine());

        Console.Write("Password: ");
        var password = Console.ReadLine();

        var currentUser = userRepository.GetUserById(idInput);
        if (currentUser == null || currentUser.Password != password)
        {
            return null;
        }

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
                continue;
            }

            Console.WriteLine($"Welcome, {currentUser.FullName} ({currentUser.Role})\n");
            currentUser.Run();
        }
    }

    private static void Main()
    {
        var program = new Program();
        program.Run();
    }
}