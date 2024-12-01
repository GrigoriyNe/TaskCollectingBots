using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private float _speed = 2;

    private WaitForSecondsRealtime _wait;
    private Coroutine _coroutine;
    private Resource _target;
    private float _waitValue = 5;

    private bool _isBisy = false;
    private bool _isTakedResource = false;

    public event Action<Resource> IsCollect;

    public bool IsBisy => _isBisy;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && _isTakedResource == false)
        {
            if (resource.IsTaked == false)
            {
                _target.Taked(this);
                _isTakedResource = true;
                _coroutine = null;

                _coroutine = StartCoroutine(GoToBase());
            }
        }

        else if (other.TryGetComponent(out Base _) && _isTakedResource)
        {
            IsCollect?.Invoke(_target);
            _target.Throw();
            _coroutine = null;
            _isTakedResource = false;
            _isBisy = false;
        }
    }

    public void TakeOrder(Transform target)
    {
        if (target.TryGetComponent(out Resource resours) && _isBisy == false)
        {
            if (resours.IsTaked == false)
            {
                _target = resours;
                _isBisy = true;
            }

            if (_coroutine == null)
                _coroutine = StartCoroutine(GoToResource());
        }
    }

    private IEnumerator GoToResource()
    {
        while (_isTakedResource == false)
        {
            if (_target.IsTaked == false)
            { 
                Vector3 targetPosition = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z); 
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
                transform.LookAt(_target.transform.position);
            }
            else
            {
                _coroutine = null;
                _isTakedResource = false;
                _isBisy = false;
            }

            yield return null;
        }
    }

    private IEnumerator GoToBase()
    {
        while (_isTakedResource)
        {
            Vector3 targetPosition = new Vector3(_base.transform.position.x, transform.position.y, _base.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            transform.LookAt(_base.transform.position);

            yield return null;
        }
    }
}
