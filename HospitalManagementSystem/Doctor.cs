namespace HospitalManagementSystem;
// inherited from User class
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
            string addr = patient.Address ?? "";

            Console.WriteLine($"{pName,-20} | {dName,-20} | {email,-25} | {phone,-12} | {addr}");
        }
    }

    public void RenderAppointments(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("All Appointments");

        // Table header
        Console.WriteLine($"{"Doctor",-20} | {"Patient",-20} | Description");
        Console.WriteLine(new string('-', 70));

        // Get only this doctor's appointments
        var appts = appointmentRepository.GetAppointmentsByUserID(Id);

        // Keep only rows where this doctor is the doctor
        var hasAny = false;
        foreach (var a in appts)
        {
            if (a.DoctorID != Id) continue;

            hasAny = true;

            var doctor = this; // current user
            var patient = userRepository.GetUserById(a.PatientID);

            string doctorName = doctor?.FullName ?? $"Doctor#{Id}";
            string patientName = patient?.FullName ?? $"Patient#{a.PatientID}";
            string desc = a.SymptomsDescription ?? "";

            Console.WriteLine($"{doctorName,-20} | {patientName,-20} | {desc}");
        }

        if (!hasAny)
        {
            Console.WriteLine("No appointments found.");
        }
    }


    public void RenderCheckPatient(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Check Patient Details");

        Console.Write("Enter the ID of the patient to check: ");
        string? input = Console.ReadLine();

        int patientId;
        try
        {
            patientId = Convert.ToInt32(input);
        }
        catch
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var patient = userRepository.GetUserById(patientId);
        if (patient == null || patient.Role != "PATIENT")
        {
            Console.WriteLine("No patient found with that ID.");
            return;
        }

        string assignedDoctorName = "Unassigned";
        var appts = appointmentRepository.GetAppointmentsByUserID(patientId);
        foreach (var a in appts)
        {
            if (a.PatientID == patientId)
            {
                var d = userRepository.GetUserById(a.DoctorID);
                if (d != null && d.Role == "DOCTOR")
                    assignedDoctorName = d.FullName ?? $"Doctor#{a.DoctorID}";
                break; // first match is enough
            }
        }

        Console.WriteLine();
        Console.WriteLine($"{"Patient",-20} | {"Doctor",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 120));

        string pName = patient.FullName ?? $"Patient#{patient.Id}";
        string email = patient.Email ?? "";
        string phone = patient.Phone ?? "";
        string addr = patient.Address ?? "";

        Console.WriteLine($"{pName,-20} | {assignedDoctorName,-20} | {email,-25} | {phone,-12} | {addr}");
    }

    public void RenderAppointmentsWithPatient(UserRepository userRepository,
        AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Appointments With");

        Console.Write("Enter the ID of the patient you would like to view appointments for: ");
        string? input = Console.ReadLine();

        int patientId;
        try
        {
            patientId = Convert.ToInt32(input);
        }
        catch
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var patient = userRepository.GetUserById(patientId);
        if (patient == null || patient.Role != "PATIENT")
        {
            Console.WriteLine("No patient found with that ID.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"{"Doctor",-20} | {"Patient",-20} | Description");
        Console.WriteLine(new string('-', 70));

        var appts = appointmentRepository.GetAppointmentsByUserID(patientId);

        bool any = false;
        foreach (var a in appts)
        {
            if (a.DoctorID != this.Id || a.PatientID != patientId) continue;

            any = true;
            string doctorName = this.FullName ?? $"Doctor#{this.Id}";
            string patientName = patient.FullName ?? $"Patient#{patientId}";
            string desc = a.SymptomsDescription ?? "";

            Console.WriteLine($"{doctorName,-20} | {patientName,-20} | {desc}");
        }

        if (!any)
        {
            Console.WriteLine("No appointments found with that patient.");
        }
    }

    // abstract method: run the menu for this user type
    // method override in subclasses
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
                    RenderAppointments(userRepository, appointmentRepository);
                    break;

                case "4":
                    RenderCheckPatient(userRepository, appointmentRepository);
                    break;

                case "5":
                    RenderAppointmentsWithPatient(userRepository, appointmentRepository);
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