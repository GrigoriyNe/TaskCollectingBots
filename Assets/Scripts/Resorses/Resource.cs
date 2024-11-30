using System;
public class Resource : SpawnerableObject
{
    public bool IsOdered {  get; private set; }
    public bool IsTaked {  get; private set; }

    private void Start()
    {
        IsOdered = false;
        IsTaked = false;
    }

    public void ChangeOdered()
    {
        IsOdered = true;
    }

    public void Taked(Unit unit)
    {
        IsTaked = true;
        transform.SetParent(unit.transform);
        transform.position = unit.transform.position;
    }

    public void Throw(Base thisBase)
    {
        transform.SetParent(thisBase.transform);
        IsTaked = false;
        IsOdered = false;
        Return();
    }
}
