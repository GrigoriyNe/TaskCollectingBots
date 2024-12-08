using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : SpawnerableObject
{
    [SerializeField] protected Transform Container;
    [SerializeField] private SpawnerableObject _prefab;

    protected Queue<T> Pool;
    protected int InitCreateValue = 1;

    public int ActiveItems {get; private set; }

    private void Awake()
    {
        Pool = new Queue<T>();
    }

    public void Reset()
    {
        Pool.Clear();
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

    protected void PutObject(SpawnerableObject item)
    {
        item.Returned -= PutObject;
        item.transform.SetParent(null);
        Pool.Enqueue(item as T);
        ActiveItems--;
    }

    protected void Activate(SpawnerableObject item)
    {
        item.Returned += PutObject;
        item.gameObject.SetActive(true);
    }


    private SpawnerableObject Init()
    {
        SpawnerableObject item = Instantiate(_prefab);

        return item;
    }
}