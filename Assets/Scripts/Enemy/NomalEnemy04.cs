using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalEnemy04 : EnemyBase
{
    protected override void ShootBullet()
    {
        StartCoroutine(ShootRoutine());
    }

    WaitForSeconds shootInterval = new WaitForSeconds(0.1f);
    private IEnumerator ShootRoutine()
    {
        isAttack = true;

        int ranNum = Random.Range(3, 6);

        for (int i = 0; i < ranNum; i++)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<EnemyBullet>().InitBullet(30, 0.5f, 3);

            yield return shootInterval;
        }

        isAttack = false;
    }
}