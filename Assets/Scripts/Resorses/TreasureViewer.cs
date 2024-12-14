using System;
using TMPro;
using UnityEngine;

public class TreasureViewer : MonoBehaviour
{
    private const string SignsBetweenTexts = ": ";

    [SerializeField] private TextMeshProUGUI _viewTextName;
    [SerializeField] private TextMeshProUGUI _viewCounter;
    [SerializeField] private Treasure _treasure;

    private void Awake()
    {
        if (_viewTextName.text != _treasure.Name)
        {
            _treasure.SetName();
            ViewName();
        }
    }

    private void OnEnable()
    {
        _treasure.Returned += OnReturn;
    }

    private void OnDestroy()
    {
        _treasure.Returned -= OnReturn;
    }

    public void ViewName()
    {
        _viewTextName.text = _treasure.Name + SignsBetweenTexts;
    }

    private void OnReturn(SpawnableObject _)
    {
        int counter = Convert.ToInt32(_viewCounter.text.ToString());
        counter++;

        _viewCounter.text = counter.ToString();
    }
}
