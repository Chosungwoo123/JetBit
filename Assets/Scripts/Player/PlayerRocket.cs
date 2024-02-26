using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    public float rotSpeed;
    public float targetingRotSpeed;
    public float moveSpeed;
    public float moveMultiply;
    public float scanRange;
    public float damage;
    public Effect explotionEffect;

    public LayerMask targetLayer;

    private float angle;

    private bool isTargeting;

    private GameObject target;
    private Rigidbody2D rigid;

    private Vector2 moveVec;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveUpdate();
        RotationUpdate();
        CheckingTarget();
    }

    private void MoveUpdate()
    {
        moveVec.x = Mathf.Lerp(rigid.velocity.x, transform.up.x * moveSpeed, Time.deltaTime * moveMultiply);
        moveVec.y = Mathf.Lerp(rigid.velocity.y, transform.up.y * moveSpeed, Time.deltaTime * moveMultiply);

        rigid.velocity = moveVec;
    }

    private void RotationUpdate()
    {
        if (target != null && target.layer == 6)
        {
            Vector2 mouseDir = target.transform.position - transform.position;

            angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

            angle -= 90;
        }
        else
        {
            isTargeting = false;
        }

        Quaternion dirRot = Quaternion.Euler(0, 0, angle);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, 
                                                   Time.deltaTime * ((isTargeting) ? targetingRotSpeed : rotSpeed));
    }

    private void CheckingTarget()
    {
        if (isTargeting)
        {
            return;
        }

        var scaningTarget = Physics2D.OverlapCircle(transform.position, scanRange, targetLayer);

        if (scaningTarget != null)
        {
            target = scaningTarget.gameObject;
            isTargeting = true;
            return;
        }
    }

    public void InitRocket(float targetAngle)
    {
        angle = targetAngle;
        isTargeting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().OnDamage(damage);

            Instantiate(explotionEffect, transform.position, Quaternion.identity);

            GameManager.Instance.CameraShake(20, 0.3f);
            GameManager.Instance.ShowEffectImage(0.1f, 0.5f);

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}