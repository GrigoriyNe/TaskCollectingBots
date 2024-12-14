using System.Collections;
using UnityEngine;

public class ShowEffector : MonoBehaviour
{
    [SerializeField] private ParticleSystemRenderer _effect;

    private WaitForSeconds _wait;
    private float _valueDelay = 2f;

    private void Awake()
    {
        _wait = new WaitForSeconds(_valueDelay);
        _effect.gameObject.SetActive(false);
    }

    public void ShowEffect()
    {
        _effect.gameObject.SetActive(true);
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate ()
    {
        yield return _wait;

        _effect.gameObject.SetActive(false);
    }
}
