using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float sec;

    private SpriteRenderer sr;
    private WaitForSeconds waitTime;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        waitTime = new WaitForSeconds(sec / sprites.Count);

        StartCoroutine(EffectStart());
    }

    private IEnumerator EffectStart()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            sr.sprite = sprites[i];

            yield return waitTime;
        }

        Destroy(gameObject);
    }
}