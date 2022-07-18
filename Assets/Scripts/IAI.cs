using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAI
{
    void Move(List<Ground> path);
    IEnumerator MoveCoroutine(List<Ground> path);
    Vector3 GetPlayerPos();


}
