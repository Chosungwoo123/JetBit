using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRocketCrosshair : MonoBehaviour
{
    private GameObject targetObj;
    private SpriteRenderer sr;

    private bool isStop;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        targetObj = GameManager.Instance.curPlayer;
        sr = GetComponent<SpriteRenderer>();

        isStop = false;

        transform.localScale = Vector3.one * 30;
        transform.DOScale(1, 1);

        StartCoroutine(AlphaRoutine());
    }

    public void Stop()
    {

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
    }
}