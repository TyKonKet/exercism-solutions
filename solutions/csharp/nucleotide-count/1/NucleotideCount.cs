public static class NucleotideCount
{
    public static IDictionary<char, int> Count(string sequence)
    {
        var nucleodites = new Dictionary<char, int>()
        {
            ['A'] = 0,
            ['C'] = 0,
            ['G'] = 0,
            ['T'] = 0
        };

        foreach(var nucleodite in sequence)
        {
            if (!nucleodites.ContainsKey(nucleodite))
            {
                throw new ArgumentException();
            }

            nucleodites[nucleodite]++;
        }

        return nucleodites;
    }
}