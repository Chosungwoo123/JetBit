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

    [SerializeField] private bool isRotation;   // 플레이어 쪽을 바라보는지 체크하는 변수
    [SerializeField] private bool isSelfDestruct; // 플레이어에 닿으면 자폭하는지 체크하는 변수

    #endregion

    public Effect dieEffect;

    private float curHealth;

    private Rigidbody2D rigid;

    private Vector2 moveVec;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveSpeed += Random.Range(-3, 3);

        curHealth = maxHealth;
    }

    private void Update()
    {
        RotationUpdate();
        MoveUpdate();
    }

    private void RotationUpdate()
    {
        if (!isRotation)
        {
            return;
        }

        Vector2 playerDir = GameManager.Instance.curPlayer.transform.position - transform.position;

        float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;

        Quaternion dirRot = Quaternion.Euler(0, 0, angle - 90);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, Time.deltaTime * rotSpeed);
    }

    private void MoveUpdate()
    {
        moveVec.x = Mathf.Lerp(rigid.velocity.x, transform.up.x * moveSpeed, Time.deltaTime * moveMultiply);
        moveVec.y = Mathf.Lerp(rigid.velocity.y, transform.up.y * moveSpeed, Time.deltaTime * moveMultiply);

        rigid.velocity = moveVec;
    }

    public void OnDamage(float damage)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            // 죽는 로직
            Instantiate(dieEffect, transform.position, Quaternion.identity);

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
            Instantiate(dieEffect, transform.position, Quaternion.identity);

            GameManager.Instance.CameraShake(30, 0.3f);
            GameManager.Instance.ShowEffectImage(0.15f, 0.5f);

            Destroy(gameObject);
        }
    }
}