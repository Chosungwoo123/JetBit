using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Image fadeOutImage;

    private bool isGameStart = false;

    private void Update()
    {
        if (Input.anyKeyDown && !isGameStart)
        {
            isGameStart = true;

            StartCoroutine(GameStartRoutine());
        }
    }

    private IEnumerator GameStartRoutine()
    {
        float targetAlpha = 1;
        float curAlpha = 0;
        float temp = 0;

        fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, curAlpha);

        while (temp <= 0.5f)
        {
            curAlpha += Time.deltaTime * targetAlpha / 0.5f;

            fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, curAlpha);

            temp += Time.deltaTime;

            yield return null;
        }

        SceneManager.LoadScene("GameScene");
    }
}