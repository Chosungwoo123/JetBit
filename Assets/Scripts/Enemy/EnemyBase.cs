using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    public EnemyDetails enemyDetails;

    #region 이펙트 관련

    [Space(10)]
    [Header("이펙트 관련")]
    public GameObject smokeEffect;

    #endregion

    protected bool isAttack;

    protected float attackTimer;

    private bool isDie;

    private float moveSpeed;
    private float curHealth;

    private Rigidbody2D rigid;
    private SpriteRenderer sr;

    private WaitForSeconds hitDelay;

    private Vector2 moveVec;

    protected Vector3 targetPos;

    protected virtual void Start()
    {
        // 변수 초기화
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        moveSpeed = enemyDetails.moveSpeed;
        moveSpeed += Random.Range(enemyDetails.maxAdjustmentMoveSpeed, enemyDetails.maxAdjustmentMoveSpeed);

        hitDelay = new WaitForSeconds(0.05f);
    }

    private void OnEnable()
    {
        // 오브젝트 풀에서 꺠어날때 초기화
        curHealth = enemyDetails.maxHealth;
        isDie = false;
    }

    protected virtual void Update()
    {
        if (isDie)
        {
            // 플레이어 쪽 말고 진행방향 쪽을 바라봄
            if (enemyDetails.isRotation)
            {
                Vector2 dir = rigid.velocity.normalized;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
            
            return;
        }

        RotationUpdate();
        MoveUpdate();
        AttackUpdate();
        TargetUpdate();
    }

    private void RotationUpdate()
    {
        // 플레이어 방향 바라보기
        if (!enemyDetails.isRotation)
        {
            return;
        }

        Vector2 playerDir = targetPos - transform.position;

        float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;

        Quaternion dirRot = Quaternion.Euler(0, 0, angle - 90);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, Time.deltaTime * enemyDetails.rotSpeed);
    }

    private void MoveUpdate()
    {
        if (enemyDetails.isRotation)
        {
            moveVec.x = Mathf.Lerp(rigid.velocity.x, transform.up.x * moveSpeed, Time.deltaTime * enemyDetails.moveMultiply);
            moveVec.y = Mathf.Lerp(rigid.velocity.y, transform.up.y * moveSpeed, Time.deltaTime * enemyDetails.moveMultiply);
        }
        else
        {
            Vector2 dir = GameManager.Instance.curPlayer.transform.position - transform.position;

            dir.Normalize();

            moveVec.x = Mathf.Lerp(rigid.velocity.x, dir.x * moveSpeed, Time.deltaTime * enemyDetails.moveMultiply);
            moveVec.y = Mathf.Lerp(rigid.velocity.y, dir.y * moveSpeed, Time.deltaTime * enemyDetails.moveMultiply);
        }

        rigid.velocity = moveVec;
    }

    protected virtual void AttackUpdate()
    {
        if (isAttack)
        {
            return;
        }

        if (attackTimer >= enemyDetails.attackRate && !isAttack)
        {
            ShootBullet();
            attackTimer = 0;
        }

        attackTimer += Time.deltaTime;
    }

    protected virtual void ShootBullet()
    {
        return;
    }

    protected virtual void TargetUpdate()
    {
        targetPos = GameManager.Instance.curPlayer.transform.position;
    }

    public void OnDamage(float damage)
    {
        if (isDie)
        {
            return;
        }

        curHealth -= damage;

        if (curHealth <= 0)
        {
            StartCoroutine(DieRoutine());
            return;
        }

        StartCoroutine(HitRoutine(damage));
    }

    private IEnumerator DieRoutine()
    {
        smokeEffect.SetActive(true);
        gameObject.layer = 7;

        isDie = true;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < enemyDetails.dieEffects.Length; i++)
        {
            Instantiate(enemyDetails.dieEffects[i], transform.position, Quaternion.identity);
        }

        // 스코어 플러스, 이펙트
        GameManager.Instance.CameraShake(30, 0.1f);
        GameManager.Instance.PlusScore(enemyDetails.dieScore);

        enemyDetails.scorePopup.Spawn(transform.position, enemyDetails.dieScore);

        gameObject.SetActive(false);
    }

    private IEnumerator HitRoutine(float damageAmount)
    {
        // 반짝 거리는 애니메이션
        sr.material = enemyDetails.hitMaterial;

        // 데미지 팝업
        enemyDetails.damagePopup.Spawn(transform.position + (Vector3)Random.insideUnitCircle, damageAmount);

        yield return hitDelay;

        sr.material = enemyDetails.nomalMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && enemyDetails.isSelfDestruct)
        {
            // 죽는 로직
            for (int i = 0; i < enemyDetails.dieEffects.Length; i++)
            {
                Instantiate(enemyDetails.dieEffects[i], transform.position, Quaternion.identity);
            }

            // 플레이어 데미지 주기

            collision.GetComponent<Player>().OnDamage(enemyDetails.contactDamage);

            Destroy(gameObject);
        }
    }
}