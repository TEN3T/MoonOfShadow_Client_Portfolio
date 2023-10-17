using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    protected T prefab;
    protected int count;
    protected Transform container;

    protected Stack<T> pool;

    public ObjectPool(T prefab, Transform container)
    {
        this.prefab = prefab;
        this.container = container;
        count = 1;
        pool = new Stack<T>();
    }

    public ObjectPool(T prefab, Transform container, int count) : this(prefab, container)
    {
        this.count = count;
    }

    public T GetObject()
    {
        if (pool.Count == 0)
        {
            AddObject();
        }

        return pool.Pop();
    }

    public void ReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Push(obj);
    }

    private void AddObject()
    {
        for (int i = 0; i < count; i++)
        {
            T obj = Object.Instantiate(prefab, container);
            obj.gameObject.SetActive(false);
            pool.Push(obj);
        }
    }

    //[SerializeReference] protected T prefab;
    //[SerializeField] [Range(1, 1000)] protected int count = 1;

    //protected Stack<T> pool;

    //protected virtual void Awake()
    //{
    //    pool = new Stack<T>();
    //    AddObject();
    //}

    //public T GetObject()
    //{
    //    if (pool.Count == 0)
    //    {
    //        AddObject();
    //    }

    //    T obj = pool.Pop();
    //    obj.gameObject.transform.SetParent(transform);
    //    return obj;
    //}

    //public void ReleaseObject(T obj)
    //{
    //    obj.gameObject.SetActive(false);
    //    obj.gameObject.transform.SetParent(transform);
    //    pool.Push(obj);
    //}

    //public void AddObject()
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        T obj = Instantiate(prefab, transform);
    //        obj.gameObject.SetActive(false);
    //        pool.Push(obj);
    //    }
    //}
}
