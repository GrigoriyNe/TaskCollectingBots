using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private PointTake _takePosition;
    [SerializeField] private float _speed = 3;

    private Coroutine _moving;
    private Resource _target;

    private bool _isBisy = false;
    private bool _isTakedResource = false;

    public bool IsBisy => _isBisy;

    public event Action<Resource, Unit> Collected;

    public void TakeOrder(Resource target)
    {
        if (_isBisy == false)
        {
            _target = target;
            _isBisy = true;
            _base.AddOderedResouce(target);

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
        if (transform.position == _target.transform.position
            && _isTakedResource == false)
        {
            _isTakedResource = true;
            _target.Take(_takePosition.transform);
            StopCoroutine(_moving);
            _moving = StartCoroutine(MoveTo(_base.transform.position));
        }
        else if (transform.position == _base.transform.position
            && _isTakedResource)
        {
            _base.RemoveOderedResoursce(_target);
            _target.Throw();
            _isTakedResource = false;
            StopCoroutine(_moving);
            _isBisy = false;
            Collected?.Invoke(_target, this);
        }
    }
}