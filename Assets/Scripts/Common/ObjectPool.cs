using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : SpawnerableObject
{
    [SerializeField] private T _prefab;

    private Queue<T> Pool;

    private void Awake()
    {
        Pool = new Queue<T>();
    }

    public SpawnerableObject GetItem()
    {
        return CreateObject();
    }

    public void ReturnItem(T item)
    {
        Pool.Enqueue(item);
    }

    private SpawnerableObject CreateObject()
    {
        SpawnerableObject item;

        if (Pool.Count == 0)
        {
            item = Instantiate(_prefab);
            Activate(item);

            return item;
        }

        item = Pool.Dequeue();
        Activate(item);

        return item;
    }

    private void Activate(SpawnerableObject item)
    {
        item.transform.SetParent(null);
        item.gameObject.SetActive(true);
    }
}