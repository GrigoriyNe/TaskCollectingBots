using System.Collections;
using UnityEngine;

public class BaseAnimator : MonoBehaviour
{
    private const string IsSelected = nameof(IsSelected);
    private const string IsWrongPlace = nameof(IsWrongPlace);

    [SerializeField] private Animator _animator;

    private WaitForSeconds _wait;
    private float _waitValue = 0.5f;

    private void Awake()
    {
        _wait = new WaitForSeconds(_waitValue);
    }

    public void PlayAnimationSelected()
    {
        _animator.SetBool(IsSelected, true);
    }

    public void StopAnimationSelected()
    {
        _animator.SetBool(IsSelected, false);
    }

    public void PlayAnimationWhrongPlace()
    {
        _animator.SetBool(IsWrongPlace, true);
        StartCoroutine(StopAnimation());
    }

    private IEnumerator StopAnimation()
    {
        yield return _wait;
        _animator.SetBool(IsWrongPlace, false);
    }
}