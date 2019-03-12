using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public static class TransformExt
{
    public static T FindChild<T>(this Transform transform) where T : class
    {
        return transform.Cast<Transform>()
            .SelectMany(ch => ch.GetComponents<Component>())
            .OfType<T>()
            .FirstOrDefault();
    }

    public static IEnumerable<T> Get<T>(this Transform tr) where T : class
    {
        return tr.GetComponents<Component>().OfType<T>();
    }

    public static void ForEach<T>(this Transform tr, Action<T> act) where T : class
    {
        foreach (var ch in tr.Get<Component>().OfType<T>())
            act(ch);
    }

    public static T FindChildNamed<T>(this Transform trans, string name) where T: Component
    {
        return trans.GetComponentsInChildren<T>().FirstOrDefault(ch => ch.name == name);
    }

    public static T FindChildNamed<T>(this GameObject trans, string name) where T: Component
    {
        return trans.GetComponentsInChildren<T>().FirstOrDefault(ch => ch.name == name);
    }
}
