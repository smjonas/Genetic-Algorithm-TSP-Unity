using System.Collections.Generic;

public static class Extensions {

    /// <summary>
    /// Randomly rearranges the elements of a list.
    /// </summary>
    public static void ShuffleList<T>(this List<T> list) {

        int n = list.Count;

        while (n > 1) {
            int k = (UnityEngine.Random.Range(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
