using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Unit : MonoBehaviour
{
    private readonly float _divideVelocityThenTakeCoin = 4;

    [Range(0,1)]
    [SerializeField] private float _volume = 0.5f;
    [SerializeField] private float _speed = 1;
    [field: SerializeField] public bool IsBusy { get; private set; }

    private bool _isOnBase = true;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Vector3 _basePosition;
    private Vector3 _target;
    private AudioSource _audio;
    private List<Resource> _resources = new();

    public event Action<List<Resource>> ResourceTakenOnBase;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audio = GetComponent<AudioSource>();
        _audio.volume = _volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Resource resource))
            TakeCoin(resource);
        else if (other.gameObject.TryGetComponent(out BaseRoboticAssemblers baseRoboticAssemblers))
            Deactivation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BaseRoboticAssemblers baseRoboticAssemblers))
            _isOnBase = false;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _target) <= 0.7f)
            GoToBase();
    }

    public void GoToTarget(Vector3 target, Vector3 basePosition)
    {
        Activation(target, basePosition);

        _agent.speed = _speed;
        _agent.destination = _target;
    }

    private void TakeCoin(Resource resource)
    {
        _resources.Add(resource);
        resource.transform.parent = transform;
        resource.TakeCoin();
        _agent.velocity = _agent.velocity/ _divideVelocityThenTakeCoin;

        if (resource.transform.position == _target)
            GoToBase(); 
    }

    private void GoToBase()
    {
        if (_isOnBase)
        {
            Deactivation();
            return;
        }

        _agent.speed = _speed;
        _agent.destination = _basePosition;
    }

    private void Activation(Vector3 target, Vector3 basePosition)
    {
        IsBusy = true;
        _target = target;
        _basePosition = basePosition;

        _animator.SetBool("IsMove", IsBusy);
    }

    private void Deactivation()
    {
        this.ResourceTakenOnBase.Invoke(_resources);
        _audio.Play();

        _agent.speed = 0;
        _resources.Clear();
        IsBusy = false;
        _isOnBase = true;
        _animator.SetBool("IsMove", IsBusy);
    }
}