using UnityEngine;

public abstract class Resource : SpawnerableObject
{
    public string Name => _name;

    protected string _name;

    public abstract void SetName();

    public void Take(Transform parent)
    {
        transform.SetParent(parent);
        transform.position = parent.position;
    }

    public void Throw()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
        Return();
    }
}
