using System;
public class Resource : SpawnerableObject
{
    public void Taked(Unit unit)
    {
        this.transform.SetParent(unit.transform);
    }

    public void Throw()
    {
        transform.SetParent(null);
        Return();
    }
}
