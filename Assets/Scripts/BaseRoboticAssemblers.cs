using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseRoboticAssemblers : MonoBehaviour
{
    [SerializeField] private Radar _radar = new();
    [SerializeField] private TextMeshProUGUI _display;
    [SerializeField] private int _unitsStartCount;
    [SerializeField] private Unit _unitsPrefab;
    [SerializeField] private List<Unit> _units = new();

    private List<Resource> _foundedResources = new();
    private int _collectedResources = 0;

    private void OnEnable()
    {
        _radar.ResourceFound += ResourceFounded;
    }

    private void OnDisable()
    {
        _radar.ResourceFound -= ResourceFounded;

        foreach (Unit unit in _units)
            unit.ResourceTakenOnBase -= IncreaseAndDisplayRecource;
    }

    private void Start()
    {
        for (int i = 0; i < _unitsStartCount; i++)
            SpawnNewUnit();

        StartCoroutine(_radar.CheckInRadius(transform.position));
        DisplayRecource();
    }

    private void IncreaseRecource(int value) => _collectedResources += value;

    private void DisplayRecource() => _display.text = $"{_collectedResources}";

    private void SpawnNewUnit()
    {
        Unit unit = Instantiate(_unitsPrefab);
        unit.transform.parent = transform;
        unit.transform.position = transform.position;
        unit.ResourceTakenOnBase += IncreaseAndDisplayRecource;
        _units.Add(unit);
    }

    private void ResourceFounded(Resource resource)
    {
        RecordTarget(resource);

        for (int i = 0; i < _foundedResources.Count; i++)
            if (_foundedResources[i].IsTaken == false)
                SendingUnit(_foundedResources[i]);
    }

    private void RecordTarget(Resource resource)
    {
        if (_foundedResources.Contains(resource) == false)
            _foundedResources.Add(resource);
    }

    private void SendingUnit(Resource resource)
    {
        foreach (Unit unit in _units)
        {
            if (unit.IsBusy == false)
            {
                unit.GoToTarget(resource.GetPosition(), transform.position);
                return;
            }
        }
    }

    private void IncreaseAndDisplayRecource(List<Resource> resources)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            IncreaseRecource(resources[i].Value);
            resources[i].DeleteCoin();
        }

        DisplayRecource();
    }

    public void OnDrawGizmos()
    {
        _radar.OnDrawGizmos(transform.localPosition);
    }
}