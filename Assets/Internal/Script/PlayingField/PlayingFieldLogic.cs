using UnityEngine;

public class PlayingFieldLogic : MonoBehaviour
{
    [SerializeField] private Transform _borderLeft;
    [SerializeField] private Transform _borderRight;

    public float BorderLeft => _borderLeft.position.x + _borderLeft.localScale.x / 2f;
    public float BorderRight => _borderRight.position.x - _borderRight.localScale.x / 2f;

    private GameObject _gameObject;

    private void Awake()
    {
        _gameObject = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dead Zone")
        {
            var poolObject = _gameObject.GetComponent<PoolObject>();
            poolObject.ReturnToPool();
        }
    }
}
