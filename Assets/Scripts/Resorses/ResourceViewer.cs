using System;
using TMPro;
using UnityEngine;

public class ResourceViewer : MonoBehaviour
{
    private const string SignsBetweenTexts = ": ";

    [SerializeField] private TextMeshProUGUI _viewTextName;
    [SerializeField] private TextMeshProUGUI _viewCounter;
    [SerializeField] private Resource _resource;

    private void Awake()
    {
        if (_viewTextName.text != _resource.Name)
        {
            _resource.SetName();
            ViewName();
        }
    }

    private void OnEnable()
    {
        _resource.Returned += OnReturn;
    }

    private void OnDestroy()
    {
        _resource.Returned -= OnReturn;
    }

    public void ViewName()
    {
        _viewTextName.text = _resource.Name + SignsBetweenTexts;
    }

    private void OnReturn(SpawnerableObject _)
    {
        int counter = Convert.ToInt32(_viewCounter.text.ToString());
        counter++;

        _viewCounter.text = counter.ToString();
    }
}
