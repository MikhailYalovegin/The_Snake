using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform _plaerTransform;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Vector3 _offSet;

    private void FixedUpdate()
    {
        Vector3 _followPos = _plaerTransform.position + _offSet;

        var _newPositionCamer = new Vector3(_cameraTransform.position.x, _cameraTransform.position.y, _followPos.z);

        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _newPositionCamer, Time.fixedDeltaTime);
    }
}
