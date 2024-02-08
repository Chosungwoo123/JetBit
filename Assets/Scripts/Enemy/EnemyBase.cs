using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    #region 기본 스탯

    [Space(10)]
    [Header("기본 스탯")]
    [SerializeField] private float rotSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveMultiply;
    [SerializeField] private float maxHealth;

    [SerializeField] private int maxAdjustmentMoveSpeed;
    [SerializeField] private int minAdjustmentMoveSpeed;
    
    [Tooltip("플레이어 쪽을 바라보는지 체크하는 변수")]
    [SerializeField] private bool isRotation;

    [Tooltip("플레이어에 닿으면 자폭하는지 체크하는 변수")]
    [SerializeField] private bool isSelfDestruct;

    #endregion

    public GameObject dieEffect;
    public GameObject dieEffect2;

    #region 공격 관련 스탯

    [Space(10)]
    [Header("공격 관련 스탯")]
    [SerializeField] private float attackRate;
    [SerializeField] protected GameObject bulletPrefab;

    #endregion

    protected bool isAttack;

    private float curHealth;
    private float attackTimer;

    private Rigidbody2D rigid;

    private Vector2 moveVec;

    protected Vector3 targetPos;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveSpeed += Random.Range(maxAdjustmentMoveSpeed, maxAdjustmentMoveSpeed);

        curHealth = maxHealth;
    }

    protected virtual void Update()
    {
        RotationUpdate();
        MoveUpdate();
        AttackUpdate();
        TargetUpdate();
    }

    private void RotationUpdate()
    {
        // 플레이어 방향 바라보기
        if (!isRotation)
        {
            return;
        }

        Vector2 playerDir = targetPos - transform.position;

        float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;

        Quaternion dirRot = Quaternion.Euler(0, 0, angle - 90);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, Time.deltaTime * rotSpeed);
    }

    private void MoveUpdate()
    {
        if (isRotation)
        {
            moveVec.x = Mathf.Lerp(rigid.velocity.x, transform.up.x * moveSpeed, Time.deltaTime * moveMultiply);
            moveVec.y = Mathf.Lerp(rigid.velocity.y, transform.up.y * moveSpeed, Time.deltaTime * moveMultiply);
        }
        else
        {
            Vector2 dir = GameManager.Instance.curPlayer.transform.position - transform.position;

            dir.Normalize();

            moveVec.x = Mathf.Lerp(rigid.velocity.x, dir.x * moveSpeed, Time.deltaTime * moveMultiply);
            moveVec.y = Mathf.Lerp(rigid.velocity.y, dir.y * moveSpeed, Time.deltaTime * moveMultiply);
        }

        rigid.velocity = moveVec;
    }

    protected virtual void AttackUpdate()
    {
        if (isAttack)
        {
            return;
        }

        if (attackTimer >= attackRate && !isAttack)
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
        curHealth -= damage;

        if (curHealth <= 0)
        {
            // 죽는 로직
            Instantiate(dieEffect, transform.position, transform.rotation);

            if (dieEffect2 != null)
            {
                Instantiate(dieEffect2, transform.position, transform.rotation);
            }

            GameManager.Instance.CameraShake(30, 0.3f);
            GameManager.Instance.ShowEffectImage(0.15f, 0.5f);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isSelfDestruct)
        {
            // 죽는 로직
            Instantiate(dieEffect, transform.position, transform.rotation);

            if (dieEffect2 != null)
            {
                Instantiate(dieEffect2, transform.position, transform.rotation);
            }

            GameManager.Instance.CameraShake(30, 0.3f);
            GameManager.Instance.ShowEffectImage(0.15f, 0.5f);

            // 플레이어 데미지 주기

            Destroy(gameObject);
        }
    }
}