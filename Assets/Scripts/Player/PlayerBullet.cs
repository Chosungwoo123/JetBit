using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;
    public float damage;
    public GameObject endEffect;

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

        StartCoroutine(ScaleRoutine());
        StartCoroutine(DieRoutine());
    }

    private IEnumerator ScaleRoutine()
    {
        Vector3 curScale = transform.localScale;

        curScale.x = 0;

        transform.localScale = curScale;

        while (curScale.x < 1)
        {
            curScale.x += Time.deltaTime / 0.15f;

            transform.localScale = curScale;

            yield return null;
        }

        curScale.x = 1;

        transform.localScale = curScale;
    }

    private IEnumerator DieRoutine()
    {
        yield return dieTime;

        Instantiate(endEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBase>().OnDamage(damage);
            Instantiate(endEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}