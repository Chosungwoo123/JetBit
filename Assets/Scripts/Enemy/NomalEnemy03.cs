using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalEnemy03 : EnemyBase
{
    protected override void Start()
    {
        base.Start();

        SetRandomTargetPos();
        InvokeRepeating(nameof(SetRandomTargetPos), 4, 4);
    }

    protected override void Update()
    {
        base.Update();

        ChildUpdate();
    }

    protected override void TargetUpdate()
    {

    }

    private void ChildUpdate()
    {
        if (transform.childCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void SetRandomTargetPos()
    {
        targetPos = new Vector3(Random.Range(-96, 96), Random.Range(-17, 17), 0);
    }
}