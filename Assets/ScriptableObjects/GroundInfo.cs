using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundInfo", menuName = "Custom/Ground info")]
[Serializable]
public class GroundInfo : ScriptableObject
{
    public Vector2 size;
    public List<bool> obstacles;
}