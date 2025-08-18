namespace HospitalManagementSystem;

public abstract class User
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    // virtual method is optional to override
    public virtual void DisplayDetails()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Full Name: {FullName}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine($"Role: {Role}");
    }

    public abstract void LoadUserFromData(string[] data); // parent method must implement in child class
    public abstract void RenderUserMenu();

    public static User? CreateUserFromData(string[] parts) // normal mthods inherited as-is 
    {
        if (parts.Length < 5) return null;

        var role = parts[0].ToUpper();
        User? user = null;

        if (role == "ADMIN")
            user = new Administrator();
        else if (role == "DOCTOR")
            user = new Doctor();
        else if (role == "PATIENT")
            user = new Patient();

        if (user != null) user.LoadUserFromData(parts);

        return user;
    }

    public static User? LoginWithUserId(List<User> users, string idInput, string password)
    {
        // finding if the user exists in data.txt
        foreach (var user in users)
            if (user.Id.ToString() == idInput && user.Password == password)
                return user;

        return null;
    }
}