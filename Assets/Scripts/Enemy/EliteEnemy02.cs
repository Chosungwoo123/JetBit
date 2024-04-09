using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy02 : EnemyBase
{
    #region 미사일 관련

    [Space(10)]
    [Header("미사일 관련")]
    [SerializeField] private EnemyRocketCrosshair crosshairPrefab;

    #endregion

    protected override void ShootBullet()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        isAttack = true;

        var crosshair = Instantiate(crosshairPrefab);
        crosshair.Init(gameObject);

        yield return new WaitForSeconds(3f);

        crosshair.Stop();

        Instantiate(enemyDetails.bulletPrefab, transform.position, Quaternion.identity)
            .GetComponent<EliteEnemy02Rocket>().SetTargetPos(crosshair.transform.position, crosshair);

        isAttack = false;
    }
}