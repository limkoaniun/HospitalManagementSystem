using System;
using System.Collections.Generic;
using System.IO;

namespace HospitalManagementSystem
{
    public class AppointmentRepository
    {
        private List<Appointment> appointments;

        private readonly string appointmentData =
            "/Users/koanlin/RiderProjects/HospitalManagementSystem/HospitalManagementSystem/appointment.txt";

        public AppointmentRepository()
        {
            appointments = new List<Appointment>();
            LoadAppointments();
        }
    
        public List<Appointment> GetAppointmentsByUserID(int id)
        {
            List<Appointment> result = new List<Appointment>();
            foreach (Appointment appt in appointments)
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

                    Appointment appointment = new Appointment
                    {
                        PatientID = Convert.ToInt32(parts[0]),
                        DoctorID = Convert.ToInt32(parts[1]),
                        SymptomsDescription = parts[2].Trim()
                    };

                    appointments.Add(appointment);
                }
            }
        }
    }
}