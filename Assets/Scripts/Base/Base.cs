using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private float _radius = 50;

    private WaitForSecondsRealtime _wait;
    private Coroutine _coroutine;
    private List<Transform> _resoursesPosition;
    private float _waitValue = 5;

    private void Start()
    {
        _wait = new WaitForSecondsRealtime(_waitValue);
        _resoursesPosition = new List<Transform>();

        if (_coroutine == null)
            _coroutine = StartCoroutine(Scanning());
    }

    private IEnumerator Scanning()
    {
        while (enabled)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

            foreach (Collider hit in hits)
            {
                Debug.Log(hit);

                if (hit.TryGetComponent(out Resource _))
                    _resoursesPosition.Add(hit.transform);
            }

            if (_resoursesPosition.Count > 0)
            {
                GetOrder();
            }

            yield return _wait;
        }
    }

    private IEnumerator CreateUnit()
    {
        //while 
        yield return null;
    }

    private void GetOrder()
    {
        foreach (Transform position in _resoursesPosition)
            {
                _unit.TakeOrder(position);
            }
    }
}
