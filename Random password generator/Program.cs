using System;
using System.IO;
using System.Security.Principal;
using System.Text;

class PasswordGenerator
{
    static void Main(string[] args)
    {
        Console.Write("Enter the length of the password: ");
        int length = int.Parse(Console.ReadLine());

        Console.Write("Do you want to include capital letters? (Y/N): ");
        char capitalLetters = Console.ReadKey().KeyChar;

        Console.Write("\nDo you want to include special characters? (Y/N): ");
        char specialChars = Console.ReadKey().KeyChar;

        Console.Write("\nDo you want to include numbers? (Y/N): ");
        char numbers = Console.ReadKey().KeyChar;

        Console.Write("\nDo you want to save the password to the desktop? (Y/N): ");
        char saveToDesktop = Console.ReadKey().KeyChar;

        string password = GeneratePassword(length, capitalLetters == 'Y', specialChars == 'Y', numbers == 'Y');
        Console.WriteLine("\nYour password is: " + password);

        if (saveToDesktop == 'Y')
        {
            Console.Write("\nEnter the directory name where you want to save the password file: ");
            string directoryName = Console.ReadLine();

            SavePasswordToFile(password, directoryName);
        }
        else
        {
            SavePasswordToFile(password);
        }

        Console.ReadLine();
    }

    static string GeneratePassword(int length, bool includeCapitalLetters, bool includeSpecialChars, bool includeNumbers)
    {
        const string letters = "abcdefghijklmnopqrstuvwxyz";
        const string capitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "1234567890";
        const string specialChars = "!@#$%^&*()_+-=[]{};:,.<>?";

        StringBuilder password = new StringBuilder();
        Random random = new Random();

        string sourceChars = letters;

        if (includeCapitalLetters)
        {
            sourceChars += capitalLetters;
        }

        if (includeSpecialChars)
        {
            sourceChars += specialChars;
        }

        if (includeNumbers)
        {
            sourceChars += numbers;
        }

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(sourceChars.Length);
            password.Append(sourceChars[index]);
        }

        return password.ToString();
    }

    static void SavePasswordToFile(string password, string directoryName = null)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        if (!IsUserAdministrator())
        {
            Console.WriteLine("Administrator privileges required to save file.");
            return;
        }

        string directoryPath;

        if (directoryName == null)
        {
            directoryPath = desktopPath;
        }
        else
        {
            directoryPath = Path.Combine(desktopPath, directoryName);
        }

        try
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryPath, "password.txt")))
            {
                writer.Write(password);
            }
            Console.WriteLine("Password saved to file: " + Path.Combine(directoryPath, "password.txt"));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving password to file: " + ex.Message);
        }
    }

    static bool IsUserAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}