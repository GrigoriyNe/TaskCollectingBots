using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Base), typeof(UnitDirector))]
public class BaseCrafter : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private TextMeshProUGUI _counterWood;
    [SerializeField] private TextMeshProUGUI _counterMetal;
    [SerializeField] private TextMeshProUGUI _counterMoney;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private FlagNewBase _flag;
    [SerializeField] private Scanner _scaner;
    [SerializeField] private int _startUnits;

    private Base _base;
    private UnitDirector _director;
    private PlayerInputController _input;
    private int _treasuresForCreateUint = 3;
    private int _treasuresForCreateBase = 5;
    private bool _isNeedBildBase = false;
    private bool _onBaseSelected = false;
    private bool _isflagIsOver;
    private Coroutine _coroutine = null;
    private WaitForSeconds _wait = new WaitForSeconds(3);

    public event Action<Unit> UnitCreated;

    private void OnEnable()
    {
        _director = this.GetComponent<UnitDirector>();
        _base = this.GetComponent<Base>();

        GameObject player = GameObject.Find("Player");

        if (player.TryGetComponent(out PlayerInputController input))
        {
            _input = input;
            input.Clicked += CreateFlag;
        }

        for (int i = 0; i < _startUnits; i++)
        {
            CreateUnit();
        }

        _coroutine = StartCoroutine(CreateNewUnit());
    }

    private void OnDisable()
    {
        _input.Clicked -= CreateFlag;
    }

    private void CreateFlag(RaycastHit createPosition)
    {
        if (_isflagIsOver)
            return;

        if (_director.Units.Count < 2)
        {
            Debug.Log("Мало юнитов, для создания новой базы");
            return;
        }

        if (createPosition.transform.TryGetComponent(out Base selectedBase))
        {
            if (selectedBase == _base)
            {
                _onBaseSelected = true;
                Debug.Log("Base Selected!");
            }
        }

        if (_onBaseSelected && createPosition.transform.TryGetComponent(out Ground _))
        {
            _flag.transform.position = createPosition.point;
            _onBaseSelected = false;
        }

        _isNeedBildBase = true;
        _coroutine = null;

        if (WillBeEnoughTreasureToBuild() && _isNeedBildBase)
        {
            _isNeedBildBase = false;
            TakeAwayTreasure();
            _coroutine = StartCoroutine(CreateNewUnit());
            _director.BildBase(_flag);

            _isflagIsOver = true;
            _input.Clicked -= CreateFlag;
        }
        else
        {
            StopCoroutine(WaitTreasureForBildBase(createPosition));
            StartCoroutine(WaitTreasureForBildBase(createPosition));
        }

    }

    private void TakeAwayTreasure()
    {
        _counterWood.text = (Convert.ToInt32(_counterWood.text.ToString()) - _treasuresForCreateBase).ToString();
        _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateBase).ToString();
    }

    private bool WillBeEnoughTreasureToBuild()
    {
        if (Convert.ToInt32(_counterWood.text.ToString()) >= _treasuresForCreateBase
            && Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateBase)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator CreateNewUnit()
    {
        while (_isNeedBildBase == false || _director.Units.Count < 1)
        {
            if (Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateUint)
            {
                CreateUnit();
                _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateUint).ToString();
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private void CreateUnit()
    {
        Unit newUnit = Instantiate(_unitPrefab, _spawnPoint.position, transform.rotation);
        newUnit.RegistredBase(_base);
        UnitCreated?.Invoke(newUnit);
    }

    private IEnumerator WaitTreasureForBildBase(RaycastHit createPosition)
    {
        yield return new WaitForSeconds(5f);
        CreateFlag(createPosition);
    }
}
