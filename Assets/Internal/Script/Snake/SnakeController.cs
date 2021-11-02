using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private GameObject _snakeHead;
    [SerializeField] private float _speedSnakeHead = 1f;
    [Space]
    [SerializeField] private Joystick _joystick;
    [SerializeField] private float _angleRotationSnake = 80f;
    [Space]
    [SerializeField] private GameObject _prefabSnakeTail;
    [SerializeField] private int _lengthTailAtStart = 2;
    [Space]
    [SerializeField] private PlayingFieldLogic _playingFieldLogic;

    private List<GameObject> _snakeTorso;
    private Rigidbody _rigidbodyPlayer;
    private float _horizontal;

    private void Awake()
    {
        _snakeTorso = new List<GameObject>();
        _rigidbodyPlayer = _snakeHead.GetComponent<Rigidbody>();
        _horizontal = 0f;

        for (int i = 0; i < _lengthTailAtStart; i++)
        {
            NewSnakeElemenTailt();
        }
    }

    private void Start()
    {
        _snakeHead.GetComponent<SnakeHead>().onCoinTake += NewSnakeElemenTailt;
        _snakeHead.GetComponent<SnakeHead>().onNewColor += NewSnakeColor;
    }

    private void FixedUpdate()
    {
        MoveSnakeHead();
        MoveSnakeTail();
    }

    private void MoveSnakeHead()
    {
        _horizontal = _joystick.Horizontal;

        Vector3 _direction = Vector3.forward;
        Quaternion _deltaRotation;

        if (_horizontal < 0 && _playingFieldLogic.BorderLeft < _rigidbodyPlayer.transform.position.x - _rigidbodyPlayer.transform.localScale.x / 2f)
        {
            _deltaRotation = Quaternion.Euler(new Vector3(0f, -_angleRotationSnake, 0f));
        }
        else if (_horizontal > 0 && _playingFieldLogic.BorderRight > _rigidbodyPlayer.transform.position.x + _rigidbodyPlayer.transform.localScale.x / 2f)
        {
            _deltaRotation = Quaternion.Euler(new Vector3(0f, _angleRotationSnake, 0f));
        }
        else
        {
            _deltaRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        _direction = _deltaRotation * _direction;

        Vector3 _movePositionPlayer = Vector3.Lerp(_rigidbodyPlayer.position, _rigidbodyPlayer.position + _direction, Time.fixedDeltaTime * _speedSnakeHead);

        _rigidbodyPlayer.MovePosition(_movePositionPlayer);
    }

    private void MoveSnakeTail()
    {
        for (int i = 0; i < _snakeTorso.Count; i++)
        {
            Vector3 _velocity = Vector3.zero;

            Vector3 _destination;

            if (i == 0)
            {
                _destination = _rigidbodyPlayer.position;
            }
            else
            {
                _destination = _snakeTorso[i - 1].transform.position;
            }

            _snakeTorso[i].GetComponent<Rigidbody>().position = Vector3.SmoothDamp(_snakeTorso[i].transform.position, _destination, ref _velocity, 0.15f / _speedSnakeHead, 100f, Time.fixedDeltaTime);
        }
    }

    private void NewSnakeElemenTailt()
    {
        _snakeTorso.Add(Instantiate(_prefabSnakeTail, transform.position, transform.rotation));
    }

    private void NewSnakeColor(Color color)
    {
        _snakeHead.GetComponent<Renderer>().material.color = color;

        for (int i = 0; i < _snakeTorso.Count; i++)
        {
            _snakeTorso[i].GetComponent<Renderer>().material.color = color;
        }
    }
}
