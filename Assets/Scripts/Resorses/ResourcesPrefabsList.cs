using System.Collections.Generic;
using UnityEngine;

public class ResourcesPrefabsList : MonoBehaviour
{
    [SerializeField] private List<Resources> _resources;

    public Resources GetResourse()
    {
        int valueRandom = Random.Range(0, _resources.Count);

        return _resources[valueRandom];
    }
}
