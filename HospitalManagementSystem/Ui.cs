namespace HospitalManagementSystem;

public static class Ui
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
}