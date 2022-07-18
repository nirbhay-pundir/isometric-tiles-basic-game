using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAI
{
    LineRenderer line;
    public GroundGenerator GG;
    Animator enemyAnimator;
    bool moving;
    public float speed;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();

        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
    }
    public void Move(List<Ground> path)
    {
        enemyAnimator.SetBool("run", true);
        moving = true;
        StartCoroutine(MoveCoroutine(path));
    }

    public IEnumerator MoveCoroutine(List<Ground> path)
    {
        foreach (Ground g in path)
        {
            Vector3 pos = new(g.transform.position.x, 0.5f, g.transform.position.z);
            transform.LookAt(pos);
            while (Vector3.Distance(transform.position, pos) > .0001)
            {
                transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
                yield return null;
            }
        }
        enemyAnimator.SetBool("run", false);
        moving = false;
    }
    public Vector3 GetPlayerPos()
    {
        Vector3 playerPos = GroundGenerator.player.transform.position;

        List<Vector3> positions = new();
        if (playerPos.x - 1 >= 0)
        {
            if (GroundGenerator.groundList[(int)playerPos.x - 1][(int)playerPos.z].isMovable)
                positions.Add(new(playerPos.x - 1, 0.5f, playerPos.z));
            if (playerPos.z - 1 >= 0 && GroundGenerator.groundList[(int)playerPos.x - 1][(int)playerPos.z - 1].isMovable)
                positions.Add(new(playerPos.x - 1, 0.5f, playerPos.z - 1));
            if (playerPos.z + 1 < GG.size.y && GroundGenerator.groundList[(int)playerPos.x - 1][(int)playerPos.z + 1].isMovable)
                positions.Add(new(playerPos.x - 1, 0.5f, playerPos.z + 1));
        }
        if (playerPos.x + 1 < GG.size.x)
        {
            if (GroundGenerator.groundList[(int)playerPos.x + 1][(int)playerPos.z].isMovable)
                positions.Add(new(playerPos.x + 1, 0.5f, playerPos.z));
            if (playerPos.z - 1 >= 0 && GroundGenerator.groundList[(int)playerPos.x + 1][(int)playerPos.z - 1].isMovable)
                positions.Add(new(playerPos.x + 1, 0.5f, playerPos.z - 1));
            if (playerPos.z + 1 < GG.size.y && GroundGenerator.groundList[(int)playerPos.x + 1][(int)playerPos.z + 1].isMovable)
                positions.Add(new(playerPos.x + 1, 0.5f, playerPos.z + 1));
        }
        if (playerPos.z - 1 >= 0 && GroundGenerator.groundList[(int)playerPos.x][(int)playerPos.z - 1].isMovable)
            positions.Add(new(playerPos.x, 0.5f, playerPos.z - 1));
        if (playerPos.z + 1 < GG.size.y && GroundGenerator.groundList[(int)playerPos.x][(int)playerPos.z + 1].isMovable)
            positions.Add(new(playerPos.x, 0.5f, playerPos.z + 1));

        if (positions != null)
        {
            float minDistance = Vector3.Distance(transform.position, positions[0]);
            foreach (Vector3 pos in positions)
            {
                if (Vector3.Distance(transform.position, pos) < minDistance)
                {
                    minDistance = Vector3.Distance(transform.position, pos);
                    playerPos = pos;
                }
            }
            return playerPos;
        }
        else
            return new();
    }

    private void Update()
    {
        if (GroundGenerator.started == true && !Player.moving)
        {
            Vector3 pos = GetPlayerPos();
            if (pos != null && moving == false)
            {
                List<Ground> path = GroundGenerator.pF.FindPath(GroundGenerator.groundList[(int)transform.position.x][(int)transform.position.z], GroundGenerator.groundList[(int)pos.x][(int)pos.z]);
                if (path != null)
                {
                    line.positionCount = path.Count;
                    for (int i = 0; i < path.Count; i++)
                    {
                        line.SetPosition(i, new(path[i].PosOnGrid.x, 0.52f, path[i].PosOnGrid.y));
                    }
                    Move(path);
                }
            }
        }
    }
}
