using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

    public event Action<Resource, Unit> Collected;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.activeSelf == false)
            return;

        if (other.TryGetComponent(out Resource resource) && _isTakedResource == false )
        {
            if (transform.position == resource.transform.position && _target.transform.parent == null)
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
            StopCoroutine(GoToBase());
            _coroutine = null;
            Collected?.Invoke(_target, this);
            _isBisy = false;
        }
    }

    public void TakeOrder(Resource target)
    {
        if (_isBisy == false && target.transform.parent == null && target.transform.position != Vector3.zero)
        {
            _target = target;
            _isBisy = true;

            _coroutine = null;
            _coroutine = StartCoroutine(GoToResource());
        }
    }

    private IEnumerator GoToResource()
    {
        while (transform.position != _target.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_target.transform.position);

            //if (_target.transform.position == Vector3.zero || _target.transform.parent != null)
            //{
            //    _isBisy = false;
            //    _target = null;
            //    StopCoroutine(GoToResource());
            //    _coroutine = null;
            //}

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
