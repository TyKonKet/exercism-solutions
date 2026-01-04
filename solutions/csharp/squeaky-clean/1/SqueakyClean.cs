public static class Identifier
{
    public static string Clean(string identifier) => identifier.Replace(' ', '_').Replace("\0", "CTRL").KebabToCamel().RemoveNonLetterCharacters().RemoveGreekLetters();

    static string KebabToCamel(this string input)
    {
        if (!input.Contains('-')) return input;
        var parts = input.Split('-');
        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i].Length > 0)
            {
                parts[i] = char.ToUpper(parts[i][0]) + parts[i][1..];
            }
        }
        return string.Join(string.Empty, parts);
    }

    static string RemoveNonLetterCharacters(this string input)
    {
        return new string([.. input.Where(c => char.IsLetter(c) || c == '_')]);
    }

    static string RemoveGreekLetters(this string input)
    {
        return new string([.. input.Where(c => c < 'α' || c > 'ω')]);
    }
}
