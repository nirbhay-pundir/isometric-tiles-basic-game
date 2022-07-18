using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GroundGenerator))]
public class GroundEditor : Editor
{
    GroundGenerator gg;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Board"))
        {
            GenerateBoard();
        }
        if (GUILayout.Button("Refresh Board"))
        {
            RefreshBoard();
        }
        if (GUILayout.Button("Clear Board"))
        {
            ClearBoard();
        }
    }

    public void GenerateBoard()
    {
        ClearBoard();
        gg = (GroundGenerator)target;
        GroundInfo info = Resources.Load<GroundInfo>("GroundInfo");
        info.size = gg.size;
        info.obstacles = new();
        List<List<Ground>> groundList = new();
        for (int i = 0; i < info.size.x; i++)
        {
            groundList.Add(new());
            for (int j = 0; j < info.size.y; j++)
            {
                var newGround = Instantiate(gg.ground) as Ground;
                newGround.transform.SetParent(gg.transform);
                newGround.transform.position = new Vector3(i, 0, j);
                newGround.PosOnGrid = new(i, j);
                newGround.isMovable = true;
                groundList[i].Add(newGround);
                info.obstacles.Add(true);
            }
        }
        ObstaclesManager.GenerateUI(info.size, groundList, gg.obstacle, gg.pointingSphere);
    }

    public void RefreshBoard()
    {
        gg = (GroundGenerator)target;
        ClearBoard();
        GroundInfo info = Resources.Load<GroundInfo>("GroundInfo");
        List<List<Ground>> groundList = new();
        int k = 0;
        for (int i = 0; i < info.size.x; i++)
        {
            groundList.Add(new());
            for (int j = 0; j < info.size.y; j++)
            {
                var newGround = Instantiate(gg.ground) as Ground;
                newGround.transform.SetParent(gg.transform);
                newGround.transform.position = new Vector3(i, 0, j);
                newGround.PosOnGrid = new(i, j);
                newGround.isMovable = info.obstacles[k];
                if (!newGround.isMovable)
                {
                    var sphere = Instantiate(gg.obstacle);
                    sphere.transform.SetParent(newGround.transform);
                    sphere.transform.localPosition = new Vector3(0f, 0.5f, 0);
                }
                groundList[i].Add(newGround);
                k++;
            }
        }
        ObstaclesManager.GenerateUI(info.size, groundList, gg.obstacle, gg.pointingSphere);
    }

    void ClearBoard()
    {
        gg = (GroundGenerator)target;
        for (int i = gg.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gg.transform.GetChild(i).gameObject);
        }
        ObstaclesManager.ClearUI();
    }
}
