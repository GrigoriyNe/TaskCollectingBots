using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private float _speed = 3;
    [SerializeField] private PointTake _takePosition;

    private WaitForSecondsRealtime _wait;
    private Coroutine _coroutine;
    private Resource _target;

    private bool _isBisy = false;
    private bool _isTakedResource = false;

    public bool IsBisy => _isBisy;

    public event Action<Resource> IsCollect;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.activeSelf == false)
            return;

        if (other.TryGetComponent(out Resource resource) && _isTakedResource == false && _target.transform.parent == null)
        {
            if (_target.transform.position == resource.transform.position)
            {
                _isTakedResource = true;
                _target.Taked(_takePosition.transform);
                StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(GoToBase());
            }
        }
        else if (other.TryGetComponent(out Base _) && _isTakedResource)
        {
            _target.Throw();
            _isTakedResource = false;
            _isBisy = false;
            _coroutine = null;
            IsCollect?.Invoke(_target);
        }
    }

    public void TakeOrder(Resource target)
    {
        if (_isBisy == false && target.transform.parent == null)
        {
            _target = target;
            _isBisy = true;

            if (_coroutine == null)
                _coroutine = StartCoroutine(GoToResource());
        }
    }

    private Vector3 FindResoursePosition()
    {
        if (_target != null)
            return _target.transform.position;
        else
            return Vector3.zero;

    }

    private IEnumerator GoToResource()
    {
        while (_isTakedResource == false)
        {
            Vector3 targetPosition = FindResoursePosition();

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            transform.LookAt(_target.transform.position);

            if (_target.transform.position == Vector3.zero)
            {
                _isBisy = false;
                _coroutine = null;
                _target = null;
            }

            yield return null;
        }
    }

    private IEnumerator GoToBase()
    {
        while (_isTakedResource)
        {
            transform.position = Vector3.MoveTowards(transform.position, _base.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_base.transform.position);

            yield return null;
        }
    }
}
