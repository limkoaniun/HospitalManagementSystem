namespace HospitalManagementSystem;

// Abstract base class representing a user in the hospital system
// This demonstrates inheritance - Doctor, Patient, and Administrator classes inherit from User
// Cannot be instantiated directly - must use one of the derived classes
public abstract class User
{
    // User properties that are common to all user types
    public int Id { get; set; } // Unique identifier for the user
    public string Password { get; set; } // Login password (stored in plain text for simplicity)
    public string FullName { get; set; } // User's full name for display
    public string Email { get; set; } // Contact email address
    public string Role { get; set; } // User role: DOCTOR, PATIENT, or ADMIN
    public string Phone { get; set; } // Contact phone number
    public string Address { get; set; } // Physical address

    // Abstract method that must be implemented by each derived class
    // This is an example of method overriding - each user type has its own menu system
    // Parameters: repositories needed to access user and appointment data
    public abstract void Run(UserRepository userRepository, AppointmentRepository appointmentRepository);
}