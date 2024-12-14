using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEffector : MonoBehaviour
{
    [SerializeField] private ParticleSystemRenderer _effect;

    private void Awake()
    {
        _effect.gameObject.SetActive(false);
    }

    public void ShowEffect()
    {
        _effect.gameObject.SetActive(true);
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate ()
    {
        yield return new WaitForSeconds(2);

        _effect.gameObject.SetActive(false);
    }
}
