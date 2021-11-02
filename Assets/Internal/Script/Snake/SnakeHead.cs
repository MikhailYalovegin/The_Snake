using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public delegate void OnCoinTake();
    public event OnCoinTake onCoinTake;

    public delegate void OnNewColor(Color color);
    public event OnNewColor onNewColor;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NewColorWall")
        {
            Color color = other.GetComponent<Renderer>().material.color;
            onNewColor?.Invoke(color);
        }

        if (other.tag == "Food")
        {
            onCoinTake?.Invoke();
        }

        if (other.tag == "Crystal")
        {
            Debug.Log("Crystal");
        }

        if (other.tag == "Hindrance")
        {
            Debug.Log("Hindrance");
        }
    }


}
