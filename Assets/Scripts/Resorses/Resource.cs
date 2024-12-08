using UnityEngine;

public class Resource : SpawnerableObject
{

    private void Start()
    {
    }

    public void Taked(Transform parent)
    {
        transform.SetParent(parent);
        transform.position = parent.position;
    }

    public void Throw()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        Return();
    }
}
