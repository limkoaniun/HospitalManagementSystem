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
    
    public void RenderPatientsDetails(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("My Patients");

        // Find all appointments where this doctor is involved
        var appts = appointmentRepository.GetAppointmentsByUserID(Id);

        // Collect unique patient IDs
        var patientIds = new HashSet<int>();
        foreach (var a in appts)
        {
            if (a.DoctorID == Id) // be explicit that these are your patients
                patientIds.Add(a.PatientID);
        }

        if (patientIds.Count == 0)
        {
            Console.WriteLine($"No patients are currently linked to {FullName}.");
            return;
        }

        Console.WriteLine($"Patients assigned to {FullName}:");
        Console.WriteLine();
        Console.WriteLine($"{"Patient",-20} | {"Doctor",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 120));

        // Print each patient on one line
        foreach (var pid in patientIds)
        {
            var patient = userRepository.GetUserById(pid);
            if (patient == null || patient.Role != "PATIENT") continue;

            string pName = patient.FullName ?? $"Patient#{pid}";
            string dName = this.FullName ?? $"Doctor#{Id}";
            string email = patient.Email ?? "";
            string phone = patient.Phone ?? "";
            string addr  = patient.Address ?? "";

            Console.WriteLine($"{pName,-20} | {dName,-20} | {email,-25} | {phone,-12} | {addr}");
        }
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
                    RenderPatientsDetails(userRepository, appointmentRepository);
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