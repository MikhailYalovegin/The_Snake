using System.Collections;
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
    [SerializeField] private int _lengthTailAtStart = 5;
    [Space]
    [SerializeField] private PlayingFieldTransform _playingFieldTransform;
    [Space]
    [SerializeField] private float _timeFever = 1f;

    public delegate void OnEndFever();
    public event OnEndFever _onEndFever;

    private List<GameObject> _snakeTorso;
    private Rigidbody _rigidbodyPlayer;
    private float _horizontal;
    private float _timeFeverNew;
    private float _speedSnake;

    private void Awake()
    {
        _speedSnake = _speedSnakeHead;
        _timeFeverNew = _timeFever;
        _snakeTorso = new List<GameObject>();
        _rigidbodyPlayer = _snakeHead.GetComponent<Rigidbody>();
        _horizontal = 0f;

        Color _colorSnake = _snakeHead.GetComponent<Renderer>().material.color;

        for (int i = 0; i < _lengthTailAtStart; i++)
        {
            NewSnakeElemenTailt(_colorSnake, _rigidbodyPlayer.transform.position);
        }
    }

    private void Start()
    {
        _snakeHead.GetComponent<SnakeHead>()._onCoinTake += NewSnakeElemenTailt;
        _snakeHead.GetComponent<SnakeHead>()._onNewColor += NewSnakeColor;
        _snakeHead.GetComponent<SnakeHead>()._onFever += SnakeControllerOnFever;
    }

    private void Update()
    {
        if (_timeFeverNew < _timeFever)
        {
            _timeFeverNew += Time.deltaTime;
        }
        else
        {
            _speedSnake = _speedSnakeHead;
            _onEndFever?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        MoveSnakeHead();
        MoveSnakeTail();
    }

    private void SnakeControllerOnFever()
    {
        _timeFeverNew = 0;
        _speedSnake *= 3;
    }

    private void MoveSnakeHead()
    {
        _horizontal = _joystick.Horizontal;

        Vector3 _direction = Vector3.forward;
        Quaternion _deltaRotation;

        if (_horizontal < 0 && _playingFieldTransform.BorderLeft < _rigidbodyPlayer.transform.position.x - _rigidbodyPlayer.transform.localScale.x / 2f)
        {
            _deltaRotation = Quaternion.Euler(new Vector3(0f, -_angleRotationSnake, 0f));
        }
        else if (_horizontal > 0 && _playingFieldTransform.BorderRight > _rigidbodyPlayer.transform.position.x + _rigidbodyPlayer.transform.localScale.x / 2f)
        {
            _deltaRotation = Quaternion.Euler(new Vector3(0f, _angleRotationSnake, 0f));
        }
        else
        {
            _deltaRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        _direction = _deltaRotation * _direction;

        Vector3 _movePositionPlayer = Vector3.Lerp(_rigidbodyPlayer.position, _rigidbodyPlayer.position + _direction, Time.fixedDeltaTime * _speedSnake);

        _rigidbodyPlayer.MovePosition(_movePositionPlayer);
    }

    private void MoveSnakeTail()
    {
        for (int i = 0; i < _snakeTorso.Count; i++)
        {
            Vector3 _velocity = Vector3.zero;

            Vector3 _destination;

            if (i == 0) _destination = _rigidbodyPlayer.position;

            else _destination = _snakeTorso[i - 1].transform.position;

            _snakeTorso[i].GetComponent<Rigidbody>().position = Vector3.SmoothDamp(_snakeTorso[i].transform.position, _destination, ref _velocity, 0.15f / _speedSnake, 100f, Time.fixedDeltaTime);
        }
    }

    private void NewSnakeElemenTailt(Color _colorSnake, Vector3 _positionSnake)
    {
        if (_snakeTorso.Count >= 1)
        {
            _positionSnake = _snakeTorso[_snakeTorso.Count - 1].transform.position;
        }
        GameObject _elemenTailt = Instantiate(_prefabSnakeTail, _positionSnake, transform.rotation, transform);
        _elemenTailt.GetComponent<Renderer>().material.color = _colorSnake;

        _snakeTorso.Add(_elemenTailt);

        if (_lengthTailAtStart < _snakeTorso.Count)
        {
            StartCoroutine(ScaleSnakeElemenTailt());
        }
    }

    private void NewSnakeColor(Color _newSnake)
    {
        _snakeHead.GetComponent<Renderer>().material.color = _newSnake;

        for (int i = 0; i < _snakeTorso.Count; i++)
        {
            _snakeTorso[i].GetComponent<Renderer>().material.color = _newSnake;
        }
    }

    private IEnumerator ScaleSnakeElemenTailt()
    {
        List<GameObject> list = new List<GameObject>();
        foreach (var item in _snakeTorso)
        {
            list.Add(item);
        }

        foreach (var _item in list)
        {
            _item.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);

            yield return new WaitForSeconds(0.125f);

            _item.transform.localScale = _prefabSnakeTail.transform.localScale;
        }
    }
}
