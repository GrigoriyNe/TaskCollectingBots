using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDirector : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;

    private List<Unit> _freeUnits;
    private Coroutine _coroutine;
    private bool _isTaskOnQuene = false;
    private Queue<Transform> tasks = new Queue<Transform>();

    private void OnEnable()
    {
        _freeUnits = new List<Unit>();
        GetFreeUnit();
    }

    public void GetOrder(Transform _resoursePosition)
    {
        if (_freeUnits.Count > 0)
        {
            Unit unit = GetFreeUnit();
            unit.TakeOrder(_resoursePosition);
        }
        else if (_isTaskOnQuene)
        {
            Unit unit = GetFreeUnit();
            unit.TakeOrder(tasks.Dequeue());
        }
        else
        {
            _isTaskOnQuene = true;
            WriteTask(_resoursePosition);
        }
    }

    private void WriteTask(Transform _resoursePosition)
    {
        tasks.Enqueue(_resoursePosition);
    }

    private Unit GetFreeUnit()
    {
        foreach (Unit unit in _units)
        {
            if (unit.IsBisy == false)
            {
                _freeUnits.Add(unit);
            }
        }

        int randomValue = Random.Range(0, _freeUnits.Count);

        return _freeUnits[randomValue];
    }
}