using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDirector : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;

    private List<Unit> _freeUnits = new List<Unit>();
    private List<Resource> _tasks = new List<Resource>();

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
        _tasks = _newResources;

        if (_freeUnits.Count == 0)
            return;


        if (_tasks.Count > 0)
            GetOrders();
    }

    private void GetOrders()
    {
        foreach (Unit unit in _freeUnits)
        {
            if (_tasks.Count > 0)
            {
                Resource resource = _tasks[Random.Range(0, _tasks.Count)];
                _tasks.Remove(resource);
                unit.TakeOrder(resource);
            }
            else
            {
                StartCoroutine(GetOderFreeUnit(unit));
            }
        }
    }

    private void GetOrders(Unit unit)
    {
        if (_tasks.Count > 0)
        {
            Resource resource = _tasks[Random.Range(0, _tasks.Count)];
            _tasks.Remove(resource);
            unit.TakeOrder(resource);
        }
        else
        {
            StartCoroutine(GetOderFreeUnit(unit));
        }
    }

    private void OnFreeUnit(Resource resource, Unit unit)
    {
        if (_tasks.Count > 0)
        {
            if (_coroutine == null)
                StartCoroutine(GetOderFreeUnit(unit));
        }
    }

    private IEnumerator GetOderFreeUnit(Unit unit)
    {
        yield return new WaitForSecondsRealtime(.1f);

        GetOrders(unit);
        _coroutine = null;
    }
}