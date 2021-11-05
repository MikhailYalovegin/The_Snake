using UnityEngine;

public class PlayingFieldTransform : MonoBehaviour
{
    [SerializeField] private Transform _borderLeft;
    [SerializeField] private Transform _borderRight;

    public float BorderLeft => _borderLeft.position.x + _borderLeft.localScale.x / 2f;
    public float BorderRight => _borderRight.position.x - _borderRight.localScale.x / 2f;
}
