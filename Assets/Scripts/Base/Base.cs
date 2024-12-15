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
    private List<Treasure> _oderedTreasures;
    private List<Treasure> _newTreasures;

    private void OnEnable()
    {
        StartCoroutine(GatherTreasures());
        _wait = new WaitForSeconds(_waitScaningValue);
        _oderedTreasures = new List<Treasure>();
        _newTreasures = new List<Treasure>();
    }

    public void RemoveOderedTreasures(Treasure treasures)
    {
        _oderedTreasures.Remove(treasures);
    }

    public void AddOderedTreasures(Treasure treasure)
    {
        _oderedTreasures.Add(treasure);
    }

    private IEnumerator GatherTreasures()
    {
        while (enabled)
        {
            yield return _wait;

            RecordTreasures();
            SortTreasures();
            SendOrders(_newTreasures);
            _effector.ShowEffect();
        }
    }

    private void RecordTreasures()
    {
        _newTreasures = _scaner.Scan();
    }
    
    private void SortTreasures()
    {
        _oderedTreasures.ForEach(item => _newTreasures.Remove(item));
    }

    private void SendOrders(List<Treasure> targets)
    {
        if (targets.Count > 0)
            _unitDirector.SetOrder(targets);
    }
}
