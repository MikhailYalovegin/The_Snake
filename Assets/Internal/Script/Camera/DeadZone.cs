using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider _other)
    {
        _other.transform.parent.GetComponent<PoolObject>().ReturnToPool();
    }
}
