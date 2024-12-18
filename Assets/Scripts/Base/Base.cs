using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent
    (typeof(Scanner),
    typeof(ShowEffector),
    typeof(UnitDirector))]
public class Base : MonoBehaviour
{
    [SerializeField] private float _waitScaningValue = 5f;
    [SerializeField] private SelectedAnimator _animator;

    private ShowEffector _effector;
    private UnitDirector _unitDirector;
    private Scanner _scaner;
    private WaitForSeconds _wait;

    private List<Treasure> _oderedTreasures;
    private List<Treasure> _newTreasures;

    private void OnEnable()
    {
        StartCoroutine(GatherTreasures());
    }

    private void OnDisable()
    {
        StopCoroutine(GatherTreasures());
    }

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitScaningValue);
        _oderedTreasures = new List<Treasure>();
        _newTreasures = new List<Treasure>();

        _scaner = this.GetComponent<Scanner>();
        _unitDirector = this.GetComponent<UnitDirector>();
        _effector = this.GetComponent<ShowEffector>();
    }

    public void RemoveOderedTreasures(Treasure treasures)
    {
        _oderedTreasures.Remove(treasures);
    }

    public void AddOderedTreasures(Treasure treasure)
    {
        _oderedTreasures.Add(treasure);
    }

    public void PlayAnimationSelected()
    {
        _animator.Play();
    }

    public void StopAnimationSelected()
    {
        _animator.Stop();
    }

    private IEnumerator GatherTreasures()
    {
        while (enabled)
        {
            RecordTreasures();
            SortTreasures();
            SendOrders(_newTreasures);
            _effector.ShowEffect();
            
            yield return _wait;
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
