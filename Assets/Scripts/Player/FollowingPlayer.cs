using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingPlayer : MonoBehaviour
{
    public Transform targetTran;

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, targetTran.position, Time.deltaTime * 10);
    }
}