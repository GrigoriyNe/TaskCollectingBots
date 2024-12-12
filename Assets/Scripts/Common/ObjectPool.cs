using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : SpawnerableObject
{
    [SerializeField] private Transform Container;
    [SerializeField] private T _prefab;

    protected Queue<T> Pool;
    protected int InitCreateValue = 1;

    public int ActiveItems {get; private set; }

    private void Awake()
    {
        Pool = new Queue<T>();
    }

    public SpawnerableObject GetObject()
    {
        ActiveItems++;
        return CreateObject();
    }

    private SpawnerableObject CreateObject()
    {
        SpawnerableObject item;

        if (Pool.Count < InitCreateValue)
        {
            item = Init();
            Activate(item);

            return item;
        }

        item = Pool.Dequeue();
        Activate(item);

        return item;
    }

    private void PutObject(SpawnerableObject item)
    {
        item.transform.SetParent(Container);
        item.Returned -= PutObject;
        Pool.Enqueue(item as T);
        ActiveItems--;
    }

    private void Activate(SpawnerableObject item)
    {
        item.Returned += PutObject;
        item.transform.SetParent(null);
        item.gameObject.SetActive(true);
    }

    private SpawnerableObject Init()
    {
        return Instantiate(_prefab);
    }
}