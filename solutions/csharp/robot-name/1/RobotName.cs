public class Robot
{
    private string? name = null;

    private static char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    private static List<string> usedNames = [];

    public string Name
    {
        get
        {
            name ??= GetRandomName();
            return name;
        }
    }

    public static string GetRandomName()
    {
        string newName;
        do
        {
            char letter1 = letters[Random.Shared.Next(letters.Length)];
            char letter2 = letters[Random.Shared.Next(letters.Length)];
            int number = Random.Shared.Next(0, 1000);
            newName = $"{letter1}{letter2}{number:D3}";
        } while (usedNames.Contains(newName));
        usedNames.Add(newName);
        return newName;
    }

    public void Reset()
    {
        if (name != null)
        {
            usedNames.Remove(name);
        }
        name = null;
    }
}