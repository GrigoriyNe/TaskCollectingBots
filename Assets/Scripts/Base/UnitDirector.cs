using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitDirector : MonoBehaviour
{
    [SerializeField] private List<Unit> _units;

    private List<Unit> _freeUnits = new List<Unit>();
    private List<Resource> _oderedResouce = new List<Resource>();
    private List<Resource> _freeResouce = new List<Resource>();

    private void OnEnable()
    {
        foreach (Unit unit in _units)
        {
            unit.IsCollect += OnFreeUnit;

            if (unit.IsBisy == false)
            {
                _freeUnits.Add(unit);
            }
        }
    }

    public void SetOrder(List<Resource> _newResources)
    {
        if (_freeUnits.Count == 0)
            return;

        if(_freeUnits.Count == _units.Count)
            _oderedResouce.Clear();

        if (_oderedResouce.Count == 0)
        {
            _freeResouce = _newResources;
        }
        else
        {
            if (_freeResouce.Count == 0)
            {
                foreach (Resource oldTask in _oderedResouce.ToList<Resource>())
                {
                    foreach (Resource newTask in _newResources.ToList<Resource>())
                    {
                        if (newTask == oldTask)
                        {
                            _newResources.Remove(newTask);
                        }
                        if (newTask.transform.position == Vector3.zero)
                        {
                            _newResources.Remove(newTask);
                        }
                    }
                }

                _freeResouce = _newResources;
            }
        }

        if (_freeResouce.Count > 0)
            GetOrders();
    }

    private void GetOrders()
    {
        if (_freeResouce.Count == 0)
            return;

        foreach (Unit unit in _freeUnits)
        {
            if (unit != null)
            {
                if (_freeResouce.Count > 0)
                {
                    Resource resource = _freeResouce[Random.Range(0, _freeResouce.Count)];
                    _oderedResouce.Add(resource);
                    _freeResouce.Remove(resource);
                    unit.TakeOrder(resource);
                }
            }
        }
    }

    private void OnFreeUnit(Resource resource)
    {
        if (_freeResouce.Count > 0)
        {
            GetOrders();
            _oderedResouce.Remove(resource);
        }
    }
}