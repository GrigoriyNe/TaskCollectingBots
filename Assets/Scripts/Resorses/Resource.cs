using UnityEngine;

public class Resource : SpawnerableObject
{
    public void Taked(Transform parent)
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
