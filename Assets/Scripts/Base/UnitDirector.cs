using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDirector : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;

    private List<Unit> _freeUnits = new List<Unit>();
    private List<Resource> _resources = new List<Resource>();

    private Coroutine _coroutine = null;

    private void OnEnable()
    {
        foreach (Unit unit in _units)
        {
            unit.Collected += OnFreeUnit;

            if (unit.IsBisy == false)
            {
                _freeUnits.Add(unit);
            }
        }
    }

    public void SetOrder(List<Resource> _newResources)
    {
        if (_resources.Count < _units.Count)
            _resources = _newResources;

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
            Resource resource = _resources[Random.Range(0, _resources.Count)];
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
        yield return new WaitForSecondsRealtime(.1f);

        GetOrder(unit);
    }
}