using System.Collections;
using UnityEngine;

public abstract class ObjectSpawner<T> : MonoBehaviour where T : SpawnerableObject
{
    [SerializeField] private float _delay;
    [SerializeField] private ObjectPool<T> _pool;

    private float _minRandomValueSpawnPoint = -5;
    private float _maxRandomValueSpawnPoint = 5;

    private Coroutine _coroutine;

    private void OnDisable()
    {
        _coroutine = null;
    }

    private void Start()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(GenerateObject());
    }

    public void SetPause(bool activate)
    {
        if (activate)
            _coroutine = null;
        else
            _coroutine = StartCoroutine(GenerateObject());
    }

    private void Spawn()
    {
        var item = _pool.GetObject();

        Vector3 randomSpawnPosition = GetRandomSpawnPoint();
        item.transform.position = randomSpawnPosition;
        item.gameObject.SetActive(true);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        float randomValueSpawn = Random.Range(_minRandomValueSpawnPoint, _maxRandomValueSpawnPoint);

        Vector3 randomSpawnPosition = new Vector3(transform.position.x + randomValueSpawn,
            transform.position.y,
            transform.position.z + randomValueSpawn);

        return randomSpawnPosition;
    }

    private IEnumerator GenerateObject()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            Spawn();
            yield return wait;
        }
    }
}