using System.Collections;
using UnityEngine;

public abstract class ObjectSpawner<T> : MonoBehaviour where T : SpawnerableObject
{
    [SerializeField] protected ObjectPool<T> Pool;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxItem = 10;
    [SerializeField] private int _startItem = 3;
    [SerializeField] private float _minRandomValueSpawnPoint = -10;
    [SerializeField] private float _maxRandomValueSpawnPoint = 10;

    private Coroutine _coroutine;

    private void Start()
    {
        for (int i = 0; i < _startItem; i++)
        {
            Spawn();
        }

        if (_coroutine == null)
            _coroutine = StartCoroutine(GenerateObject());
    }

    private void OnDisable()
    {
        _coroutine = null;
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

    private void Spawn()
    {
        var item = Pool.GetObject();

        Vector3 randomSpawnPosition = GetRandomSpawnPoint();
        item.transform.position = randomSpawnPosition;
        item.gameObject.SetActive(true);
    }

    private IEnumerator GenerateObject()
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while (Pool.ActiveItems <= _maxItem)
        {
            Spawn();
            yield return wait;
        }
    }
}