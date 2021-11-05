using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] private SnakeController _snakeController;

    public delegate void OnCoinTake(Color _colorSnake, Vector3 _positionSnake);
    public event OnCoinTake _onCoinTake;

    public delegate void OnNewColor(Color _newColorSnake);
    public event OnNewColor _onNewColor;

    public delegate void OnCollectingCrystals(int _numberCrystals);
    public event OnCollectingCrystals _onCollectingCrystals;

    public delegate void OnLevelBuilding();
    public event OnLevelBuilding _onLevelBuilding;

    public delegate void OnFever();
    public event OnFever _onFever;

    private int _numberFeverCrystal;
    private int _numberCrystal;

    private bool _checFever;

    private void Awake()
    {
        _checFever = true;
        _numberCrystal = 0;
        _numberFeverCrystal = 0;
    }

    private void Start()
    {
        _snakeController._onEndFever += _snakeController__onEndFever;
    }

    private void _snakeController__onEndFever()
    {
        _checFever = true;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_checFever)
        {
            if (_other.tag == "ColoredWall")
            {
                Color _newColorSnake = _other.GetComponent<Renderer>().material.color;
                _onNewColor?.Invoke(_newColorSnake);
                _onLevelBuilding?.Invoke();
            }

            if (_other.tag == "Food")
            {
                _numberFeverCrystal = 0;
                if (_other.gameObject.GetComponent<Renderer>().material.color != GetComponent<Renderer>().material.color && _numberFeverCrystal != 3)
                {
                    Restart.RestartLevel();
                }
                _other.transform.parent.GetComponent<PoolObject>().ReturnToPool();
                Color _colorSnake = GetComponent<Renderer>().material.color;
                Vector3 _positionSnake = transform.position;
                _onCoinTake?.Invoke(_colorSnake, _positionSnake);
            }

            if (_other.tag == "Crystal")
            {
                _numberCrystal++;
                _numberFeverCrystal++;

                _other.transform.parent.GetComponent<PoolObject>().ReturnToPool();

                if (_numberFeverCrystal == 3)
                {
                    _onFever?.Invoke();
                    _numberFeverCrystal = 0;
                    _checFever = false;
                }
                _onCollectingCrystals?.Invoke(_numberCrystal);
            }

            if (_other.tag == "Hindrance" && _numberFeverCrystal != 3)
            {
                Restart.RestartLevel();
            }
        }
        else
        {
            if (_other.tag == "Hindrance" || _other.tag == "Crystal")
            {
                Debug.Log("2");
                _numberCrystal++;
                _other.transform.parent.GetComponent<PoolObject>().ReturnToPool();
            }
            if (_other.tag == "Food")
            {
                _other.transform.parent.GetComponent<PoolObject>().ReturnToPool();
                Color _colorSnake = GetComponent<Renderer>().material.color;
                Vector3 _positionSnake = transform.position;
                _onCoinTake?.Invoke(_colorSnake, _positionSnake);
            }
            if (_other.tag == "ColoredWall")
            {
                Color _newColorSnake = _other.GetComponent<Renderer>().material.color;
                _onNewColor?.Invoke(_newColorSnake);
                _onLevelBuilding?.Invoke();
            }
        }
    }
}
