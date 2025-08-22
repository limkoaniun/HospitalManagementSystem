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

    public User? GetUserById(int id)
    {
        return users.FirstOrDefault(x => x.Id == id);
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

                    var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length < 5) continue;

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

                    // Format: Role,ID,Password,FullName,Email
                    user.Id = Convert.ToInt32(parts[1]);
                    user.Password = parts[2];
                    user.FullName = parts[3];
                    user.Email = parts[4];

                    users.Add(user);
                }
            }
        }
    }
}