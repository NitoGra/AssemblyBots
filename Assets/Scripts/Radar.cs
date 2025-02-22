using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Radar
{
    [SerializeField] private float _radius = 4;
    [SerializeField] private float _checkDelay = 1;

    [field: SerializeField] public LayerMask TargetMask { get; private set; }

    public event Action<Resource> ResourceFound;

    public IEnumerator CheckInRadius(Vector3 radarPosition)
    {
        WaitForSeconds wait = new(_checkDelay);

        while (true)
        {
            foreach (Collider target in Physics.OverlapSphere(radarPosition, _radius, TargetMask))
            {
                Resource resource = target.GetComponent<Resource>();

                if (resource.IsTaken == false)
                    ResourceFound.Invoke(resource);
            }

            yield return wait;
        }
    }

    public void OnDrawGizmos(Vector3 posistion) => Gizmos.DrawWireSphere(posistion, _radius);
}