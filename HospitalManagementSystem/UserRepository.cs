namespace HospitalManagementSystem;

using System.IO;

public class UserRepository
{
    private readonly List<User> users;

    private readonly string usersData =
        "/Users/koanlin/RiderProjects/HospitalManagementSystem/HospitalManagementSystem/data.txt";

    public UserRepository()
    {
        users = new List<User>();
        LoadUsers();
    }

    public List<User> GetAllDoctors()
    {
        var result = new List<User>();
        foreach (var u in users)
        {
            if (u.Role == "DOCTOR")
                result.Add(u);
        }

        return result;
    }

    public User? GetDoctorById(int id)
    {
        var u = GetUserById(id);
        if (u != null && u.Role == "DOCTOR") return u;
        return null;
    }

    public User? GetUserById(int id)
    {
        foreach (var user in users)
        {
            if (user.Id == id)
            {
                return user;
            }
        }

        return null; // no match found
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

    // Generate a unique numeric ID that is not already used
    public int GenerateNewId()
    {
        // id start at 
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
        // keep in-memory list current
        users.Add(doctor);

        // persist to the same file format your loader expects:
        // DOCTOR|-|ID|-|Password|-|FullName|-|Email|-|Phone|-|Address
        using (var fs = new FileStream(usersData, FileMode.Append, FileAccess.Write))
        using (var writer = new StreamWriter(fs))
        {
            writer.WriteLine(
                $"DOCTOR|-|{doctor.Id}|-|{doctor.Password}|-|{doctor.FullName}|-|{doctor.Email}|-|{doctor.Phone}|-|{doctor.Address}");
        }
    }

    public void AddPatient(Patient patient)
    {
        // keep in-memory list current
        users.Add(patient);

        // Persist in the same format your loader expects
        // PATIENT|-|ID|-|Password|-|FullName|-|Email|-|Phone|-|Address
        using (var fs = new FileStream(usersData, FileMode.Append, FileAccess.Write))
        using (var writer = new StreamWriter(fs))
        {
            writer.WriteLine(
                $"PATIENT|-|{patient.Id}|-|{patient.Password}|-|{patient.FullName}|-|{patient.Email}|-|{patient.Phone}|-|{patient.Address}");
        }
    }

    public User? GetUserByCredentials(int id, string? password)
    {
        foreach (var u in users)
        {
            if (u.Id == id && u.Password == password)
                return u;
        }

        return null;
    }


    private void LoadUsers()
    {
        if (!File.Exists(usersData))
        {
            Console.WriteLine("User data file not found.");
            return;
        }

        // Use FileStream to read file
        using (FileStream fs = new FileStream(usersData, FileMode.Open, FileAccess.Read))
        {
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
                    else
                    {
                        user = new Patient();
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
}