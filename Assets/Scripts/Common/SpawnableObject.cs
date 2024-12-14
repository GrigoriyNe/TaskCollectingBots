using System;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    public event Action<SpawnableObject> Returned;

    public void Return()
    {
        Returned.Invoke(this);
    }
}