using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalEnemy02 : EnemyBase
{
    [Space(10)]
    [Header("스캔 범위")]
    [SerializeField] private float attackRange;
    [SerializeField] private float targetingRange;

    private bool canAttack;
    private bool playerScaning;

    protected override void Start()
    {
        base.Start();


        // 랜덤한 위치로 이동
        targetPos = new Vector3(Random.Range(-96, 96), Random.Range(-17, 17), 0);
    }

    protected override void Update()
    {
        base.Update();
        ScanUpdate();
    }

    private void ScanUpdate()
    {
        // 플레이어가 주변에 있으면 플레이어를 공격
        if (Vector3.Distance(GameManager.Instance.curPlayer.transform.position, transform.position) <= attackRange)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        // 플레이어가 가까이 다가오면 플레이어를 쫒아감
        if (Vector3.Distance(GameManager.Instance.curPlayer.transform.position, transform.position) <= targetingRange)
        {
            playerScaning = true;
        }
    }

    protected override void TargetUpdate()
    {
        if (!playerScaning && Vector3.Distance(targetPos, transform.position) <= 1f)
        {
            SettingNextTargetPos();
        }
        else if (playerScaning)
        {
            targetPos = GameManager.Instance.curPlayer.transform.position;
        }
    }

    private void SettingNextTargetPos()
    {
        Vector3 newTargetPos = new Vector3(Random.Range(-96, 96), Random.Range(-17, 17), 0);

        while (Vector2.Distance(targetPos, newTargetPos) < 30f)
        {
            newTargetPos = new Vector3(Random.Range(-96, 96), Random.Range(-17, 17), 0);
        }

        targetPos = newTargetPos;
    }

    protected override void AttackUpdate()
    {

    }

    protected override void ShootBullet()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}