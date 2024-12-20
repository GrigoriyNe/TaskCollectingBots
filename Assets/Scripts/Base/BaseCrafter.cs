﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent
    (typeof(Base),
    typeof(UnitDirector))]
public class BaseCrafter : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private TextMeshProUGUI _counterWood;
    [SerializeField] private TextMeshProUGUI _counterMetal;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private FlagNewBase _flag;
    [SerializeField] private int _startUnits;

    private Base _base;
    private Scanner _scanner;
    private UnitDirector _director;
    private PlayerInputController _input;

    private int _treasuresForCreateUint = 3;
    private int _treasuresForCreateBase = 5;
    private int _radiusMultiplier = 2;
    private int _valueWait = 3;
    private bool _isNeedBuildBase;
    private bool _onBaseSelected;
    private bool _isflagIsOver;

    private Coroutine _unitCreating;
    private Coroutine _waitingTreasure;
    private RaycastHit _clickHit;
    private WaitForSeconds _wait;

    public event Action<Unit> UnitCreated;

    private void OnEnable()
    {
        _director = this.GetComponent<UnitDirector>();
        _base = this.GetComponent<Base>();
        _scanner = this.GetComponent<Scanner>();

        _isNeedBuildBase = false;
        _onBaseSelected = false;
        _wait = new WaitForSeconds(_valueWait);

        for (int i = 0; i < _startUnits; i++)
        {
            CreateUnit();
        }

        _unitCreating = StartCoroutine(CreateNewUnit());

        if (_input == null)
        {
            PlayerInputController[] input = GameObject.FindObjectsOfType<PlayerInputController>();

            if (input == null)
                return;

            _input = input[0];
            _input.Clicked += HandleFlagPlacement;
        }
    }

    public void TakeInput(PlayerInputController input)
    {
        if (input == null)
        {
            _input = input;
            _input.Clicked += HandleFlagPlacement;
        }
    }

    private void HandleFlagPlacement(RaycastHit clickHit)
    {
        if (_isflagIsOver)
            return;

        _clickHit = clickHit;
        CreateFlag();
    }

    private void CreateFlag()
    {
        if (_isflagIsOver)
            return;

        if (_clickHit.transform.TryGetComponent(out Base selectedBase))
        {
            if (selectedBase == _base)
            {
                _onBaseSelected = true;
                _base.PlayAnimationSelected();
            }
        }
        else if (_onBaseSelected && _clickHit.transform.TryGetComponent(out Ground _))
        {
            float distance = Vector3.Distance(transform.position, _clickHit.point);
            float doubleRadiusScanning = _scanner.Radius * _radiusMultiplier;

            if (distance < doubleRadiusScanning)
            {
                _base.PlayAnimationWhrongPlace();
                return;
            }

            _flag.transform.position = _clickHit.point;
            _onBaseSelected = false;
            _base.StopAnimationSelected();
        }

        GiveOrderBuild();
    }

    private void GiveOrderBuild()
    {
        if (_director.Units.Count > 1)
            _isNeedBuildBase = true;
        else
            ActivateWaitForBuild();

        if (IsEnoughTreasureToBuild())
        {
            _isNeedBuildBase = false;
            TakeAwayTreasure();
            _unitCreating = StartCoroutine(CreateNewUnit());
            _director.BildBase(_flag);

            _isflagIsOver = true;
            _input.Clicked -= HandleFlagPlacement;
        }
        else
        {
            ActivateWaitForBuild();
        }
    }

    private void ActivateWaitForBuild()
    {
        if (_waitingTreasure != null)
            _waitingTreasure = null;

        _waitingTreasure = StartCoroutine(WaitTreasureForBildBase());
    }

    private void TakeAwayTreasure()
    {
        _counterWood.text = (Convert.ToInt32(_counterWood.text.ToString()) - _treasuresForCreateBase).ToString();
        _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateBase).ToString();
    }

    private bool IsEnoughTreasureToBuild()
    {
        if (Convert.ToInt32(_counterWood.text.ToString()) >= _treasuresForCreateBase
            && Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateBase)
            return true;
        else
            return false;
    }

    private void CreateUnit()
    {
        Unit newUnit = Instantiate(_unitPrefab, _spawnPoint.position, transform.rotation);
        newUnit.RegistredOnBase(_base);
        UnitCreated?.Invoke(newUnit);
    }

    private IEnumerator CreateNewUnit()
    {
        while (_isNeedBuildBase == false)
        {
            if (Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateUint)
            {
                CreateUnit();
                _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateUint).ToString();
            }

            yield return _wait;
        }
    }

    private IEnumerator WaitTreasureForBildBase()
    {
        yield return _wait;
        CreateFlag();
    }
}
