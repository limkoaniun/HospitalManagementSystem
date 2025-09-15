namespace HospitalManagementSystem;

internal static class Ui
{
    public static void RenderHeader(string title)
    {
        Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
        Console.WriteLine("│              DOTNET Hospital Management System              │");
        Console.WriteLine("├─────────────────────────────────────────────────────────────┤");

        // Center the title in the box – rough centering, safe for now
        int boxWidth = 61; // width between the side │ characters
        string paddedTitle = title.PadLeft(((boxWidth - title.Length) / 2) + title.Length)
            .PadRight(boxWidth);
        Console.WriteLine($"│{paddedTitle}│");
        Console.WriteLine("└─────────────────────────────────────────────────────────────┘");
        Console.WriteLine();
    }

    // extension method - extends Program class with password masking functionality
    public static string ReadPasswordMasked(this Program program)
    {
        var pwd = new System.Text.StringBuilder();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine(); // move to next line
                break;
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (pwd.Length > 0)
                {
                    pwd.Remove(pwd.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }
            else if (!char.IsControl(key.KeyChar))
            {
                pwd.Append(key.KeyChar);
                Console.Write("*");
            }
        }

        return pwd.ToString();
    }
}