using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private float speed;
    Animator playerAnimator;
    public static bool moving;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }
    public void Move(List<Ground> path)
    {
        playerAnimator.SetBool("run", true);
        moving = true;
        StartCoroutine(MovePlayerCoroutine(path));
    }

    IEnumerator MovePlayerCoroutine(List<Ground> path)
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
        playerAnimator.SetBool("run", false);
        moving = false;
    }
}
