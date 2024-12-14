using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDirector : MonoBehaviour
{
    private const int OffsetEnumeration = 1;

    [SerializeField] private List<Unit> _units;

    private List<Unit> _freeUnits = new List<Unit>();
    private List<Treasure> _treasures = new List<Treasure>();

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

    public void SetOrder(List<Treasure> newTreasures)
    {
        if (_treasures.Count < _units.Count)
            _treasures = newTreasures;

        if (_freeUnits.Count == 0)
            return;

        if (_treasures.Count > 0)
            ExecuteOrders();
    }

    private void ExecuteOrders()
    {
        foreach (Unit unit in _freeUnits)
        {
            if (unit.IsBisy == false)
            {
                ExecuteOrders(unit);
            }
        }
    }

    private void ExecuteOrders(Unit unit)
    {
        if (_treasures.Count > 0)
        {
            Treasure treasure = _treasures[_treasures.Count - OffsetEnumeration];
            _treasures.Remove(treasure);
            unit.TakeOrder(treasure);
        }
    }

    private void OnFreeUnit(Treasure treasure, Unit unit)
    {
        if (_treasures.Count > 0)
        {
            StartCoroutine(ExecuteOrderFreeUnit(unit));
        }
    }

    private IEnumerator ExecuteOrderFreeUnit(Unit unit)
    {
        yield return _wait;

        ExecuteOrders(unit);
    }
}