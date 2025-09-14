using System;

namespace HospitalManagementSystem
{
    internal class Program
    {
        private readonly UserRepository userRepository;
        private readonly AppointmentRepository appointmentRepository;

        public Program()
        {
            userRepository = new UserRepository();
            appointmentRepository = new AppointmentRepository();
        }

        ~Program()
        {
            // garbage collection
            GC.Collect(); // 1. force collection
            GC.WaitForPendingFinalizers(); // 2. wait for finalizers
            GC.Collect(); // 3. collect finalizable survivors
        }

        private User? Login()
        {
            Console.Clear();
            Ui.RenderHeader("Login");

            Console.Write("ID: ");
            string? idText = Console.ReadLine();
            int idInput;
            
            // exception handling for invalid ID format
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
            // extension method in UI
            string? password = this.ReadPasswordMasked();

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
                // Run the role-specific menu (as in Doctor/Admin/Patient menu)
                currentUser.Run(userRepository, appointmentRepository);
            }
        }

        private static void Main()
        {
            var program = new Program();
            program.Run();
        }
    }
}