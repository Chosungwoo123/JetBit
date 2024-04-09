using UnityEngine;
using DamageNumbersPro;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "ScriptableObject/EnemyDetails")]
public class EnemyDetails : ScriptableObject
{
    #region 기본 스탯

    [Space(10)]
    [Header("기본 스탯")]
    public float rotSpeed;
    public float moveSpeed;
    public float moveMultiply;
    public float maxHealth;

    public int maxAdjustmentMoveSpeed;
    public int minAdjustmentMoveSpeed;

    [Tooltip("플레이어 쪽을 바라보는지 체크하는 변수")]
    public bool isRotation;

    [Tooltip("플레이어에 닿으면 자폭하는지 체크하는 변수")]
    public bool isSelfDestruct;

    #endregion

    #region 이펙트 관련

    [Space(10)]
    [Header("이펙트 관련")]
    public GameObject[] dieEffects;

    #endregion

    #region 공격 관련 스탯

    [Space(10)]
    [Header("공격 관련 스탯")]
    public float attackRate;
    public GameObject bulletPrefab;
    public float contactDamage;

    #endregion

    #region 머테리얼 관련

    [Space(10)]
    [Header("머테리얼 관련")]
    public Material hitMaterial;
    public Material nomalMaterial;

    #endregion

    #region UI 관련

    [Space(10)]
    [Header("UI 관련")]
    public DamageNumber damagePopup;
    public DamageNumber scorePopup;

    #endregion

    #region 스코어

    [Space(10)]
    [Header("스코어")]
    public int dieScore;

    #endregion
}