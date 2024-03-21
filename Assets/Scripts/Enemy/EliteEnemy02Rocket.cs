using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy02Rocket : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float explosionRange;
    [SerializeField] private GameObject[] explosionEffects;

    private Vector3 targetPos;
    private Rigidbody2D rigid;
    private EnemyRocketCrosshair crosshairObj;

    public void SetTargetPos(Vector3 target, EnemyRocketCrosshair crosshair)
    {
        targetPos = target;
        crosshairObj = crosshair;

        Vector2 dir = targetPos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 180;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        rigid = GetComponent<Rigidbody2D>();

        rigid.velocity = dir.normalized * moveSpeed;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.5f)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        for (int i = 0; i < explosionEffects.Length; i++)
        {
            Instantiate(explosionEffects[i], transform.position, Quaternion.identity);
        }

        // 만약 플레이어가 볌위 안에 있으면 데미이 입히기
        if (Vector2.Distance(transform.position, GameManager.Instance.curPlayer.transform.position) < explosionRange)
        {
            GameManager.Instance.curPlayer.GetComponent<Player>().OnDamage(damage);
        }

        GameManager.Instance.CameraShake(30, 0.2f);

        if (crosshairObj != null)
        {
            Destroy(crosshairObj.gameObject);
        }
        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}