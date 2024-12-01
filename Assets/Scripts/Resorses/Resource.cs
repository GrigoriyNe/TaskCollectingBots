using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Resource : SpawnerableObject
{
    private Rigidbody _rb;
    public bool IsOdered {  get; private set; }
    public bool IsTaked {  get; private set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        IsOdered = false;
        IsTaked = false;
    }

    public void ChangeOdered()
    {
        IsOdered = true;
    }

    public void Taked(Unit unit)
    {
        _rb.isKinematic = true;
        IsTaked = true;
        transform.SetParent(unit.transform);
        transform.position = unit.transform.position;
    }

    public void Throw(Base thisBase)
    {
        _rb.isKinematic = false;
        transform.SetParent(null);
        IsTaked = false;
        IsOdered = false;
        Return();
    }
}
