using UnityEngine;

public abstract class Treasure : SpawnableObject
{
    protected string NameTreasure;
    public string Name => NameTreasure;

    private void OnEnable()
    {
        SetName();
    }

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
