using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Resource : SpawnerableObject
{
    private Rigidbody _rigidboby;
    public bool IsTaked {  get; private set; }

    private void Start()
    {
        _rigidboby = GetComponent<Rigidbody>();
        IsTaked = false;
    }

    public void Taked(Unit unit)
    {
        _rigidboby.isKinematic = true;
        IsTaked = true;
        transform.SetParent(unit.transform);
        transform.position = unit.transform.position;
    }

    public void Throw()
    {
        _rigidboby.isKinematic = false;
        transform.SetParent(null);
        IsTaked = false;
        Return();
    }
}
