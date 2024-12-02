using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Resource : SpawnerableObject
{
    private Rigidbody _rigidboby;

    private void Start()
    {
        _rigidboby = GetComponent<Rigidbody>();
    }

    public void Taked(Unit unit)
    {
        _rigidboby.isKinematic = true;
        transform.SetParent(unit.transform);
        transform.position = unit.transform.position;
    }

    public void Throw()
    {
        _rigidboby.isKinematic = false;
        transform.SetParent(null);
        Return();
    }
}
