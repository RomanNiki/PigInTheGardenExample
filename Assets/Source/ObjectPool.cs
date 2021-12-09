using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T _prefab;
    public bool AutoExpand { get; set; }
    public Transform _container { get; }

    private List<T> _pool;

    public ObjectPool(T prefab, int count)
    {
        _prefab = prefab;
        _container = null;
        CreatePool(count);
    }

    public ObjectPool(T prefab, int count, Transform container)
    {
        _prefab = prefab;
        _container = container;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>(count);
        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var createdObject = Object.Instantiate(_prefab, _container);
        createdObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(createdObject);
        return createdObject;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var mono in _pool.Where(mono => !mono.gameObject.activeInHierarchy))
        {
            element = mono;
            mono.gameObject.SetActive(true);
            return true;
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
        {
            return element;
        }

        if (AutoExpand)
        {
            return CreateObject(true);
        }
        
        throw new Exception($"No free element of type {typeof(T)}");
    }

    public int CountOfFreeElements()
    {
        return _pool.Count(mono => !mono.gameObject.activeInHierarchy);
    }
}