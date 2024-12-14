using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TreasureHolder _holder;
    [SerializeField] private float _speed = 3;

    private Coroutine _moving;
    private Treasure _target;

    private bool _isBusy = false;
    private bool _isTreasureTaked = false;

    public bool IsBisy => _isBusy;

    public event Action<Treasure, Unit> Collected;

    public void TakeOrder(Treasure target)
    {
        if (_isBusy == false)
        {
            _target = target;
            _isBusy = true;
            _base.AddOderedTreasures(target);

            _moving = null;
            _moving = StartCoroutine(MoveTo(_target.transform.position));

        }
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
    }
}