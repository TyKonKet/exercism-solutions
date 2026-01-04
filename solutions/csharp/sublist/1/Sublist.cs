public enum SublistType
{
    Equal,
    Unequal,
    Superlist,
    Sublist
}

public static class Sublist
{
    public static SublistType Classify<T>(List<T> listA, List<T> listB) where T : IComparable
    {
        if (listA is null || listB is null)
        {
            return listA == listB ? SublistType.Equal : SublistType.Unequal;
        }

        if (listA.Count == listB.Count)
        {
            for (int i = 0; i < listA.Count; i++)
            {
                if (listA[i].CompareTo(listB[i]) != 0)
                {
                    return SublistType.Unequal;
                }
            }
            return SublistType.Equal;
        }
        else if (listA.Count < listB.Count)
        {
            return IsSublist(listA, listB) ? SublistType.Sublist : SublistType.Unequal;
        }
        else
        {
            return IsSublist(listB, listA) ? SublistType.Superlist : SublistType.Unequal;
        }
    }

    private static bool IsSublist<T>(List<T> sub, List<T> super) where T : IComparable
    {
        if (sub.Count == 0)
            return true;

        var matchingCount = 0;
        for (int i = 0; i < super.Count; i++)
        {
            if (super[i].CompareTo(sub[matchingCount]) == 0)
            {
                matchingCount++;
                if (matchingCount == sub.Count)
                    return true;
            }
            else
            {
                i -= matchingCount;
                matchingCount = 0;
            }
        }
        return false;
    }
}