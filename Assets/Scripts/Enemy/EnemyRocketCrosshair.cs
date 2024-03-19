using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRocketCrosshair : MonoBehaviour
{
    private GameObject targetObj;
    private SpriteRenderer sr;
    private GameObject parentObj;

    private bool isStop;

    public void Init(GameObject parent)
    {
        targetObj = GameManager.Instance.curPlayer;
        sr = GetComponent<SpriteRenderer>();
        parentObj = parent;

        isStop = false;

        transform.localScale = Vector3.one * 30;
        transform.DOScale(1, 1);

        StartCoroutine(AlphaRoutine());
    }

    public void Stop()
    {
        isStop = true;
    }

    private IEnumerator AlphaRoutine()
    {
        Color color = sr.color;

        color.a = 0;
        sr.color = color;

        float temp = 0;

        while (temp < 1)
        {
            color.a = temp;
            sr.color = color;

            temp += Time.deltaTime;

            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (!isStop)
        {
            transform.position = Vector2.Lerp(transform.position, targetObj.transform.position, Time.deltaTime * 20);
        }

        // 중간에 적이 죽으면 바로 파괴
        if (parentObj == null)
        {
            Destroy(gameObject);
        }
    }
}