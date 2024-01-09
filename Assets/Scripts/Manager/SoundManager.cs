using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameObject soundEffectObj;

    public float sfxVolume = 1f;
    public float bgmVolume = 1f;

    public static SoundManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.loop = true;
    }

    public void PlaySound(AudioClip clip, float pitch)
    {
        GameObject soundObj = PoolManager.Instance.GetGameObejct(soundEffectObj, transform.position, Quaternion.identity);

        soundObj.GetComponent<AudioSource>().pitch = pitch;
        soundObj.GetComponent<AudioSource>().volume = sfxVolume;

        soundObj.SetActive(true);

        soundObj.GetComponent<AudioSource>().PlayOneShot(clip);

        StartCoroutine(StopSound(soundObj, clip.length));
    }

    public void PlaySound(AudioClip clip)
    {
        GameObject soundObj = PoolManager.Instance.GetGameObejct(soundEffectObj, transform.position, Quaternion.identity);

        soundObj.GetComponent<AudioSource>().pitch = 1f;
        soundObj.GetComponent<AudioSource>().volume = sfxVolume;

        soundObj.SetActive(true);

        soundObj.GetComponent<AudioSource>().PlayOneShot(clip);

        StartCoroutine(StopSound(soundObj, clip.length));
    }

    IEnumerator StopSound(GameObject soundObj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (soundObj != null)
        {
            soundObj.SetActive(false);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = bgmVolume;
        musicSource.Play();
    }

    public void SetMusicVolume(float value)
    {
        bgmVolume = value;

        musicSource.volume = value;
    }

    public void SetEffectVolume(float value)
    {
        sfxVolume = value;
    }
}