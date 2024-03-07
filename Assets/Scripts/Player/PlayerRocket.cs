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

    [Tooltip("흐물흐물하게 움직이는 정도")]
    public float sinMovementAmount;
    public float sinMovementSpeed;

    public GameObject explotionEffect;

    public LayerMask targetLayer;

    public AudioClip explosionSound;

    private float angle;
    private float sinAmount;

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
    }

    private void FixedUpdate()
    {
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
            Vector2 targetDir = target.transform.position - transform.position;

            angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            angle -= 90;
        }
        else
        {
            isTargeting = false;
        }

        sinAmount += Time.deltaTime * (sinMovementSpeed * ((isTargeting) ? rotSpeed / targetingRotSpeed : 1));

        Quaternion dirRot = Quaternion.Euler(0, 0, angle);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, 
                                                   Time.deltaTime * ((isTargeting) ? targetingRotSpeed : rotSpeed));

        Quaternion sinRot = Quaternion.Euler(0, 0, transform.eulerAngles.z + (sinMovementAmount * Mathf.Sin(sinAmount)));

        transform.rotation = sinRot;
    }

    private void CheckingTarget()
    {
        if (isTargeting)
        {
            return;
        }

        Collider2D[] scaningTarget = Physics2D.OverlapCircleAll(transform.position, scanRange, targetLayer);

        float tempDistance = 99999f;

        if (scaningTarget != null)
        {
            isTargeting = true;
        }

        foreach (Collider2D temp in scaningTarget)
        {
            if (tempDistance > Vector2.Distance(transform.position, temp.transform.position))
            {
                target = temp.gameObject;
                tempDistance = Vector2.Distance(transform.position, temp.transform.position);
            }
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

            GameManager.Instance.CameraShake(50, 0.01f);
            SoundManager.Instance.PlaySound(explosionSound);

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}