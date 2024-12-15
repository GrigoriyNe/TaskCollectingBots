using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class UnitDirector : MonoBehaviour
{
    private const int OffsetEnumeration = 1;

    //[SerializeField] private List<Unit> _units;
    [SerializeField] private BaseCrafter _crafer;

    private List<Treasure> _treasures = new List<Treasure>();
    private List<Unit> _units = new List<Unit>();

    private float _delayValue = 0.1f;
    private WaitForSeconds _wait;
    private bool _needNewBase = false;
    private FlagNewBase _flag;

    private void OnEnable()
    {
        _wait = new WaitForSeconds(_delayValue);
        _crafer.UnitCreated += OnUnitCreate;
    }

    private void OnDisable()
    {
        _crafer.UnitCreated -= OnUnitCreate;

        foreach (Unit unit in _units)
        {
            unit.Collected -= OnFreeUnit;
        }
    }

    private void Start()
    {
        foreach (Unit unit in _units)
        {
            unit.Collected += OnFreeUnit;
        }
    }

    public void SetOrder(List<Treasure> newTreasures)
    {
        _treasures = newTreasures;

        if (_treasures.Count > 0)
            ExecuteOrders();
    }

    public void BildBase(FlagNewBase flag)
    {
        _needNewBase = true;
        _flag = flag;
    }

    public void OnUnitCreate(Unit unit)
    {
        _units.Add(unit);
    }

    private void ExecuteOrders()
    {
        foreach (Unit unit in _units)
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
        if (_needNewBase)
        {
            unit.BildBase(_flag.transform.position);
            _needNewBase = false;
            _units.Remove(unit);
        }
        else if (_treasures.Count > 0)
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