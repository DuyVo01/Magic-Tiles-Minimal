using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public static class RandomExtension
{
    public static int GetRandomAvailableIndex<T>(this ICollection<T> collection, int occupiedIndex)
    {
        if (occupiedIndex == collection.Count - 1)
        {
            return UnityEngine.Random.Range(0, collection.Count - 1);
        }

        int randomValue = UnityEngine.Random.Range(0, collection.Count - 1);
        return randomValue >= occupiedIndex ? randomValue + 1 : randomValue;
    }

    public static int GetRandomAvailableIndex<T>(
        this ICollection<T> collection,
        IEnumerable<int> occupiedIndices
    )
    {
        HashSet<int> collectionToGetValue = new();

        for (int i = 0; i < collection.Count; i++)
        {
            if (occupiedIndices.Contains(i))
            {
                continue;
            }
            collectionToGetValue.Add(i);
        }

        int randomValue = UnityEngine.Random.Range(0, collectionToGetValue.Count);
        return collectionToGetValue.ElementAt(randomValue);
    }
}

public static class ArrayExtension
{
    public static int GetIndexOneUnitAway<T>(this ICollection<T> array, int fromIndex)
    {
        if (fromIndex >= array.Count - 2)
        {
            return fromIndex - 2;
        }
        else
        {
            return fromIndex + 2;
        }
    }
}
