using UnityEngine;

public class SelectedAnimator : MonoBehaviour
{
    private const string IsSelected = nameof(IsSelected);

    [SerializeField] private Animator _animator;

    public void Play()
    {
        _animator.SetBool(IsSelected, true);
    }

    public void Stop()
    {
        _animator.SetBool(IsSelected, false);
    }
}