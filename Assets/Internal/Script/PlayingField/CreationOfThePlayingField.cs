using UnityEngine;

public class CreationOfThePlayingField : MonoBehaviour
{
    [SerializeField] private int _maximumNumberOfGamingPlatforms;

    [SerializeField] private Pool _pool;

    private Vector3 _creationPosition;

    private void Start()
    {
        _creationPosition = new Vector3(0, 0, -10f);

        for (int i = 0; i < _maximumNumberOfGamingPlatforms; i++)
        {
            _pool.GetFreeElement(_creationPosition);
            _creationPosition += new Vector3(0, 0, 10f);
        }
    }

    private void Update()
    {
        if (_pool.AreThereAnyActiveElements())
        {
            _pool.GetFreeElement(_creationPosition);
            _creationPosition += new Vector3(0, 0, 10f);
        }
    }
}
