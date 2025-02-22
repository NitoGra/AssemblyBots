using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private RandomPositionInArea _randomPosition = new();
    [SerializeField] private Transform _resourcePrefab;
    [SerializeField] private int _spawnCount = 2;
    [SerializeField] private float _spawnDelay = 3;

    private void Start()
    {
        StartCoroutine(SpawnOnDelay());
    }

    private IEnumerator SpawnOnDelay()
    {
        WaitForSeconds wait = new(_spawnDelay);
        yield return wait;

        while (true)
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                Transform resource = Instantiate(_resourcePrefab);
                resource.transform.parent = transform;
                resource.transform.position = _randomPosition.MakeRandomPositionInFlatArea(transform);
            }

            yield return wait;
        }
    }

    public void OnDrawGizmos()
    {
        _randomPosition.OnDrawGizmos(transform.localPosition);
    }
}