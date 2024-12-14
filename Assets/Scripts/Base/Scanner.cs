using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _radius = 50;

    public List<Resource> Scan()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);
        List<Resource> resources = new List<Resource>();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Resource item))
            {
                resources.Add(item);
            }
        }

        return resources;
    }
}