using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private TreasureHolder _holder;
    [SerializeField] private float _speed = 3;

    private Base _base;
    private Coroutine _moving;
    private Treasure _target;
    private Vector3 _pointBuild;

    private bool _isBusy = false;
    private bool _isTreasureTaked = false;
    private bool _isBuilding = false;
    private int _maxDistanse = 15;

    public bool IsBisy => _isBusy;

    public event Action<Treasure, Unit> Collected;

    public void TakeOrder(Treasure target)
    {
        if (transform.position.x - target.transform.position.x > _maxDistanse)
            return;

        if (_isBusy == false)
        {
            _target = target;
            _isBusy = true;
            _base.AddOderedTreasures(target);

            _moving = null;
            _moving = StartCoroutine(MoveTo(_target.transform.position));
        }
    }

    public void RegistredBase(Base newBase)
    {
        _base = newBase;

        if (newBase.TryGetComponent(out UnitDirector director))
        {
            director.OnUnitAdd(this);
        }
    }

    public void BildBase(Vector3 newBase)
    {
        _isBusy = true;
        _isBuilding = true;
        _pointBuild = newBase;
        _moving = null;
        _moving = StartCoroutine(MoveTo(_pointBuild));

    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            transform.LookAt(target);

            yield return null;
        }

        CheckPosition();
    }

    private void CheckPosition()
    {
        if (transform.position == _target.transform.position && _isTreasureTaked == false)
        {
            _isTreasureTaked = true;
            _target.Take(_holder.transform);
            StopCoroutine(_moving);
            _moving = StartCoroutine(MoveTo(_base.transform.position));
        }
        else if (transform.position == _base.transform.position && _isTreasureTaked)
        {
            _base.RemoveOderedTreasures(_target);
            _target.Throw();
            _isTreasureTaked = false;
            StopCoroutine(_moving);
            _isBusy = false;
            Collected?.Invoke(_target, this);
        }
        else if (transform.position == _pointBuild && _isBuilding)
        {
            Base newBase = Instantiate(_basePrefab, _pointBuild, transform.rotation);
            StopCoroutine(_moving);
            _isBusy = false;
            _isBuilding = false;
            RegistredBase(newBase);
        }
    }
}