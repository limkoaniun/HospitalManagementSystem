namespace HospitalManagementSystem;

public class Doctor : User
{
    public Doctor()
    {
        Role = "DOCTOR";
    }
    
    public void RenderDoctorDetails()
    {
        Console.Clear();

        Ui.RenderHeader("My Details");

        // Table header
        Console.WriteLine($"{"Name",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 90));

        // Row for this doctor
        string name = FullName ?? $"Doctor#{Id}";
        string email = Email ?? "";
        string phone = Phone ?? "";
        string address = Address ?? "";

        Console.WriteLine($"{name,-20} | {email,-25} | {phone,-12} | {address}");
    }


    public override void Run(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        while (true)
        {
            Console.Clear();

            string welcome = $"Welcome to DOTNET Hospital Management System, {FullName}";

            // Header box
            Ui.RenderHeader("Doctor Menu");

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
                    RenderDoctorDetails();
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