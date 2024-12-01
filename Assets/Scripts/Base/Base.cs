using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private float _radius = 50;
    [SerializeField] private ParticleSystemRenderer _effect;
    [SerializeField] private UnitDirector _unitDirector;
    [SerializeField] private float _waitScaningValue = 5;

    private WaitForSecondsRealtime _wait;
    private Coroutine _coroutine;

    private void Start()
    {
        _wait = new WaitForSecondsRealtime(_waitScaningValue);

        if (_coroutine == null)
            _coroutine = StartCoroutine(Scanning());
    }

    private IEnumerator Scanning()
    {
        while (enabled)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius);
            Instantiate(_effect, transform.position, transform.rotation);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out Resource item))
                {
                    if (item.IsTaked == false)
                    {
                        _unitDirector.GetOrder(item.transform);
                    }
                }
            }

            yield return _wait;
        }
    }
}
