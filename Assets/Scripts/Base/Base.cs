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
    private List<Treasure> _oderedResouce = new List<Treasure>();
    private List<Treasure> _newResouces = new List<Treasure>();

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitScaningValue);
    }

    private void Start()
    {
        StartCoroutine(GatherResources());
    }

    public void RemoveOderedResoursce(Treasure resource)
    {
        _oderedResouce.Remove(resource);
    }

    public void AddOderedResouce(Treasure resource)
    {
        _oderedResouce.Add(resource);
    }

    private IEnumerator GatherResources()
    {
        while (enabled)
        {
            SetResources();
            SortResources();
            TrySetOrder(_newResouces);
            _effector.ShowEffect();

            yield return _wait;
        }
    }

    private void SetResources()
    {
        _newResouces = _scaner.Scan();
    }
    
    private void SortResources()
    {
        _oderedResouce.ForEach(item => _newResouces.Remove(item));
    }

    private void TrySetOrder(List<Treasure> targets)
    {
        if (targets.Count > 0)
            _unitDirector.SetOrder(targets);
    }
}
