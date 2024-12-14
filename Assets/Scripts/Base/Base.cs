using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scaner;
    [SerializeField] private UnitDirector _unitDirector;
    [SerializeField] private ShowEffector _effector;
    [SerializeField] private float _waitScaningValue = 5f;
    

    private WaitForSeconds _wait;
    private List<Resource> _oderedResouce = new List<Resource>();
    private List<Resource> _newResouces = new List<Resource>();

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitScaningValue);
    }

    private void Start()
    {
        StartCoroutine(GatherResources());
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
            _effector.ShowEffect();

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
