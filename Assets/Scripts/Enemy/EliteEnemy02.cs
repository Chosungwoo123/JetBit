using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy02 : EnemyBase
{
    #region 미사일 관련

    [Space(10)]
    [Header("미사일 관련")]
    [SerializeField] private EnemyRocketCrosshair crosshair;

    #endregion

    protected override void ShootBullet()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        yield break;
    }
}