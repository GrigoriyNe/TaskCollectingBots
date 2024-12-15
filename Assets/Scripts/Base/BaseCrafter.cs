using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BaseCrafter : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _counterWood;
    [SerializeField] private TextMeshProUGUI _counterMetal;
    [SerializeField] private TextMeshProUGUI _counterMoney;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PlayerInputController _input;
    [SerializeField] private FlagNewBase _flag;
    [SerializeField] private UnitDirector _director;
    [SerializeField] private int _startUnits;

    private int _treasuresForCreateUint = 3;
    private int _treasuresForCreateBase = 5;
    private bool _isNeedBildBase = false;

    public event Action<Unit> UnitCreated;

    private void Awake()
    {
        for (int i = 0; i < _startUnits; i++)
        {
            CreateUnit();
        }
    }

    
    private void Start()
    {
        StartCoroutine(CreateNewUnit());
        _input.Clicked += CreateFlag;
    }

    private void OnDisable()
    {
        _input.Clicked -= CreateFlag;
    }

    private void CreateFlag(Vector3 createPosition)
    {
        _isNeedBildBase = true;

        if (_flag.gameObject.activeSelf == false)
        {
            _flag.gameObject.SetActive(true);
            _flag.transform.position = createPosition;
        }

        if (Convert.ToInt32(_counterWood.text.ToString()) >= _treasuresForCreateBase
            && Convert.ToInt32(_counterMetal.text.ToString()) >= _treasuresForCreateBase
            && Convert.ToInt32(_counterMoney.text.ToString()) >= _treasuresForCreateBase)
        {
            _director.BildBase(_flag);
            _isNeedBildBase = false;
            StartCoroutine(CreateNewUnit());

            _counterWood.text = (Convert.ToInt32(_counterWood.text.ToString()) - _treasuresForCreateBase).ToString();
            _counterMetal.text = (Convert.ToInt32(_counterMetal.text.ToString()) - _treasuresForCreateBase).ToString();
            _counterMoney.text = (Convert.ToInt32(_counterMoney.text.ToString()) - _treasuresForCreateBase).ToString();
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
            if (Convert.ToInt32(_counterWood.text.ToString()) >= _treasuresForCreateUint)
            {
                CreateUnit();
                _counterWood.text = (Convert.ToInt32(_counterWood.text.ToString()) - _treasuresForCreateUint).ToString();
                
            }

            yield return new WaitForSeconds(.5f);

        }
    }

    private void CreateUnit()
    {
        Unit newUnit = Instantiate(_unitPrefab, _spawnPoint.position, transform.rotation);
        newUnit.RegistredBase(_base);
        UnitCreated?.Invoke(newUnit);

    }

    private IEnumerator WaitTreasureForBase()
    {
        yield return new WaitForSeconds(5f);

        CreateFlag(_flag.transform.position);
    }
}
