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
                    Console.Write("Enter Doctor ID: ");
                    if (int.TryParse(Console.ReadLine(), out int docId))
                    {
                        var doctor = userRepository.GetUserById(docId);
                        if (doctor != null && doctor.Role == "DOCTOR")
                            doctor.DisplayDetails();
                        else
                            Console.WriteLine("Doctor not found.");
                    }

                    break;

                case "3":
                    // TODO: implement GetAllPatients() in UserRepository
                    // var patients = userRepository.GetAllPatients();
                    // Console.WriteLine("Patients:");
                    // foreach (var pat in patients)
                    // {
                    //     Console.WriteLine($"{pat.Id} - {pat.FullName} ({pat.Email})");
                    // }
                    Console.WriteLine("List all patients (not yet implemented).");
                    break;

                case "4":
                    Console.Write("Enter Patient ID: ");
                    if (int.TryParse(Console.ReadLine(), out int patId))
                    {
                        var patient = userRepository.GetUserById(patId);
                        if (patient != null && patient.Role == "PATIENT")
                            patient.DisplayDetails();
                        else
                            Console.WriteLine("Patient not found.");
                    }

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