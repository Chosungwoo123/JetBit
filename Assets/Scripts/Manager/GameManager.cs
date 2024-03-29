using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region 싱글톤

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    #endregion

    #region UI 관련 오브젝트

    [Space(10)]
    [Header("UI 관련 오브젝트")]
    [SerializeField] private Image effectImage;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image fadeImage;

    #endregion

    #region 카메라 관련 오브젝트

    [Space(10)]
    [Header("카메라 관련 오브젝트")]
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private CameraZoomInOut cameraZoom;

    #endregion

    #region 게임 관련 중요한 오브젝트

    [Space(10)]
    [Header("게임 관련 중요한 오브젝트")]
    public GameObject curPlayer;
    public GameObject windEffect;

    #endregion

    #region 게임 관련 중요한 변수들

    [Space(10)]
    [Header("게임 관련 중요한 변수들")]
    [SerializeField] private int maxFrame;

    #endregion

    #region 사운드 관련 변수

    [Space(10)]
    [Header("사운드 관련 변수")]
    [SerializeField] private AudioClip bgm;

    #endregion

    private int curScore = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 페이드 인
        StartCoroutine(FadeInRoutine());

        // 프레임 고정
        Application.targetFrameRate = maxFrame;

        // 변수 초기화
        scoreText.text = curScore.ToString();

        // BGM 틀기
        SoundManager.Instance.PlayMusic(bgm);
    }

    private IEnumerator FadeInRoutine()
    {
        float curAlpha = 1;
        float temp = 0;

        Color fadeColor = fadeImage.color;

        while (temp <= 0.5f)
        {
            curAlpha -= Time.deltaTime / 0.5f;

            fadeColor.a = curAlpha;
            fadeImage.color = fadeColor;

            temp += Time.deltaTime;

            yield return null;
        }
    }

    public void CameraShake(float intensity, float time)
    {
        cameraShake.ShakeCamera(intensity, time);
    }

    Coroutine effectImageRoutine;
    public void ShowEffectImage(float time, float fadeAmount)
    {
        if (effectImageRoutine != null)
        {
            StopCoroutine(effectImageRoutine);
        }

        effectImageRoutine = StartCoroutine(FadeOutInObject(effectImage, time, fadeAmount));
    }

    private IEnumerator FadeOutInObject(Image _image, float time, float fadeAmount)
    {
        if (time == 0)
        {
            yield break;
        }

        float curAlpha = 0;
        float temp = 0;

        float fadeInOutTime = time / 2;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);

        while (temp <= fadeInOutTime)
        {
            curAlpha += Time.deltaTime * fadeAmount / fadeInOutTime;

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);

            temp += Time.deltaTime;

            yield return null;
        }

        curAlpha = fadeAmount;

        temp = 0;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);

        while (temp <= fadeInOutTime)
        {
            curAlpha -= Time.deltaTime / fadeInOutTime;

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);

            temp += Time.deltaTime;

            yield return null;
        }
    }

    public void CameraZoomInOut(float zoomAmount, float time)
    {
        cameraZoom.CameraZoom(zoomAmount, time);
    }

    public void PlusScore(int score)
    {
        curScore += score;

        scoreText.text = curScore.ToString();
        scoreText.transform.DOScale(Vector3.one * 0.5f, 0f);
        scoreText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
    }
}