using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalText : MonoBehaviour
{
    [SerializeField] private SnakeHead _snakeHead;
    [SerializeField] private Text _textMesh;
    void Start()
    {
        _snakeHead._onCollectingCrystals += _snakeHeadTetCrystals;
    }

    private void _snakeHeadTetCrystals(int _numberCrystals)
    {
        _textMesh.text = "Кристалы: " + _numberCrystals;
    }
}
