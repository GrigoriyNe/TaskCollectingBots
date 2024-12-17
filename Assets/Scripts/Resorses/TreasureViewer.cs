using System;
using TMPro;
using UnityEngine;

public class TreasureViewer : MonoBehaviour
{
    private const string SignsBetweenTexts = ": ";

    [SerializeField] private TextMeshProUGUI _viewTextName;
    [SerializeField] private TextMeshProUGUI _viewCounter;
    [SerializeField] private UnitDirector _director;

    private void OnEnable()
    {
        _viewCounter.text = "0";
    }

    private void OnDisable()
    {
        foreach (Unit unit in _director.Units)
        {
            unit.Collected -= OnCollected;
        }
    }

    private void Start()
    {
        foreach (Unit unit in _director.Units)
        {
            unit.Collected += OnCollected;
        }
    }

    private void OnCollected(Treasure treasure, Unit _)
    {
        if (_viewTextName.text.ToString() == treasure.Name.ToString())
        {
            int counter = Convert.ToInt32(_viewCounter.text.ToString());
            counter++;

            _viewCounter.text = counter.ToString();
        }
    }
}
