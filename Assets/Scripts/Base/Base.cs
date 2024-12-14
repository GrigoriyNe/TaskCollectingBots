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
    private List<Treasure> _oderedTreasures = new List<Treasure>();
    private List<Treasure> _newTreasures = new List<Treasure>();

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitScaningValue);
    }

    private void Start()
    {
        StartCoroutine(GatherTreasures());
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
            SetTreasures();
            SortTreasures();
            TrySetOrder(_newTreasures);
            _effector.ShowEffect();

            yield return _wait;
        }
    }

    private void SetTreasures()
    {
        _newTreasures = _scaner.Scan();
    }
    
    private void SortTreasures()
    {
        _oderedTreasures.ForEach(item => _newTreasures.Remove(item));
    }

    private void TrySetOrder(List<Treasure> targets)
    {
        if (targets.Count > 0)
            _unitDirector.SetOrder(targets);
    }
}
