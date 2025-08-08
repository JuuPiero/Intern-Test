using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void ClearChildren(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }


    public static List<T> GetChildren<T>(this Transform transform) where T : Component
    {
        List<T> Children = new();
        int count = transform.childCount;

        for (int i = 0; i < count; i++)
        {
            T t = transform.GetChild(i).GetComponent<T>();
            if (t != null) Children.Add(t);
        }
        return Children;
    }

    public static List<T> GetChildrenRecursive<T>(this Transform transform) where T : Component
    {
        List<T> result = new();
        foreach (Transform child in transform)
        {
            T component = child.GetComponent<T>();
            if (component != null)
                result.Add(component);

            // Đệ quy lấy từ con của child
            result.AddRange(child.GetChildrenRecursive<T>());
        }
        return result;
    }
    
}