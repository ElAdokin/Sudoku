using System.Collections.Generic;

public static class ListExtensions
{
    public static void MoveListElement<T>(this List<T> list, int oldIndex, int newIndex)
    {
        T item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }
}
