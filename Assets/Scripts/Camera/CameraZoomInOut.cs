using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomInOut : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachine;

    private void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
    }

    Coroutine cameraRoutine;
    public void CameraZoom(float zoomAmount, float time)
    {
        if (cameraRoutine != null)
        {
            StopCoroutine(cameraRoutine);
        }

        cameraRoutine = StartCoroutine(ZoomRoutine(zoomAmount, time));
    }

    private IEnumerator ZoomRoutine(float zoomAmount, float time)
    {
        if (time == 0)
        {
            cinemachine.m_Lens.OrthographicSize = zoomAmount;
            yield break;
        }

        float curSize = cinemachine.m_Lens.OrthographicSize;
        float temp = 0f;
        float ratio = 0;

        while (temp <= time)
        {
            cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(curSize, zoomAmount, ratio);

            temp += Time.deltaTime;
            ratio += Time.deltaTime / time;

            yield return null;
        }

        cinemachine.m_Lens.OrthographicSize = zoomAmount;
    }
}