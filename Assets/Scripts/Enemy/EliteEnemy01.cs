using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy01 : EnemyBase
{
    protected override void ShootBullet()
    {
        isAttack = true;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        int shootCount = Random.Range(3, 6);
        int bulletCount = 7;
        float offset = 0f;

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount + offset), 
                                          Mathf.Sin(Mathf.PI * 2 * j / bulletCount + offset));

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                angle -= 90;

                Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
            }

            offset += 1f;

            yield return new WaitForSeconds(0.2f);
        }

        isAttack = false;
    }
}