using System;
using System.Collections;
using UnityEngine;

public class Unit : SpawnerableObject
{
    [SerializeField] private Base _base;
    [SerializeField] private float _speed = 2;

    private WaitForSecondsRealtime _wait;
    private Coroutine _coroutine;
    private float _waitValue = 5;
    private Resource _target;

    private bool _isBisy = false;
    private bool _isTakedResource = false;

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
            _target.Throw(_base);
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
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
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
            transform.position = Vector3.MoveTowards(transform.position, _base.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_base.transform.position);

            yield return null;
        }
    }
}
