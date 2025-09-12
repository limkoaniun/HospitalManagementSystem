namespace HospitalManagementSystem
{
    public class AppointmentRepository : IRepository<Appointment>
    {
        private readonly List<Appointment> appointments;

        private readonly string appointmentData =
            "/Users/koanlin/RiderProjects/HospitalManagementSystem/HospitalManagementSystem/appointment.txt";

        public AppointmentRepository()
        {
            appointments = new List<Appointment>();
            LoadAppointments();
        }

        // IRepository<Appointment> implementation
        public Appointment? GetById(int id)
        {
            // Not supported because Appointment has no unique Id field.
            // Keep this to satisfy the generic interface for the assignment.
            throw new NotSupportedException("Appointment does not have a unique Id.");
        }

        public List<Appointment> GetAll()
        {
            return appointments;
        }

        public void Add(Appointment entity)
        {
            AddAppointment(entity);
        }

        public void Remove(Appointment entity)
        {
            // Remove in memory
            appointments.Remove(entity);
            // Rewrite file to persist the removal
            SaveAllAppointments();
        }

        // method overloading
        public void AddAppointment(Appointment appointment)
        {
            // 1) append in memory
            appointments.Add(appointment);

            // 2) append to file in the exact format your loader expects:
            // PatientID|-|DoctorID|-|SymptomDescription
            using (var sw = new StreamWriter(appointmentData, append: true))
            {
                sw.WriteLine($"{appointment.PatientID}|-|{appointment.DoctorID}|-|{appointment.SymptomsDescription}");
            }

            Console.WriteLine("The appointment has been booked successfully");
        }

        public void AddAppointment(int patientId, int doctorId, string description)
        {
            AddAppointment(new Appointment
            {
                PatientID = patientId,
                DoctorID = doctorId,
                SymptomsDescription = description
            });
        }

        public List<Appointment> GetAppointmentsByUserID(int id)
        {
            var result = new List<Appointment>();
            foreach (var appt in appointments)
            {
                if (appt.DoctorID == id || appt.PatientID == id)
                {
                    result.Add(appt);
                }
            }

            return result;
        }

        private void LoadAppointments()
        {
            if (!File.Exists(appointmentData))
            {
                Console.WriteLine("Appointment data file not found.");
                return;
            }

            using (FileStream fs = new FileStream(appointmentData, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fs))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    var parts = line.Split("|-|", StringSplitOptions.None);
                    if (parts.Length != 3)
                        continue;

                    var appointment = new Appointment
                    {
                        PatientID = Convert.ToInt32(parts[0]),
                        DoctorID = Convert.ToInt32(parts[1]),
                        SymptomsDescription = parts[2].Trim()
                    };

                    appointments.Add(appointment);
                }
            }
        }

        private void SaveAllAppointments()
        {
            // Rewrite the entire file from the in-memory list
            using (var fs = new FileStream(appointmentData, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(fs))
            {
                // Keep header if you like. If your file does not have it, remove these two lines.
                writer.WriteLine("# Patient-to-Doctor Appointments");
                writer.WriteLine("# Format: PatientID|-|DoctorID|-|SymptomDescription");

                foreach (var appt in appointments)
                {
                    writer.WriteLine($"{appt.PatientID}|-|{appt.DoctorID}|-|{appt.SymptomsDescription}");
                }
            }
        }
    }
}