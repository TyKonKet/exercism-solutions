public static class Series
{
    public static string[] Slices(string numbers, int sliceLength)
    {
        if (sliceLength < 1 || sliceLength > numbers.Length)
            throw new ArgumentException();
        
        var slices = new List<string>();
        
        for (var i = 0; i <= numbers.Length - sliceLength; i++)
        {
            slices.Add(numbers[i..(i + sliceLength)]);
        }

        return slices.ToArray();
    }
}