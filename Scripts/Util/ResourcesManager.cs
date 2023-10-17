using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    private static Dictionary<string, object> cachedResources = new Dictionary<string, object>();

    public static T Load<T>(string path) where T : Object
    {
        T resource = default(T);

        if (cachedResources.ContainsKey(path))
        {
            resource = (T)cachedResources[path];
        }
        else
        {
            resource = Resources.Load<T>(path);
            cachedResources.Add(path, resource);

        }

        return resource;
    }

    public static T[] LoadAll<T>(string path) where T : Object
    {
        T[] resources = default(T[]);

        if (cachedResources.ContainsKey(path))
        {
            resources = (T[])cachedResources[path];
        }
        else
        {
            resources = Resources.LoadAll<T>(path);
            cachedResources.Add(path, resources);
        }

        return resources;
    }

    public static T LoadNew<T>(string path) where T : Object
    {
        T resource = default(T);

        resource = Resources.Load<T>(path);
        cachedResources.Add(path, resource);

        return resource;
    }

    public static T[] LoadNewAll<T>(string path) where T : Object
    {
        T[] resources = default(T[]);

        resources = Resources.LoadAll<T>(path);
        cachedResources.Add(path, resources);

        return resources;
    }
}
