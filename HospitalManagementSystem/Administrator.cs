namespace HospitalManagementSystem;

public class Administrator : User
{
    public Administrator()
    {
        Role = "ADMIN";
    }
    
    private void RenderAllDoctorsDetails(UserRepository userRepository)
    {
        Console.Clear();
        Ui.RenderHeader("All Doctors");

        var doctors = userRepository.GetAllDoctors();

        if (doctors.Count == 0)
        {
            Console.WriteLine("No doctors registered in the system.");
            return;
        }

        Console.WriteLine("All doctors registered to the DOTNET Hospital Management System");
        Console.WriteLine();
        Console.WriteLine($"{"Name",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 90));

        foreach (var doc in doctors)
        {
            string name = doc.FullName ?? $"Doctor#{doc.Id}";
            string email = doc.Email ?? "";
            string phone = doc.Phone ?? "";
            string address = doc.Address ?? "";
            Console.WriteLine($"{name,-20} | {email,-25} | {phone,-12} | {address}");
        }
    }
    
    private void RenderDoctorDetailsWithId(UserRepository userRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Doctor Details");

        Console.Write("Please enter the ID of the doctor whose details you are checking (or press n to return): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || input.Trim().ToLower() == "n")
            return;

        if (!int.TryParse(input, out int docId))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var doctor = userRepository.GetUserById(docId);

        if (doctor == null || doctor.Role != "DOCTOR")
        {
            Console.WriteLine("No doctor found with that ID.");
            return;
        }

        Console.WriteLine($"Details for {doctor.FullName}");
        Console.WriteLine();
        Console.WriteLine($"{"Name",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 90));

        string name = doctor.FullName ?? $"Doctor#{doctor.Id}";
        string email = doctor.Email ?? "";
        string phone = doctor.Phone ?? "";
        string address = doctor.Address ?? "";

        Console.WriteLine($"{name,-20} | {email,-25} | {phone,-12} | {address}");
    }
    
    private void RenderAllPatientsDetails(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("All Patients");

        var patients = userRepository.GetAllPatients();
        if (patients.Count == 0)
        {
            Console.WriteLine("No patients registered in the system.");
            return;
        }

        Console.WriteLine("All patients registered to the DOTNET Hospital Management System");
        Console.WriteLine();
        Console.WriteLine($"{"Patient",-20} | {"Doctor",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 120));

        foreach (var patient in patients)
        {
            // find the first linked doctor for this patient (if any)
            var appts = appointmentRepository.GetAppointmentsByUserID(patient.Id);
            int? doctorId = null;
            foreach (var a in appts)
            {
                if (a.PatientID == patient.Id)
                {
                    doctorId = a.DoctorID;
                    break; // first one is enough
                }
            }

            var doctor = doctorId.HasValue ? userRepository.GetUserById(doctorId.Value) : null;

            string patientName = patient.FullName ?? $"Patient#{patient.Id}";
            string doctorName  = (doctor != null && doctor.Role == "DOCTOR")
                ? doctor.FullName
                : "Unassigned";
            string email = patient.Email ?? "";
            string phone = patient.Phone ?? "";
            string addr  = patient.Address ?? "";

            Console.WriteLine($"{patientName,-20} | {doctorName,-20} | {email,-25} | {phone,-12} | {addr}");
        }
    }

    private void RenderPatientDetailsWithId(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Patient Details");

        Console.Write("Please enter the ID of the patient whose details you are checking (or press n to return): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || input.Trim().ToLower() == "n")
            return;

        if (!int.TryParse(input, out int patientId))
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

        // find a linked doctor through appointments, if any
        int? doctorId = null;
        var appts = appointmentRepository.GetAppointmentsByUserID(patientId);
        foreach (var a in appts)
        {
            if (a.PatientID == patientId)
            {
                doctorId = a.DoctorID;
                break;
            }
        }
        var doctor = doctorId.HasValue ? userRepository.GetUserById(doctorId.Value) : null;

        Console.WriteLine($"Details for {patient.FullName}");
        Console.WriteLine();
        Console.WriteLine($"{"Patient",-20} | {"Doctor",-20} | {"Email Address",-25} | {"Phone",-12} | Address");
        Console.WriteLine(new string('-', 120));

        string patientName = patient.FullName ?? $"Patient#{patient.Id}";
        string doctorName  = (doctor != null && doctor.Role == "DOCTOR") ? doctor.FullName : "Unassigned";
        string email = patient.Email ?? "";
        string phone = patient.Phone ?? "";
        string addr  = patient.Address ?? "";

        Console.WriteLine($"{patientName,-20} | {doctorName,-20} | {email,-25} | {phone,-12} | {addr}");
    }



    public override void Run(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        while (true)
        {
            Console.Clear();

            string welcome = $"Welcome to DOTNET Hospital Management System, {FullName}";

            // Header box
            Ui.RenderHeader("Administrator Menu");

            Console.WriteLine(welcome);
            Console.WriteLine();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. List all doctors");
            Console.WriteLine("2. Check doctor details");
            Console.WriteLine("3. List all patients");
            Console.WriteLine("4. Check patient details");
            Console.WriteLine("5. Add doctor");
            Console.WriteLine("6. Add patient");
            Console.WriteLine("7. Logout");
            Console.WriteLine("8. Exit");
            Console.WriteLine();

            Console.Write("Enter your choice: ");
            var input = (Console.ReadLine() ?? string.Empty).Trim();

            switch (input)
            {
                case "1":
                    RenderAllDoctorsDetails(userRepository);
                    break;

                case "2":
                    RenderDoctorDetailsWithId(userRepository);
                    break;

                case "3":
                    RenderAllPatientsDetails(userRepository, appointmentRepository);
                    break;

                case "4":
                    RenderPatientDetailsWithId(userRepository, appointmentRepository);
                    break;

                case "5":
                    Console.WriteLine("Add doctor selected. (Not implemented)");
                    break;

                case "6":
                    Console.WriteLine("Add patient selected. (Not implemented)");
                    break;

                case "7":
                    Console.WriteLine("Logging out...");
                    return; // back to login

                case "8":
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