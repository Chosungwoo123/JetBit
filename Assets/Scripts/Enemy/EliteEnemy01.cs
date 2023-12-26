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
        int shootCount = Random.Range(5, 10);
        int bulletCount = 10;
        float offset = 0f;

        for (int i = 0; i < shootCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount + offset), 
                                          Mathf.Sin(Mathf.PI * 2 * j / bulletCount + offset));

                Debug.Log(Mathf.PI * 2 * j / bulletCount);

                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                bullet.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 7, ForceMode2D.Impulse);

                
            }

            offset += 1f;

            yield return new WaitForSeconds(0.5f);
        }

        isAttack = false;
    }
}