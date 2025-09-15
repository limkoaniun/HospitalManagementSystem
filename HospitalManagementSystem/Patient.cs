namespace HospitalManagementSystem;

// Patient class represents patients in the hospital system
// Inheritance - Patient inherits from the abstract User base class
public class Patient : User
{
    // Constructor sets the role to PATIENT when a new patient object is created
    public Patient()
    {
        Role = "PATIENT";
    }

    // Handles the appointment booking process for patients
    // Allows new patients to choose a doctor or existing patients to book with their assigned doctor
    // Creates appointment object and writes to text file
    public void RenderBookAppointment(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Book Appointment");

        // Check if patient already has appointments (and thus an assigned doctor)
        var existingAppointments = appointmentRepository.GetAppointmentsByUserID(Id);
        User selectedDoctor = null;
        var availableDoctors = userRepository.GetAllDoctors();
        int doctorIndex = 1;

        // New patient - needs to select a doctor
        if (existingAppointments.Count == 0)
        {
            Console.WriteLine(
                "You are not registered with any doctor! Please choose which doctor you would like to register with");

            // Display list of available doctors with their details
            foreach (var doctor in availableDoctors)
            {
                Console.WriteLine(
                    $"{doctorIndex} {doctor.FullName} | {doctor.Email} | {doctor.Phone} | {doctor.Address}");
                doctorIndex++;
            }

            // Prompt for doctor selection and symptom description
            Console.WriteLine();
            Console.Write("Please choose a doctor (idx): ");
            var chosenDoctorIndex = Console.ReadLine();
            Console.Write("Description of symptoms: ");
            string symptomDescription = Console.ReadLine();

            // Create appointment with chosen doctor
            var chosenDoctorId = availableDoctors[Convert.ToInt32(chosenDoctorIndex) - 1].Id;
            appointmentRepository.AddAppointment(this.Id, chosenDoctorId, symptomDescription);
        }
        else
        {
            // Existing patient - book with their assigned doctor
            var linkedDoctorId = existingAppointments[0].DoctorID;
            selectedDoctor = userRepository.GetUserById(linkedDoctorId);

            // Display current doctor and get symptom description
            Console.WriteLine($"You are booking a new appointment with {selectedDoctor.FullName}.");
            Console.Write("Description of symptoms: ");
            string symptomDescription = Console.ReadLine();

            // Create appointment with existing doctor
            appointmentRepository.AddAppointment(this.Id, selectedDoctor.Id, symptomDescription);
        }

        Console.ReadKey(true);
    }

    // Displays all appointments for this patient
    // Shows doctor name, patient name, and symptom description for each appointment
    public void RenderAppointments(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("My Appointments");

        Console.WriteLine($"Appointments for {FullName}");
        Console.WriteLine();

        // Display table header with column names
        Console.WriteLine($"{"Doctor",-20} | {"Patient",-20} | Description");
        Console.WriteLine(new string('-', 70));

        // Get all appointments for this patient
        var myAppointments = appointmentRepository.GetAppointmentsByUserID(Id);

        // Handle case when patient has no appointments
        if (myAppointments.Count == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        // Display each appointment in table format
        foreach (var appt in myAppointments)
        {
            // Retrieve doctor and patient details
            var doctor = userRepository.GetUserById(appt.DoctorID);
            var patient = userRepository.GetUserById(appt.PatientID);

            // Format names with fallback values if data is missing
            string doctorName = doctor?.FullName ?? $"Doctor#{appt.DoctorID}";
            string patientName = patient?.FullName ?? $"Patient#{appt.PatientID}";

            Console.WriteLine($"{doctorName,-20} | {patientName,-20} | {appt.SymptomsDescription}");
        }
    }

    // Displays details of all doctors assigned to this patient
    // Finds doctors through appointment records
    public void RenderDoctorsDetails(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("My Doctor");

        Console.WriteLine("Your doctor:");
        Console.WriteLine();

        // Get all appointments to find linked doctors
        var myAppointments = appointmentRepository.GetAppointmentsByUserID(Id);
        if (myAppointments.Count == 0)
        {
            Console.WriteLine("No appointments found, so no linked doctor to display.");
            return;
        }

        // Extract unique doctor IDs from appointments
        // Using HashSet to automatically handle duplicates
        var uniqueDoctorIds = new HashSet<int>();
        foreach (var appt in myAppointments)
        {
            uniqueDoctorIds.Add(appt.DoctorID);
        }

        // Retrieve doctor information for each unique ID
        var doctors = new List<User>();
        foreach (var docId in uniqueDoctorIds)
        {
            var user = userRepository.GetUserById(docId);
            // Verify user exists and has doctor role
            if (user != null && user.Role == "DOCTOR")
            {
                doctors.Add(user);
            }
        }

        if (doctors.Count == 0)
        {
            Console.WriteLine("No doctor records found for your appointments.");
            return;
        }

        // Table header
        Console.WriteLine($"{"Name",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 90));

        // Display each doctor's information in table format
        foreach (var doc in doctors)
        {
            // Format doctor details with null-safe fallbacks
            string name = doc.FullName ?? $"Doctor#{doc.Id}";
            string email = doc.Email ?? "";
            string phone = doc.Phone ?? "";
            string address = doc.Address ?? "";
            Console.WriteLine($"{name,-20} | {email,-25} | {phone,-12} | {address}");
        }
    }

    // Displays the current patient's personal information
    // Shows ID, name, address, email, and phone details
    public void RenderPatientDetails()
    {
        Console.Clear();
        Ui.RenderHeader("My Details");


        Console.WriteLine("{0}'s Details", FullName);
        Console.WriteLine();
        Console.WriteLine("Patient ID: {0}", Id);
        Console.WriteLine("Full Name: {0}", FullName);
        Console.WriteLine("Address: {0}", Address);
        Console.WriteLine("Email: {0}", Email);
        Console.WriteLine("Phone: {0}", Phone);
    }

    // Main menu system for patient users
    // Method overriding - overrides abstract Run method from User class
    // Provides patient-specific functionality and menu options
    public override void Run(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        while (true)
        {
            Console.Clear();

            string welcome = $"Welcome to DOTNET Hospital Management System, {FullName}";

            // Header box
            Ui.RenderHeader("Patient Menu");

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

            // Process user's menu choice
            switch (choice)
            {
                case "1":
                    RenderPatientDetails();
                    break;
                case "2":
                    RenderDoctorsDetails(userRepository, appointmentRepository);
                    break;
                case "3":
                    RenderAppointments(userRepository, appointmentRepository);
                    break;
                case "4":
                    RenderBookAppointment(userRepository, appointmentRepository);
                    break;
                case "5":
                    // Logout functionality - returns to login screen
                    Console.WriteLine("Returning to login...");
                    return;
                case "6":
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