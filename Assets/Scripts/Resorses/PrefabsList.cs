using System.Collections.Generic;
using UnityEngine;

public class PrefabsList : MonoBehaviour
{
    [SerializeField] private List<SpawnerableObject> _items;

    public SpawnerableObject Get()
    {
        int valueRandom = Random.Range(0, _items.Count);

        return _items[valueRandom];
    }
}
