using System;
using UnityEngine;

[Serializable]
public class RandomPositionInArea
{
    [SerializeField] private Transform _spawnZone;

    private Transform _transform;

    public Vector3 MakeRandomPositionInFlatArea(Transform Transform)
    {
        _transform = Transform;

        Vector3 randomPos = new(
            UnityEngine.Random.Range(-_spawnZone.position.x, _spawnZone.position.x),
            _transform.position.y,
            UnityEngine.Random.Range(-_spawnZone.position.z, _spawnZone.position.z));

        return randomPos;
    }

    public void OnDrawGizmos(Vector3 posistion) => Gizmos.DrawCube(posistion, new Vector3(_spawnZone.position.x * 2, posistion.y, _spawnZone.position.z * 2));
}