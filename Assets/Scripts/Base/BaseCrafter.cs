using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BaseCrafter : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private TextMeshProUGUI _counterWood;
    [SerializeField] private TextMeshProUGUI _counterMetal;
    [SerializeField] private TextMeshProUGUI _counterMoney;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PlayerInputController _input;
    [SerializeField] private FlagNewBase _flag;
    [SerializeField] private UnitDirector _director;
    [SerializeField] private Scanner _scaner;
    [SerializeField] private int _startUnits;

    private int _treasuresForCreateUint = 3;
    private int _treasuresForCreateBase = 5;
    private bool _isNeedBildBase = false;
    private bool _isflagIsOver;
    private Coroutine _coroutine = null;

    public event Action<Unit> UnitCreated;

    private void OnEnable()
    {
        _isflagIsOver = false;
        _input.Clicked += CreateFlag;

        for (int i = 0; i < _startUnits; i++)
        {
            CreateUnit();
        }
    }
    private void OnDisable()
    {
        _input.Clicked -= CreateFlag;
    }

    private void Update()
    {
        if (_isNeedBildBase == false && _coroutine == null)
            _coroutine = StartCoroutine(CreateNewUnit());
    }

    private void CreateFlag(Vector3 createPosition)
    {
        if (_isflagIsOver)
            return;

        //if (_base.transform.position.x - createPosition.x < _scaner.Radius * 2)
        //{
        //    Debug.Log("Too close");
        //    return;
        //}

        _isNeedBildBase = true;
        _coroutine = null;
        _flag.transform.position = createPosition;

        if (Convert.ToInt32(_counterWood.text.ToString()) >= _treasuresForCreateBase
            && Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateBase
            && Convert.ToInt32(_counterMoney.text.ToString()) >= _treasuresForCreateBase)
        {
            _director.BildBase(_flag);
            _isNeedBildBase = false;

            _counterWood.text = (Convert.ToInt32(_counterWood.text.ToString()) - _treasuresForCreateBase).ToString();
            _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateBase).ToString();
            _counterMoney.text = (Convert.ToInt32(_counterMoney.text.ToString()) - _treasuresForCreateBase).ToString();

            _isflagIsOver = true;
            _flag.gameObject.SetActive(false);
            //_input.Clicked -= CreateFlag;
        }
        else
        {
            StartCoroutine(WaitTreasureForBase());
        }
    }

    private IEnumerator CreateNewUnit()
    {
        while (_isNeedBildBase == false)
        {
            if (Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateUint)
            {
                CreateUnit();
                _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateUint).ToString();
            }

            yield return null;
        }
    }

    private void CreateUnit()
    {
        Unit newUnit = Instantiate(_unitPrefab, _spawnPoint.position, transform.rotation);
        newUnit.RegistredBase(_basePrefab);
        UnitCreated?.Invoke(newUnit);
    }

    private IEnumerator WaitTreasureForBase()
    {
        yield return new WaitForSeconds(5f);

        CreateFlag(_flag.transform.position);
    }
}
