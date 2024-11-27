using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : SpawnerableObject
{
    [SerializeField] protected Transform Container;
    [SerializeField] protected ResourcesPrefabsList Prefabs;

    protected Queue<T> Pool;
    private int _initCreateValue = 10;

    private void Awake()
    {
        Pool = new Queue<T>();
    }

    public void Reset()
    {
        Pool.Clear();
        EnqueueContainer();
    }

    public SpawnerableObject GetObject()
    {
        SpawnerableObject item;

        if (Pool.Count < _initCreateValue)
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
        item.gameObject.SetActive(false);
        Pool.Enqueue(item as T);
    }

    protected void Activate(SpawnerableObject item)
    {
        item.Returned += PutObject;
        item.gameObject.SetActive(true);
    }

    protected void EnqueueContainer()
    {
        for (int i = 0; i < Container.childCount; i++)
        {
            var putableItem = Container.GetChild(i).gameObject;

            if (putableItem.TryGetComponent(out SpawnerableObject item))
                PutObject(item);
        }
    }

    private SpawnerableObject Init()
    {
        SpawnerableObject item = Instantiate(Prefabs.GetResourse());
        item.transform.parent = Container;
     //   item.Returned += PutObject;

        return item;
    }


}