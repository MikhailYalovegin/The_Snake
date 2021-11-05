using System.Collections.Generic;
using UnityEngine;

public class LogicCreatingGameObjects : MonoBehaviour
{
    [Space]
    [SerializeField] private GameObject _snakeHead;
    [Space]
    [SerializeField] private List<Material> _logicGameMaterial;
    [Space]
    [SerializeField] private Pool _coloredWall;
    [Space]
    [SerializeField] private Pool _crystal;
    [Space]
    [SerializeField] private Pool _food;
    [Space]
    [SerializeField] private Pool _hindrance;
    [Space]
    [SerializeField] private float _distanceBetweenColoredWall = 30f;
    [Space]
    [SerializeField] private int _numberOfGameObjectsInOneArea = 6;
    [Space]
    [SerializeField] private PlayingFieldTransform _playingFieldTransform;
    private float _distanceBetweenGameObjects;
    private Vector3 _creationPositionColoredWall;
    private Vector3 _creationPositionGameObjects;
    private Vector3 _positionStartNewColor;
    private int _numberColorInList;
    private int _numberColorForBilding;
    private List<float> _positionColoredWall;

    private void Awake()
    {
        _positionColoredWall = new List<float>();

        _numberColorInList = 0;
        _numberColorForBilding = 0;

        _distanceBetweenGameObjects = _distanceBetweenColoredWall / _numberOfGameObjectsInOneArea;
        _creationPositionColoredWall = Vector3.forward;
        _positionStartNewColor = _creationPositionColoredWall;
        _creationPositionGameObjects = Vector3.forward;
    }

    private void Start()
    {
        _snakeHead.GetComponent<SnakeHead>()._onLevelBuilding += LogicCreatingNewGameObjects;
        for (int i = 0; i <= 3; i++)
        {
            BuildingColoredWall();
            LogicCreatingNewGameObjects();
        }
    }

    private void LogicCreatingNewGameObjects()
    {
        BuildingColoredWall();
        for (int i = 0; i < _numberOfGameObjectsInOneArea; i++)
        {
            ArrangementGameElements();
        }
    }

    private void BuildingColoredWall()
    {
        Vector3 _posColoredWall = _creationPositionColoredWall;
        if (_numberColorInList >= _logicGameMaterial.Count)
        {
            _numberColorInList = 0;
        }
        Color _color = _logicGameMaterial[_numberColorInList].color;
        _numberColorInList++;
        _coloredWall.GetFreeElement(_posColoredWall, _color);
        _positionColoredWall.Add(_creationPositionColoredWall.z);
        _creationPositionColoredWall += Vector3.forward * _distanceBetweenColoredWall;
    }

    private void ArrangementGameElements()
    {
        Vector3 _posGameObjects = _creationPositionGameObjects;
        foreach (var item in _positionColoredWall)
        {
            if (_posGameObjects.z > item - 2f && _posGameObjects.z < item + 2f)
            {
                _creationPositionGameObjects += Vector3.forward * _distanceBetweenGameObjects;
                _posGameObjects = _creationPositionGameObjects;
            }
        }

        if (Random.Range(0, 3) != 0)
        {
            int _randomPos = (Random.Range(0, 2) != 0) ? 1 : -1;
            _posGameObjects = new Vector3(_playingFieldTransform.BorderLeft * 0.55f * _randomPos, _posGameObjects.y, _posGameObjects.z);

            Color _colorFood = CurrentFoodColor(_posGameObjects);
            _food.GetFreeElement(_posGameObjects, _colorFood);

            _posGameObjects = new Vector3(_playingFieldTransform.BorderRight * 0.55f * _randomPos, _posGameObjects.y, _posGameObjects.z);

            _colorFood = _logicGameMaterial[Random.Range(0, _logicGameMaterial.Count)].color;
            _food.GetFreeElement(_posGameObjects, _colorFood);
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                _posGameObjects = new Vector3(_playingFieldTransform.BorderLeft * 0.66f, _posGameObjects.y, _posGameObjects.z);
                _crystal.GetFreeElement(_posGameObjects);

                _posGameObjects = new Vector3(0, _posGameObjects.y, _posGameObjects.z);
                _hindrance.GetFreeElement(_posGameObjects);

                _posGameObjects = new Vector3(_playingFieldTransform.BorderRight * 0.66f, _posGameObjects.y, _posGameObjects.z);
                _crystal.GetFreeElement(_posGameObjects);
            }
            else
            {
                _posGameObjects = new Vector3(_playingFieldTransform.BorderLeft * 0.66f, _posGameObjects.y, _posGameObjects.z);
                _hindrance.GetFreeElement(_posGameObjects);

                _posGameObjects = new Vector3(0, _posGameObjects.y, _posGameObjects.z);
                _crystal.GetFreeElement(_posGameObjects);

                _posGameObjects = new Vector3(_playingFieldTransform.BorderRight * 0.6f, _posGameObjects.y, _posGameObjects.z);
                _hindrance.GetFreeElement(_posGameObjects);
            }
        }

        _creationPositionGameObjects += Vector3.forward * _distanceBetweenGameObjects;
    }

    private Color CurrentFoodColor(Vector3 _posFood)
    {
        Vector3 _positionEndNewColor = _positionStartNewColor + Vector3.forward * _distanceBetweenColoredWall;

        if (_posFood.z > _positionEndNewColor.z)
        {
            _positionStartNewColor = _positionEndNewColor;
            _positionEndNewColor = _positionStartNewColor + Vector3.forward * _distanceBetweenColoredWall;

            _numberColorForBilding++;
            if (_numberColorForBilding >= _logicGameMaterial.Count) _numberColorForBilding = 0;
        }
        return _logicGameMaterial[_numberColorForBilding].color;
    }
}
