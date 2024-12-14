using System.Collections;
using UnityEngine;

public abstract class ObjectSpawner<T> : MonoBehaviour where T : SpawnableObject
{
    [SerializeField] protected ObjectPool<T> Pool;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxItem = 10;
    [SerializeField] private int _startItem = 3;
    [SerializeField] private float _minRandomValueSpawnPoint = -10;
    [SerializeField] private float _maxRandomValueSpawnPoint = 10;
    [SerializeField] private Transform _container;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;
    private int _activeItems;

    private void Start()
    {
        _wait = new WaitForSeconds(_delay);

        for (int i = 0; i < _startItem; i++)
        {
            Spawn();
        }

        if (_coroutine == null)
            _coroutine = StartCoroutine(GenerateItem());
    }

    private void OnDisable()
    {
        _coroutine = null;
    }

    public void PutItem(SpawnableObject item)
    {
        item.transform.SetParent(_container);
        item.Returned -= PutItem;
        Pool.ReturnItem(item as T);
        _activeItems--;
    }

    private void Spawn()
    {
        var item = Pool.GetItem();

        Vector3 randomSpawnPosition = GetRandomSpawnPoint();
        item.transform.position = randomSpawnPosition;
        item.gameObject.SetActive(true);
        item.Returned += PutItem;
        _activeItems++;
    }

    
    private Vector3 GetRandomSpawnPoint()
    {
        float randomValueSpawnX = Random.Range(_minRandomValueSpawnPoint, _maxRandomValueSpawnPoint);
        float randomValueSpawnY = Random.Range(_minRandomValueSpawnPoint, _maxRandomValueSpawnPoint);

        Vector3 randomSpawnPosition = new Vector3(transform.position.x + randomValueSpawnX,
            transform.position.y,
            transform.position.z + randomValueSpawnY);

        return randomSpawnPosition;
    }

    private IEnumerator GenerateItem()
    {
        while (_activeItems <= _maxItem)
        {
            Spawn();
            yield return _wait;
        }
    }
}