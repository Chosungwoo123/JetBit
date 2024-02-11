using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private Effect endEffect;

    private float moveSpeed;
    private float lifeTime;
    private float damage;

    private Rigidbody2D rigid;
    private WaitForSeconds dieTime;

    

    public void InitBullet(float speed, float lifeTime, float damage)
    {
        this.moveSpeed = speed;
        this.lifeTime = lifeTime;
        this.damage = damage;

        // 변수 초기화
        dieTime = new WaitForSeconds(lifeTime);
        rigid = GetComponent<Rigidbody2D>();

        rigid.velocity = transform.up * moveSpeed;

        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        yield return dieTime;

        Instantiate(endEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(endEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}