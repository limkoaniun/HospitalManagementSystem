using System;

namespace HospitalManagementSystem
{
    // Main program class that manages the hospital management system application
    // Handles user authentication and navigation to role-specific menus
    internal class Program
    {
        private readonly UserRepository userRepository;
        private readonly AppointmentRepository appointmentRepository;

        // Constructor initializes the repositories for user and appointment data
        public Program()
        {
            userRepository = new UserRepository();
            appointmentRepository = new AppointmentRepository();
        }

        // Destructor demonstrates garbage collection - forces cleanup when program ends
        // This is an example for educational purposes - normally not needed in production
        ~Program()
        {
            // Garbage collection example showing the three-step process
            GC.Collect(); // 1. Force immediate garbage collection
            GC.WaitForPendingFinalizers(); // 2. Wait for all finalizers to complete
            GC.Collect(); // 3. Collect objects that were finalized
        }

        // Handles user login by validating credentials against stored user data
        // Returns: User object if login successful, null if credentials invalid
        private User? Login()
        {
            Console.Clear();
            Ui.RenderHeader("Login");

            // Prompt for user ID
            Console.Write("ID: ");
            string? idText = Console.ReadLine();
            int idInput;

            // Validate and parse the user ID input
            // Exception handling for invalid ID format (non-numeric input)
            try
            {
                idInput = Convert.ToInt32(idText);
            }
            catch
            {
                Console.WriteLine("Invalid ID format.");
                return null; // Return null to indicate failed login
            }

            // Prompt for password with masking for security
            Console.Write("Password: ");
            // Extension method usage - password input masked in console (shows * instead of actual characters)
            string? password = this.ReadPasswordMasked();

            // Verify credentials against stored user data from text file
            var currentUser = userRepository.GetUserById(idInput);
            
            // Check if user exists and password matches
            if (currentUser == null || currentUser.Password != password)
            {
                return null; // Invalid credentials
            }

            return currentUser; // Successful login
        }


        // Main application loop that handles login and role-based menu navigation
        // Continuously prompts for login until valid credentials are provided
        private void Run()
        {
            // Infinite loop keeps the application running
            while (true)
            {
                // Attempt to log in user
                var currentUser = Login();
                
                // Handle failed login attempts
                if (currentUser == null)
                {
                    Console.WriteLine("Invalid credentials, try again.\n");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                    continue; // Return to login screen
                }

                // Successful login - display welcome message
                Console.WriteLine("Welcome, {0} ({1})", currentUser.FullName, currentUser.Role);
                
                // Run the role-specific menu based on user type (Doctor/Admin/Patient)
                // This uses polymorphism - each user type has its own Run implementation
                currentUser.Run(userRepository, appointmentRepository);
                
                // When Run() returns, user has logged out - loop continues to login screen
            }
        }

        // Application entry point
        private static void Main()
        {
            var program = new Program();
            program.Run(); // Start the application
        }
    }
}