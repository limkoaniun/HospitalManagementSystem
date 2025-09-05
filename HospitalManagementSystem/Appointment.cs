namespace HospitalManagementSystem;

public class Appointment
{
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public string SymptomsDescription { get; set; } = string.Empty;
}