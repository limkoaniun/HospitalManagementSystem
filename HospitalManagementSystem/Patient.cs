namespace HospitalManagementSystem;

public class Patient : User
{
    public Patient()
    {
        Role = "PATIENT";
    }
    
    public void ShowAppointments(AppointmentRepository appointmentRepository)
    {
        var myAppointments = appointmentRepository.GetAppointmentsByUserID(Id);
        Console.WriteLine("My Appointments:");
        foreach (var appt in myAppointments)
        {
            Console.WriteLine("PatientID: {0}, DoctorID: {1}, Symptoms: {2}",
                appt.PatientID, appt.DoctorID, appt.SymptomsDescription);
            Console.WriteLine("----------------------------------------");
        }
    }
    
    public void RenderPatientDetails()
    {
        Console.Clear();

        Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
        Console.WriteLine("│              DOTNET Hospital Management System              │");
        Console.WriteLine("├─────────────────────────────────────────────────────────────┤");
        Console.WriteLine("│                          My Details                         │");
        Console.WriteLine("└─────────────────────────────────────────────────────────────┘");
        Console.WriteLine();

        Console.WriteLine("{0}'s Details", FullName);
        Console.WriteLine();
        Console.WriteLine("Patient ID: {0}", Id);
        Console.WriteLine("Full Name: {0}", FullName);
        Console.WriteLine("Address: {0}", Address);
        Console.WriteLine("Email: {0}", Email);
        Console.WriteLine("Phone: {0}", Phone);
    }

    public override void Run(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        while (true)
        {
            Console.Clear();

            string welcome = $"Welcome to DOTNET Hospital Management System, {FullName}";

            // Header box
            Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│              DOTNET Hospital Management System              │");
            Console.WriteLine("├─────────────────────────────────────────────────────────────┤");
            Console.WriteLine("│                          Patient Menu                       │");
            Console.WriteLine("└─────────────────────────────────────────────────────────────┘");
            Console.WriteLine();

            Console.WriteLine(welcome);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List patient details");
            Console.WriteLine("2. List my doctor details");
            Console.WriteLine("3. List all appointments");
            Console.WriteLine("4. Book appointment");
            Console.WriteLine("5. Exit to login");
            Console.WriteLine("6. Exit system");
            Console.WriteLine();

            Console.Write("Enter choice: ");
            var choice = (Console.ReadLine() ?? string.Empty).Trim();

            switch (choice)
            {
                case "1":
                    RenderPatientDetails();
                    break;
                case "2":
                    Console.WriteLine("List my doctor details selected.");
                    break;
                case "3":
                    ShowAppointments(appointmentRepository);
                    break;
                case "4":
                    Console.WriteLine("Book appointment selected.");
                    break;
                case "5":
                    Console.WriteLine("Returning to login...");
                    return;
                case "6":
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