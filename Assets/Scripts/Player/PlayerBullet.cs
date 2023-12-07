using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.velocity = transform.up * moveSpeed;

        StartCoroutine(ScaleRoutine());
    }

    private IEnumerator ScaleRoutine()
    {
        Vector3 curScale = transform.localScale;

        curScale.x = 0;

        transform.localScale = curScale;

        while (curScale.x < 1)
        {
            curScale.x += Time.deltaTime / 0.08f;

            transform.localScale = curScale;

            yield return null;
        }

        curScale.x = 1;

        transform.localScale = curScale;
    }
}