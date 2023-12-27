using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private Effect endEffect;

    private Rigidbody2D rigid;
    private WaitForSeconds dieTime;

    private void Awake()
    {
        dieTime = new WaitForSeconds(lifeTime);
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
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