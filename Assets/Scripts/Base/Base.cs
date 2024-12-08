using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scaner;
    [SerializeField] private UnitDirector _unitDirector;
    [SerializeField] private float _waitScaningValue = 5;
    [SerializeField] private WaitForSecondsRealtime _wait;
    [SerializeField] private ParticleSystemRenderer _effect;

    private Coroutine _coroutine;

    private void Start()
    {
        _wait = new WaitForSecondsRealtime(_waitScaningValue);

        if (_coroutine == null)
            _coroutine = StartCoroutine(GetResoursePositions());
    }

    private void TryGetOrder(List<Resource> targets)
    {
        if (targets.Count > 0)
            _unitDirector.SetOrder(targets);
    }

    private IEnumerator GetResoursePositions()
    {
        while (enabled)
        {
            List<Resource> targets = _scaner.Scanning();
            TryGetOrder(targets);
            Instantiate(_effect, transform.position, transform.rotation);

            yield return _wait;
        }
    }
}
