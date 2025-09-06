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