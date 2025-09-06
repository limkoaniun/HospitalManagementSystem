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
            Ui.RenderHeader("Login");

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
            string? password = Ui.ReadPasswordMasked();

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
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                    continue;
                }

                Console.WriteLine("Welcome, {0} ({1})", currentUser.FullName, currentUser.Role);
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