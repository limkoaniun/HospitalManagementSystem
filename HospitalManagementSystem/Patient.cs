using System.Collections;

namespace HospitalManagementSystem;

public class Patient : User
{
    public Patient()
    {
        Role = "PATIENT";
    }

    public void RenderBookAppointment(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("Book Appointment");

        var appointments = appointmentRepository.GetAppointmentsByUserID(Id);
        User theDoctor = null;
        var allDoctors = userRepository.GetAllDoctors();
        int docIdx = 1;
        if (appointments.Count == 0)
        {
            Console.WriteLine(
                "You are not registered with any doctor! Please choose which doctor you would like to register with");

            // render doctor list
            foreach (var doc in allDoctors)
            {
                Console.WriteLine($"{docIdx} {doc.FullName} | {doc.Email} | {doc.Phone} | {doc.Address}");
                docIdx++;
            }

            // Choose a doctor
            Console.WriteLine();
            Console.Write("Please choose a doctor (idx): ");
            var doctorIdx = Console.ReadLine();
            Console.Write("Description of symptoms: ");
            string symptomDescription = Console.ReadLine();
            var doctorId = allDoctors[Convert.ToInt32(doctorIdx) - 1].Id;
            appointmentRepository.AddAppointment(this.Id, Convert.ToInt32(doctorId), symptomDescription);
        }
        else
        {
            var doctorId = appointments[0].DoctorID;
            theDoctor = userRepository.GetUserById(doctorId);
            Console.WriteLine($"You are booking a new appointment with {theDoctor.FullName}.");
            Console.Write("Description of symptoms: ");
            string symptomDescription = Console.ReadLine();
            appointmentRepository.AddAppointment(this.Id, theDoctor.Id, symptomDescription);
        }

        Console.ReadKey(true);
    }

    public void RenderAppointments(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("My Appointments");

        Console.WriteLine($"Appointments for {FullName}");
        Console.WriteLine();

        // Table header
        Console.WriteLine($"{"Doctor",-20} | {"Patient",-20} | Description");
        Console.WriteLine(new string('-', 70));

        var myAppointments = appointmentRepository.GetAppointmentsByUserID(Id);

        if (myAppointments.Count == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        foreach (var appt in myAppointments)
        {
            var doctor = userRepository.GetUserById(appt.DoctorID);
            var patient = userRepository.GetUserById(appt.PatientID);

            string doctorName = doctor?.FullName ?? $"Doctor#{appt.DoctorID}";
            string patientName = patient?.FullName ?? $"Patient#{appt.PatientID}";

            Console.WriteLine($"{doctorName,-20} | {patientName,-20} | {appt.SymptomsDescription}");
        }
    }

    public void RenderDoctorsDetails(UserRepository userRepository, AppointmentRepository appointmentRepository)
    {
        Console.Clear();
        Ui.RenderHeader("My Doctor");

        Console.WriteLine("Your doctor:");
        Console.WriteLine();

        // Get all my appointments
        var myAppointments = appointmentRepository.GetAppointmentsByUserID(Id);
        if (myAppointments.Count == 0)
        {
            Console.WriteLine("No appointments found, so no linked doctor to display.");
            return;
        }

        // Collect unique doctor IDs from my appointments
        var uniqueDoctorIds = new HashSet<int>();
        foreach (var appt in myAppointments)
        {
            uniqueDoctorIds.Add(appt.DoctorID);
        }

        // Look up doctor users
        var doctors = new List<User>();
        foreach (var docId in uniqueDoctorIds)
        {
            var user = userRepository.GetUserById(docId);
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

        // Rows
        foreach (var doc in doctors)
        {
            string name = doc.FullName ?? $"Doctor#{doc.Id}";
            string email = doc.Email ?? "";
            string phone = doc.Phone ?? "";
            string address = doc.Address ?? "";
            Console.WriteLine($"{name,-20} | {email,-25} | {phone,-12} | {address}");
        }
    }

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