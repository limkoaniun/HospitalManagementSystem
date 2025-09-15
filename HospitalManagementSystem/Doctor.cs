namespace HospitalManagementSystem;

// Doctor class represents medical professionals in the hospital system
// Inheritance - Doctor inherits from the abstract User base class
public class Doctor : User
{
    // Constructor sets the role to DOCTOR when a new doctor object is created
    public Doctor()
    {
        Role = "DOCTOR";
    }

    // Displays the current doctor's personal information in a formatted table
    // Shows name, email, phone, and address details
    public void RenderDoctorDetails()
    {
        Console.Clear();

        Ui.RenderHeader("My Details");

        // Display table header with column names
        Console.WriteLine($"{"Name",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 90));

        // Format doctor details with null-safe fallbacks
        string name = FullName ?? $"Doctor#{Id}"; // Use ID if name is null
        string email = Email ?? ""; // Empty string if null
        string phone = Phone ?? ""; // Empty string if null
        string address = Address ?? ""; // Empty string if null

        Console.WriteLine($"{name,-20} | {email,-25} | {phone,-12} | {address}");
    }

    // Displays all patients assigned to this doctor through appointments
    // Parameters: repositories for accessing user and appointment data
    public void RenderPatientsDetails(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("My Patients");

        // Get all appointments for this doctor
        var appts = appointmentRepository.GetAppointmentsByUserID(Id);

        // Extract unique patient IDs from appointments
        // Using HashSet to automatically handle duplicates
        var patientIds = new HashSet<int>();
        foreach (var a in appts)
        {
            // Only add patients where this doctor is the assigned doctor
            if (a.DoctorID == Id)
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

        // Display each patient's information in table format
        foreach (var pid in patientIds)
        {
            // Retrieve patient details from repository
            var patient = userRepository.GetUserById(pid);

            // Skip if user not found or not a patient
            if (patient == null || patient.Role != "PATIENT") continue;

            string pName = patient.FullName ?? $"Patient#{pid}";
            string dName = this.FullName ?? $"Doctor#{Id}";
            string email = patient.Email ?? "";
            string phone = patient.Phone ?? "";
            string addr = patient.Address ?? "";

            Console.WriteLine($"{pName,-20} | {dName,-20} | {email,-25} | {phone,-12} | {addr}");
        }
    }

    // Displays all appointments for this doctor
    // Shows doctor name, patient name, and symptom description for each appointment
    public void RenderAppointments(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("All Appointments");

        // Table header
        Console.WriteLine($"{"Doctor",-20} | {"Patient",-20} | Description");
        Console.WriteLine(new string('-', 70));

        // Get all appointments involving this doctor
        var appts = appointmentRepository.GetAppointmentsByUserID(Id);

        // Filter and display only appointments where this user is the doctor
        var hasAny = false;
        foreach (var a in appts)
        {
            // Skip appointments where this doctor is not the assigned doctor
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


    // Allows doctor to view detailed information about a specific patient
    // Prompts for patient ID and displays their contact details and assigned doctor
    public void RenderCheckPatient(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Check Patient Details");

        Console.Write("Enter the ID of the patient to check: ");
        string? input = Console.ReadLine();

        int patientId;
        // Validate and parse the patient ID input
        // Exception handling for invalid ID format (non-numeric input)
        try
        {
            patientId = Convert.ToInt32(input);
        }
        catch
        {
            Console.WriteLine("Invalid ID format.");
            return; // Exit if ID is invalid
        }

        // Verify the patient exists in the system
        var patient = userRepository.GetUserById(patientId);
        if (patient == null || patient.Role != "PATIENT")
        {
            Console.WriteLine("No patient found with that ID.");
            return;
        }

        // Find the patient's assigned doctor through appointment records
        string assignedDoctorName = "Unassigned"; // Default if no doctor assigned
        var appts = appointmentRepository.GetAppointmentsByUserID(patientId);

        // Look for the first appointment to determine assigned doctor
        foreach (var a in appts)
        {
            if (a.PatientID == patientId)
            {
                // Retrieve doctor information
                var d = userRepository.GetUserById(a.DoctorID);
                if (d != null && d.Role == "DOCTOR")
                    assignedDoctorName = d.FullName ?? $"Doctor#{a.DoctorID}";
                break; // First match is enough - assuming one doctor per patient
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

    // Displays appointments between this doctor and a specific patient
    // Prompts for patient ID and shows all appointments with that patient
    public void RenderAppointmentsWithPatient(UserRepository userRepository,
        AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Appointments With");

        Console.Write("Enter the ID of the patient you would like to view appointments for: ");
        string? input = Console.ReadLine();

        int patientId;
        // Validate and parse the patient ID input
        // Exception handling for invalid ID format (non-numeric input)
        try
        {
            patientId = Convert.ToInt32(input);
        }
        catch
        {
            Console.WriteLine("Invalid ID format.");
            return; // Exit if ID is invalid
        }

        // Verify patient exists before proceeding
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

        // Filter and display appointments between this doctor and the specified patient
        bool any = false;
        foreach (var a in appts)
        {
            // Only show appointments where both doctor and patient match
            if (a.DoctorID != this.Id || a.PatientID != patientId) continue;

            any = true;

            // Format names with fallback values
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

    // Main menu system for doctor users
    // Method overriding - overrides abstract Run method from User class
    // Provides doctor-specific functionality and menu options
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

            // Process user's menu choice
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
                    // Logout functionality - returns to login screen
                    Console.WriteLine("Logging out...");
                    return;

                case "7":
                    // Exit functionality - terminates the entire application
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