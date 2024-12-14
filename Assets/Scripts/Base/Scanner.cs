using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _radius = 50;

    public List<Treasure> Scan()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);
        List<Treasure> treasures = new List<Treasure>();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Treasure item))
            {
                treasures.Add(item);
            }
        }

        return treasures;
    }
}