using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _radius = 50;

    private List<Resource> _resources = new List<Resource>();

    public List<Resource> Scanning()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out Resource item))
                {
                    if (item.transform.parent == null && item.transform.position != Vector3.zero)
                    {
                        _resources.Add(item);
                    }
                }
            }

            return _resources;
        }
        else
        {
            return null;
        }
    }
}