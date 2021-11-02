using UnityEngine;

public class CreationOfThePlayingField : MonoBehaviour
{
    [SerializeField] private int _maximumNumberOfGamingPlatforms;

    [SerializeField] private Pool _pool;

    [SerializeField] private GameObject _pefabNewColorWall;




    private Vector3 _creationPosition;

    private void Start()
    {
        _creationPosition = new Vector3(0, 0, -10f);

        for (int i = 0; i < _maximumNumberOfGamingPlatforms; i++)
        {
            _pool.GetFreeElement(_creationPosition);
            _creationPosition += new Vector3(0, 0, 10f);
        }
        GameObject gameObject = Instantiate(_pefabNewColorWall, new Vector3(0, 0, 10f), _pefabNewColorWall.transform.rotation);
         gameObject.GetComponentInChildren<Renderer>().material.color = Color.black;
        // gameObject.GetComponent<Material>().color = Color.black;
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
