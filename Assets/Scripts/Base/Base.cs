using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scaner;
    [SerializeField] private UnitDirector _unitDirector;
    [SerializeField] private float _waitScaningValue = 5f;
    [SerializeField] private ParticleSystemRenderer _effect;

    private WaitForSeconds _wait;
    private List<Resource> _oderedResouce = new List<Resource>();
    private List<Resource> _newResouces = new List<Resource>();

    private Coroutine _coroutine;

    private void Start()
    {
        _wait = new WaitForSeconds(_waitScaningValue);

        if (_coroutine == null)
            _coroutine = StartCoroutine(GatherResources());
    }

    public void RemoveOderedResoursce(Resource resource)
    {
        _oderedResouce.Remove(resource);
    }

    public void AddOderedResouce(Resource resource)
    {
        _oderedResouce.Add(resource);
    }

    private IEnumerator GatherResources()
    {
        while (enabled)
        {
            GetResources();
            SortResources();
            TryGetOrder(_newResouces);
            Instantiate(_effect, transform.position, transform.rotation);

            yield return _wait;
        }
    }

    private void GetResources()
    {
        _newResouces = _scaner.Scan();
    }
    
    private void SortResources()
    {
        _oderedResouce.ForEach(item => _newResouces.Remove(item));
    }

    private void TryGetOrder(List<Resource> targets)
    {
        if (targets.Count > 0)
            _unitDirector.SetOrder(targets);
    }
}
