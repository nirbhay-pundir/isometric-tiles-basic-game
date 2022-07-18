using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UpdateUI : MonoBehaviour
{
    public static Vector2Int mousePos;
    public static bool movable;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI movableText;
    Color color;
    private void Awake()
    {
        color = positionText.color;
    }
    private void Update()
    {
        if (!Player.moving)
        {
            if (movable)
            {
                movableText.SetText("Movable");
                movableText.color = color;
                positionText.SetText("x: " + mousePos.x.ToString() + "\t     y: " + mousePos.y.ToString());
                positionText.color = color;
            }
            else
            {
                movableText.SetText("Not Movable");
                movableText.color = Color.red;
                positionText.SetText("x: " + mousePos.x.ToString() + "\t     y: " + mousePos.y.ToString());
                positionText.color = Color.red;
            }
        }
        else
        {
            movableText.SetText("Running");
            movableText.color = color;
            positionText.SetText("x: " + mousePos.x.ToString() + "\t     y: " + mousePos.y.ToString());
            positionText.color = color;
        }
    }
}
