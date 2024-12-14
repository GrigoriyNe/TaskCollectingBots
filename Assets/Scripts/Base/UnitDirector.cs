using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDirector : MonoBehaviour
{
    private const int OffsetEnumeration = 1;

    [SerializeField] private List<Unit> _units;

    private List<Unit> _freeUnits = new List<Unit>();
    private List<Resource> _resources = new List<Resource>();

    private float _delayValue = 0.1f;
    private WaitForSeconds _wait;

    private void OnEnable()
    {
        _wait = new WaitForSeconds(_delayValue);

        foreach (Unit unit in _units)
        {
            unit.Collected += OnFreeUnit;

            if (unit.IsBisy == false)
            {
                _freeUnits.Add(unit);
            }
        }
    }

    private void OnDisable()
    {
        foreach (Unit unit in _units)
        {
            unit.Collected -= OnFreeUnit;
        }
    }

    public void SetOrder(List<Resource> newResources)
    {
        if (_resources.Count < _units.Count)
            _resources = newResources;

        if (_freeUnits.Count == 0)
            return;

        if (_resources.Count > 0)
            GetOrders();
    }

    private void GetOrders()
    {
        foreach (Unit unit in _freeUnits)
        {
            if (unit.IsBisy == false)
            {
                GetOrder(unit);
            }
        }
    }

    private void GetOrder(Unit unit)
    {
        if (_resources.Count > 0)
        {
            Resource resource = _resources[_resources.Count - OffsetEnumeration];
            _resources.Remove(resource);
            unit.TakeOrder(resource);
        }
    }

    private void OnFreeUnit(Resource resource, Unit unit)
    {
        if (_resources.Count > 0)
        {
            StartCoroutine(GetOderFreeUnit(unit));
        }
    }

    private IEnumerator GetOderFreeUnit(Unit unit)
    {
        yield return _wait;

        GetOrder(unit);
    }
}