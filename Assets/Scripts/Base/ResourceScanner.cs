using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner: MonoBehaviour
{
    [SerializeField] private float _radius = 50;

    private Queue<Transform> _positions = new Queue<Transform>();

    public Queue<Transform> Scanning()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Resource item))
            {
                _positions.Enqueue(item.transform);
            }
        }
        return _positions;
    }
}