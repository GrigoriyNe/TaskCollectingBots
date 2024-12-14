using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : SpawnableObject
{
    [SerializeField] private T _prefab;

    private Queue<T> Pool;

    private void Awake()
    {
        Pool = new Queue<T>();
    }

    public SpawnableObject GetItem()
    {
        return CreateObject();
    }

    public void ReturnItem(T item)
    {
        Pool.Enqueue(item);
    }

    private SpawnableObject CreateObject()
    {
        SpawnableObject item;

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

    private void Activate(SpawnableObject item)
    {
        item.transform.SetParent(null);
        item.gameObject.SetActive(true);
    }
}