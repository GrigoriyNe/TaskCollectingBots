﻿using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _radius = 50;

    public List<Resource> Scan()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);
        List<Resource> _resources = new List<Resource>();

        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out Resource item))
                {
                    _resources.Add(item);
                }
            }
        }

        return _resources;
    }
}