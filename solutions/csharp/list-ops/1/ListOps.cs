public static class ListOps
{
    public static int Length<T>(List<T> input)
    {
        return input.Count;
    }

    public static List<T> Reverse<T>(List<T> input)
    {
        var result = new List<T>();
        for (int i = input.Count - 1; i >= 0; i--)
        {
            result.Add(input[i]);
        }
        return result;
    }

    public static List<TOut> Map<TIn, TOut>(List<TIn> input, Func<TIn, TOut> map)
    {
        var result = new List<TOut>();
        for (int i = 0; i < input.Count; i++)
        {
            result.Add(map(input[i]));
        }
        return result;
    }

    public static List<T> Filter<T>(List<T> input, Func<T, bool> predicate)
    {
        var result = new List<T>();
        for (int i = 0; i < input.Count; i++)
        {
            if (predicate(input[i]))
            {
                result.Add(input[i]);
            }
        }
        return result;
    }

    public static TOut Foldl<TIn, TOut>(List<TIn> input, TOut start, Func<TOut, TIn, TOut> func)
    {
        foreach (var item in input)
        {
            start = func(start, item);
        }
        return start;
    }

    public static TOut Foldr<TIn, TOut>(List<TIn> input, TOut start, Func<TIn, TOut, TOut> func)
    {
        for (int i = input.Count - 1; i >= 0; i--)
        {
            start = func(input[i], start);
        }
        return start;
    }

    public static List<T> Concat<T>(List<List<T>> input)
    {
        var result = new List<T>();
        foreach (var list in input)
        {
            result.AddRange(list); 
        }
        return result;
    }

    public static List<T> Append<T>(List<T> left, List<T> right)
    {
        return Concat([left, right]);
    }
}