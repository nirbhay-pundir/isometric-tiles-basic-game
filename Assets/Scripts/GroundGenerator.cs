using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject enemyObj;
    public Ground ground;
    public GameObject obstacle;
    public GameObject pointingSphere;
    public Vector2Int size;

    public static GroundInfo info;
    public static PathFinder pF;
    public static List<List<Ground>> groundList;

    static LineRenderer line;
    public static Player player;
    public static bool started;

    private void Awake()
    {
        started = false;
        info = Resources.Load<GroundInfo>("GroundInfo");
        playerObj.transform.position = new(0f, 0.5f, 0.5f);
        player = playerObj.GetComponent<Player>();

        enemyObj.transform.position = new(size.x - 1, 0.5f, size.y - 1);

        line = GetComponent<LineRenderer>();

        line.startWidth = 0.05f;
        line.endWidth = 0.05f;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        int k = 0;
        groundList = new();
        for (int i = 0; i < info.size.x; i++)
        {
            groundList.Add(new());
            for (int j = 0; j < info.size.y; j++)
            {
                var newGround = Instantiate(ground) as Ground;
                newGround.transform.SetParent(transform);
                newGround.transform.position = new Vector3(i, 0, j);
                newGround.PosOnGrid = new(i, j);
                newGround.isMovable = info.obstacles[k];
                if (!newGround.isMovable)
                {
                    var sphere = Instantiate(obstacle);
                    sphere.transform.SetParent(newGround.transform);
                    sphere.transform.localPosition = new Vector3(0f, 0.5f, 0);
                }
                groundList[i].Add(newGround);
                k++;
            }
        }
        pF = new(groundList, size);
    }
    public static void CalculatePath(Vector3 pos, bool move)
    {
        List<Ground> path = pF.FindPath(groundList[(int)player.transform.position.x][(int)player.transform.position.z], groundList[(int)pos.x][(int)pos.z]);
        if (path != null)
        {
            line.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                line.SetPosition(i, new(path[i].PosOnGrid.x, 0.52f, path[i].PosOnGrid.y));
            }
            if (move)
            {
                if (started == false)
                    started = true;
                player.Move(path);
            }
        }
    }
}
