using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    [field: SerializeField] public bool IsTaken { get; private set; } = false;
    [field: SerializeField] public int Value { get; private set; } = 1;

    private Rigidbody _rigidbody;
    private float _delayToTryTakeAgain = 6;
    private WaitForSeconds _wait;

    public Vector3 GetPosition()
    {
        IsTaken = true;
        return transform.position;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _wait = new (_delayToTryTakeAgain);
    }

    public void TakeCoin()
    {
        _rigidbody.isKinematic = true;
        IsTaken = true;
    }
  
    public void DeleteCoin()
    {
        Value = 0;
        gameObject.SetActive(false);
    }

    private IEnumerator TimerToTryTakeAgain()
    {
        yield return _wait;

        if (_rigidbody.isKinematic == false)
            IsTaken = false;
    }
}