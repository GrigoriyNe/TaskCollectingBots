using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private float _speed = 3;
    [SerializeField] private PointTake _takePosition;

    private Coroutine _coroutine;
    private Resource _target;

    private bool _isBisy = false;
    private bool _isTakedResource = false;

    public bool IsBisy => _isBisy;

    public event Action<Resource, Unit> Collected;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.activeSelf == false)
            return;

        if (other.TryGetComponent(out Resource resource) && _isTakedResource == false)
        {
            if (transform.position == resource.transform.position)
            {
                _isTakedResource = true;
                _target.Taked(_takePosition.transform);
                StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(GoToBase());
            }
        }
        else if (other.TryGetComponent(out Base _) && _isTakedResource)
        {
            _base.RemoveOderedResoursce(_target);
            _target.Throw();
            _isTakedResource = false;
            _coroutine = null;
            _isBisy = false;
            Collected?.Invoke(_target, this);
        }
    }

    public void TakeOrder(Resource target)
    {
        if (_isBisy == false)
        {
            _target = target;
            _isBisy = true;
            _base.AddOderedResouce(target);

            _coroutine = null;
            _coroutine = StartCoroutine(GoToResource());
        }
    }

    private IEnumerator GoToResource()
    {
        while (transform.position != _target.transform.position)
        {
            MoveTo(_target.transform.position);

            yield return null;
        }
    }

    private IEnumerator GoToBase()
    {
        while (_isTakedResource)
        {
            MoveTo(_base.transform.position);

            yield return null;
        }
    }

    private void MoveTo(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        transform.LookAt(target);
    }
}