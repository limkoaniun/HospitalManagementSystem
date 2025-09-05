using System;

namespace HospitalManagementSystem
{
    internal class Program
    {
        private readonly UserRepository userRepository;
        private readonly AppointmentRepository appointmentRepository;

        private Program()
        {
            userRepository = new UserRepository();
            appointmentRepository = new AppointmentRepository();
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
            string? idText = Console.ReadLine();
            int idInput;
            try
            {
                idInput = Convert.ToInt32(idText);
            }
            catch
            {
                Console.WriteLine("Invalid ID format.");
                return null;
            }

            Console.Write("Password: ");
            string? password = Console.ReadLine();

            var currentUser = userRepository.GetUserById(idInput);
            if (currentUser == null || currentUser.Password != password)
            {
                Console.WriteLine("Invalid credentials.");
                return null;
            }

            Console.WriteLine("Valid credentials\n");
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
                currentUser.Run(userRepository, appointmentRepository); // Goes into Doctor/Admin/Patient menu
            }
        }

        private static void Main()
        {
            var program = new Program();
            program.Run();
        }
    }
}