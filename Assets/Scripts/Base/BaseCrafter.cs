using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class BaseCrafter : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Base _basePrefab;
    [SerializeField] private TextMeshProUGUI _counterWood;
    [SerializeField] private TextMeshProUGUI _counterMetal;
    [SerializeField] private TextMeshProUGUI _counterMoney;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private FlagNewBase _flag;
    [SerializeField] private UnitDirector _director;
    [SerializeField] private Scanner _scaner;
    [SerializeField] private int _startUnits;

    private PlayerInputController _input;
    private int _treasuresForCreateUint = 3;
    private int _treasuresForCreateBase = 5;
    private bool _isNeedBildBase = false;
    private bool _isflagIsOver;
    private Coroutine _coroutine = null;
    private WaitForSeconds _wait = new WaitForSeconds (3);

    public event Action<Unit> UnitCreated;

    private void OnEnable()
    {
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

    private void CreateFlag(RaycastHit createPosition)
    {
        if (_isflagIsOver)
            return;

        if (_director.Units.Count < 1)
        {
            StartCoroutine(WaitNewUnitForBild(createPosition));
            return;
        }

        if (createPosition.transform.TryGetComponent(out Base selectedBase))
        {
            if (selectedBase == this.GetComponent<Base>())
            {
                _isNeedBildBase = true;
                Debug.Log("Base Selected!");
            }
        }

        if (_isNeedBildBase)
        {
            if (createPosition.transform.TryGetComponent(out Ground ground))
            {
                _flag.transform.position = createPosition.point;
            }

        }

        //if (transform.position.x - createPosition.transform.position.x < _scaner.Radius / 2)
        //{
        //    Debug.Log("Too close");
        //    return;
        //}

        _coroutine = null;


        if (Convert.ToInt32(_counterWood.text.ToString()) >= _treasuresForCreateBase
            && Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateBase)
         //   && Convert.ToInt32(_counterMoney.text.ToString()) >= _treasuresForCreateBase)
        {
            _director.BildBase(_flag);
            _isNeedBildBase = false;

            _counterWood.text = (Convert.ToInt32(_counterWood.text.ToString()) - _treasuresForCreateBase).ToString();
            _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateBase).ToString();
      //      _counterMoney.text = (Convert.ToInt32(_counterMoney.text.ToString()) - _treasuresForCreateBase).ToString();

            _isflagIsOver = true;
            // _input.Clicked -= CreateFlag;
        }
        else
        {
            StartCoroutine(WaitTreasureForBildBase(createPosition));
        }
    }

    private IEnumerator WaitNewUnitForBild(RaycastHit createPosition)
    {
        yield return _wait;

        CreateFlag(createPosition);
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

    private IEnumerator WaitTreasureForBildBase(RaycastHit createPosition)
    {
        yield return new WaitForSeconds(5f);

        CreateFlag(createPosition);
    }
}
