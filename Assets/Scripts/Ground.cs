using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ground : MonoBehaviour
{
    new Renderer renderer;

    public Vector2Int PosOnGrid;
    private Color color = new(0.000f, 0.547f, 0.156f, 1);
    public bool isMovable;

    public int gCost;
    public int hCost;
    public int fCost;

    public Ground cameFromGround;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        UpdateUI.mousePos = PosOnGrid;
        UpdateUI.movable = isMovable;
        renderer.material.color = Color.red;
        if (isMovable && !Player.moving)
        {
            GroundGenerator.CalculatePath(transform.position, false);
        }
    }

    private void OnMouseExit()
    {
        renderer.material.color = color;
    }

    private void OnMouseDown()
    {
        if (isMovable && !Player.moving)
        {
            GroundGenerator.CalculatePath(transform.position, true);
        }
    }

    public void CalculateFcost()
    {
        fCost = gCost + hCost;
    }
}
