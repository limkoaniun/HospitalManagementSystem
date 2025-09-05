namespace HospitalManagementSystem;

public abstract class User
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }

    public virtual void DisplayDetails() // virtual is optional to override
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Full Name: {FullName}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Role: {Role}");
    }

    public abstract void Run(UserRepository userRepository, AppointmentRepository appointmentRepository);
}