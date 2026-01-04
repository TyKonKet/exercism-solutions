public static class ResistorColor
{
    public static readonly Dictionary<string, int> _colorCodes = new() {
        {"black", 0},
        {"brown", 1},
        {"red", 2},
        {"orange", 3},
        {"yellow", 4},
        {"green", 5},
        {"blue", 6},
        {"violet", 7},
        {"grey", 8},
        {"white", 9},
    };

    public static readonly string[] _colors = _colorCodes.Keys.ToArray();
    
    public static int ColorCode(string color) => _colorCodes[color];

    public static string[] Colors() => _colors;
}