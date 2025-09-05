namespace HospitalManagementSystem;

public class Doctor : User
{
    public Doctor()
    {
        Role = "DOCTOR";
    }

    public override void Run(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        while (true)
        {
            Console.Clear();

            string welcome = $"Welcome to DOTNET Hospital Management System {FullName}";

            // Header box
            Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│              DOTNET Hospital Management System              │");
            Console.WriteLine("├─────────────────────────────────────────────────────────────┤");
            Console.WriteLine("│                          Doctor Menu                        │");
            Console.WriteLine("└─────────────────────────────────────────────────────────────┘");
            Console.WriteLine();

            Console.WriteLine(welcome);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List doctor details");
            Console.WriteLine("2. List patients");
            Console.WriteLine("3. List appointments");
            Console.WriteLine("4. Check particular patient");
            Console.WriteLine("5. List appointments with patient");
            Console.WriteLine("6. Logout");
            Console.WriteLine("7. Exit");
            Console.WriteLine();

            Console.Write("Enter your choice: ");
            var choice = (Console.ReadLine() ?? string.Empty).Trim();

            switch (choice)
            {
                case "1":
                    // Example: doctor details (could use DisplayDetails)
                    this.DisplayDetails();
                    break;

                case "2":
                    Console.WriteLine("List patients selected.");
                    break;

                case "3":
                    // Use appointmentRepository to show doctor’s appointments
                    var myAppointments = appointmentRepository.GetAppointmentsByUserID(Id);
                    Console.WriteLine("My Appointments:");
                    foreach (var appt in myAppointments)
                    {
                        Console.WriteLine($"PatientID: {appt.PatientID}, Symptoms: {appt.SymptomsDescription}");
                    }

                    break;

                case "4":
                    Console.WriteLine("Check particular patient selected.");
                    break;

                case "5":
                    Console.WriteLine("List appointments with patient selected.");
                    break;

                case "6":
                    Console.WriteLine("Logging out...");
                    return;

                case "7":
                    Console.WriteLine("Exiting system...");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}