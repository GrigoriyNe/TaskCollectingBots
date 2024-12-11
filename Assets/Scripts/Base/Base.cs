using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scaner;
    [SerializeField] private UnitDirector _unitDirector;
    [SerializeField] private float _waitScaningValue = 5;
    [SerializeField] private WaitForSecondsRealtime _wait;
    [SerializeField] private ParticleSystemRenderer _effect;

    private List<Resource> _oderedResouce = new List<Resource>();
    private List<Resource> _newResouces = new List<Resource>();

    private Coroutine _coroutine;

    private void Start()
    {
        _wait = new WaitForSecondsRealtime(_waitScaningValue);

        if (_coroutine == null)
            _coroutine = StartCoroutine(GetResoursePositions());
    }

    private void TryGetOrder(List<Resource> targets)
    {
        if (targets.Count > 0)
            _unitDirector.SetOrder(targets);


    }

    private IEnumerator GetResoursePositions()
    {
        while (enabled)
        {
            _newResouces = _scaner.Scanning();

            //    List<Resource> result = _newResouces.Except(_oderedResouce).ToList();
            //  _newResouces.RemoveAll(l => _oderedResouce.Contains(l));
            _oderedResouce.ForEach(item => _newResouces.Remove(item));

            foreach (Resource target in _newResouces.ToList<Resource>())
            {
                foreach (Resource oldTarget in _oderedResouce.ToList<Resource>())
                {
                    if (target.transform.position == oldTarget.transform.position) // && target.transform.parent != null)
                    {
                        _newResouces.Remove(target);
                    }
                    else
                    {
                        _oderedResouce.Add(target);
                    }
                }
            }
            TryGetOrder(_newResouces);
            Instantiate(_effect, transform.position, transform.rotation);

            yield return _wait;
        }
    }
    //}
}
