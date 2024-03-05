using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    #region 싱글톤

    private static GUIManager instance = null;

    public static GUIManager Instance
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

    public Image[] playerHealthBar;

    private float playerHealthAmount = 1;

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

    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        for (int i = 0; i < playerHealthBar.Length; i++)
        {
            playerHealthBar[i].fillAmount = Mathf.Lerp(playerHealthBar[i].fillAmount, playerHealthAmount, Time.deltaTime * 10);
        }
    }

    public void SetHealthAmount(float amount)
    {
        playerHealthAmount = amount;
    }
}