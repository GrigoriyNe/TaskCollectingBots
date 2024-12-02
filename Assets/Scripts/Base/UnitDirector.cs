using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDirector : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;

    private List<Unit> _freeUnits = new List<Unit>();
    private Queue<Transform> _tasks = new Queue<Transform>();

    private Unit _freeUnit;
    private Coroutine _coroutine;
    private bool _isTaskOnQuene = false;


    private void OnEnable()
    {
        GetFreeUnit();
    }

    private IEnumerator GetOrders()
    {
        while (_tasks.Count > 0)
        {
            Unit unit = GetFreeUnit();
            {
                unit.TakeOrder(_tasks.Dequeue());
            }
            yield return null;

        }
        _coroutine = null;

    }

    public void SetOrder(Queue<Transform> _resoursePosition)
    {
        if (_resoursePosition.Count > 0)
        {
            _tasks = _resoursePosition;

            if (_coroutine == null)
                _coroutine = StartCoroutine(GetOrders());
        }
    }

    private void OnFreeUnit(Resource _)
    {
        if (_freeUnits.Count > 0)
        {
            _freeUnits.Add(_freeUnit);
        }
    }


    private Unit GetFreeUnit()
    {
        foreach (Unit unit in _units)
        {
            unit.IsCollect += OnFreeUnit;
            _freeUnit = unit;

            if (unit.IsBisy == false)
            {
                _freeUnits.Add(unit);
            }
        }

        int randomValue = Random.Range(0, _freeUnits.Count);

        return _freeUnits[randomValue];
    }
}