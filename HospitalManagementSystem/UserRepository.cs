namespace HospitalManagementSystem;

using System.IO;

// generic interface implementation - UserRepository implements IRepository<User>
public class UserRepository : IRepository<User>
{
    private readonly List<User> users;

    private readonly string usersData;

    // constructor
    public UserRepository()
    {
        // Start from bin/Debug/net9.0
        var baseDir = AppContext.BaseDirectory;
        // Go 3 levels up to project root
        var projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"../../.."));
        // Build the path to appointment.txt
        usersData = Path.Combine(projectRoot, "user.txt");

        users = new List<User>();
        LoadUsers();
    }

    // IRepository<User> implementation
    public User? GetById(int id)
    {
        // delegate to your existing method
        return GetUserById(id);
    }

    public List<User> GetAll()
    {
        return users;
    }

    public void Add(User entity)
    {
        // Route to the correct concrete adder to keep file format consistent
        if (entity is Doctor d)
        {
            AddDoctor(d);
        }
        else if (entity is Patient p)
        {
            AddPatient(p);
        }
        else if (entity is Administrator a)
        {
            AddAdministrator(a);
        }
        else
        {
            // Fallback: treat as base User without a role-specific keyword
            users.Add(entity);
            AppendUserLine(entity);
        }
    }

    public void Remove(User entity)
    {
        // Remove from memory
        users.Remove(entity);

        // Persist by rewriting the entire file to keep it consistent
        SaveAllUsers();
    }

    public List<User> GetAllDoctors()
    {
        var result = new List<User>();
        foreach (var u in users)
        {
            if (u.Role == "DOCTOR")
                result.Add(u);
        }

        // anonymous method with delegate keyword for sorting
        result.Sort(delegate(User u1, User u2)
        {
            return string.Compare(u1.FullName, u2.FullName, StringComparison.OrdinalIgnoreCase);
        });

        return result;
    }

    public List<User> GetAllPatients()
    {
        var result = new List<User>();
        foreach (var u in users)
        {
            if (u.Role == "PATIENT")
                result.Add(u);
        }

        return result;
    }

    public User? GetUserById(int id)
    {
        foreach (var u in users)
        {
            if (u.Id == id)
                return u;
        }

        return null;
    }

    // Generate a unique numeric ID that is not already used
    public int GenerateNewId()
    {
        int id = 10000;
        while (true)
        {
            bool exists = false;
            foreach (var u in users)
            {
                if (u.Id == id)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists) return id;
            id++;
        }
    }

    // Add a new doctor: update memory and append to data file
    public void AddDoctor(Doctor doctor)
    {
        users.Add(doctor);
        AppendUserLine(doctor);
    }

    public void AddPatient(Patient patient)
    {
        users.Add(patient);
        AppendUserLine(patient);
    }

    // Optional helper to add admins consistently
    public void AddAdministrator(Administrator admin)
    {
        users.Add(admin);
        AppendUserLine(admin);
    }

    private void AppendUserLine(User user)
    {
        // ROLE|-|ID|-|Password|-|FullName|-|Email|-|Phone|-|Address
        using (var fs = new FileStream(usersData, FileMode.Append, FileAccess.Write))
        using (var writer = new StreamWriter(fs))
        {
            string role = user.Role?.ToUpperInvariant() ?? "USER";
            writer.WriteLine(
                $"{role}|-|{user.Id}|-|{user.Password}|-|{user.FullName}|-|{user.Email}|-|{user.Phone}|-|{user.Address}");
        }
    }

    private void SaveAllUsers()
    {
        // Rewrite the entire file from the in-memory list
        using (var fs = new FileStream(usersData, FileMode.Create, FileAccess.Write))
        using (var writer = new StreamWriter(fs))
        {
            writer.WriteLine("# Users Data File");
            writer.WriteLine("# Format: Role|-|ID|-|Password|-|FullName|-|Email|-|Phone|-|Address");

            foreach (var user in users)
            {
                string role = user.Role?.ToUpperInvariant() ?? "USER";
                writer.WriteLine(
                    $"{role}|-|{user.Id}|-|{user.Password}|-|{user.FullName}|-|{user.Email}|-|{user.Phone}|-|{user.Address}");
            }
        }
    }

    private void LoadUsers()
    {
        if (!File.Exists(usersData))
        {
            Console.WriteLine("User data file not found.");
            return;
        }

        using (FileStream fs = new FileStream(usersData, FileMode.Open, FileAccess.Read))
        using (StreamReader reader = new StreamReader(fs))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

                var parts = line.Split("|-|", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 7) continue;

                User user;

                var role = parts[0].ToUpperInvariant();

                if (role == "ADMIN")
                {
                    user = new Administrator();
                }
                else if (role == "DOCTOR")
                {
                    user = new Doctor();
                }
                else if (role == "PATIENT")
                {
                    user = new Patient();
                }
                else
                {
                    // Unknown role, default to base User
                    user = new Patient(); // or new User if it were not abstract
                }

                // Format: Role,ID,Password,FullName,Email,PhoneNumber,Address
                user.Id = Convert.ToInt32(parts[1]);
                user.Password = parts[2];
                user.FullName = parts[3];
                user.Email = parts[4];
                user.Phone = parts[5];
                user.Address = parts[6];

                users.Add(user);
            }
        }
    }
}