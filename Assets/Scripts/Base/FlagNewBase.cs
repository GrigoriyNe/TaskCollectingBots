using UnityEngine;

public class FlagNewBase : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Base _))
            gameObject.SetActive(false);
    }
}