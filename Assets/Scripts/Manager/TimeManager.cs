using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance = null;

    public static TimeManager Instance
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

    private bool waiting;

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

    public void StopTime(float duration)
    {
        if (waiting)
        {
            return;
        }

        Time.timeScale = 0.0f;
        StartCoroutine(WaitTime(duration));
    }

    public void SlowTime(float amount, float duration)
    {
        if (waiting)
        {
            return;
        }

        Time.timeScale = amount;
        StartCoroutine(WaitTime(duration));
    }

    private IEnumerator WaitTime(float duration)
    {
        waiting = true;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1.0f;

        waiting = false;
    }
}