using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 기본 스탯

    [Space(10)] 
    [Header("기본 스탯 관련")]
    [SerializeField] private float rotSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveMultiply;
    [SerializeField] private float fireRate;
    [SerializeField] private float dashCoolTime;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashTime;
    [SerializeField] private float maxHealth;

    #endregion

    #region 게임 오브젝트

    [Space(10)]
    [Header("게임 오브젝트 관련")]

    [SerializeField] private Transform shotPos;

    [SerializeField] private PlayerBullet bulletPrefab;
    [SerializeField] private GameObject dashEffectPrefab;

    [SerializeField] private ParticleSystem trailParticle;

    [SerializeField] private PlayerRocket rocketPrefab;
    [SerializeField] private PlayerRocket missilePrefab;

    #endregion

    #region 사운드

    [Space(10)]
    [Header("사운드")]
    public PlayerSounds sounds;

    #endregion

    #region 스킬

    [Space(10)]
    [Header("스킬 관련 스탯")]
    [SerializeField] private float missileSkillCoolTime;
    

    #endregion

    private float fireTimer;
    private float dashTimer;
    private float curHealth;
    private float missileSkillTimer;

    private bool isMoveStop;
    private bool isDashing;

    private Rigidbody2D rigid;

    private WaitForSeconds dashTimeWaitForSeconds;

    private Animator anim;

    private Vector2 moveVec;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        curHealth = maxHealth;
        missileSkillTimer = 0f;

        dashTimeWaitForSeconds = new WaitForSeconds(dashTime);
    }

    private void Update()
    {
        RotationUpdate();
        MoveUpdate();
        AttackUpdate();
        DashUpdate();
        AnimationUpdate();
        SkillUpdate();
    }

    private void RotationUpdate()
    {
        Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        Quaternion dirRot = Quaternion.Euler(0, 0, angle - 90);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, Time.deltaTime * rotSpeed);

        if (transform.eulerAngles.z > 45 && transform.eulerAngles.z < 140)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void MoveUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SoundManager.Instance.PlaySound(sounds.propelSound);
        }
        if (Input.GetKey(KeyCode.W) && !isMoveStop)
        {
            moveVec.x = Mathf.Lerp(rigid.velocity.x, transform.up.x * moveSpeed, Time.deltaTime * moveMultiply);
            moveVec.y = Mathf.Lerp(rigid.velocity.y, transform.up.y * moveSpeed, Time.deltaTime * moveMultiply);

            rigid.velocity = moveVec;

            trailParticle.Play();
            return;
        }
        else if (isDashing && Input.GetKey(KeyCode.W))
        {
            trailParticle.Play();
            return;
        }
        else
        {
            trailParticle.Stop();
            return;
        }
    }

    private void AttackUpdate()
    {
        if (Input.GetMouseButton(0) && fireTimer >= fireRate)
        {
            Instantiate(bulletPrefab, shotPos.position, transform.rotation);
            fireTimer = 0;
            SoundManager.Instance.PlaySound(sounds.shotSound);
        }

        fireTimer += Time.deltaTime;
    }

    private void DashUpdate()
    {
        if (Input.GetMouseButtonDown(1) && dashTimer >= dashCoolTime && !isDashing)
        {
            StartCoroutine(DashRoutine());
        }

        dashTimer += Time.deltaTime;
    }

    private IEnumerator DashRoutine()
    {
        rigid.velocity = Vector2.zero;

        isMoveStop = true;
        isDashing = true;
        Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);

        SoundManager.Instance.PlaySound(sounds.dashSound);

        GameManager.Instance.CameraShake(20, 0.3f);
        GameManager.Instance.ShowEffectImage(0.1f, 0.5f);

        rigid.AddForce(transform.up * dashPower, ForceMode2D.Impulse);

        yield return dashTimeWaitForSeconds;

        isMoveStop = false;
        isDashing = false;
        dashTimer = 0;
    }

    private void AnimationUpdate()
    {
        anim.SetFloat("Rotation", Mathf.Abs((transform.eulerAngles.z > 180) ? 180 -
                                            (transform.eulerAngles.z - 180) : transform.eulerAngles.z));
    }

    private void SkillUpdate()
    {
        // 미사일 발사
        if (Input.GetKeyDown(KeyCode.E))
        {
            var rocket = Instantiate(rocketPrefab, shotPos.position, 
                                     Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-90, 90)));

            rocket.InitRocket(transform.eulerAngles.z);
        }

        // 필살기(미사일 여러 발 발사)
        if (Input.GetKeyDown(KeyCode.Q) && missileSkillTimer >= missileSkillCoolTime)
        {
            MissileSkill();
            missileSkillTimer = 0;
        }

        missileSkillTimer += Time.deltaTime;
    }

    private void MissileSkill()
    {
        StartCoroutine(MissileSkillRoutine());
    }

    private IEnumerator MissileSkillRoutine()
    {
        WaitForSeconds shotInterval = new WaitForSeconds(0.05f);

        for (int i = 0; i < 13; i++)
        {
            float randomDir = Random.Range(0, 360);

            var missile = Instantiate(missilePrefab, transform.position, Quaternion.Euler(0, 0, randomDir));

            missile.InitRocket(randomDir);

            SoundManager.Instance.PlaySound(sounds.missileShotSound, 0.8f);

            yield return shotInterval;
        }
    }

    public void OnDamage(float damage)
    {
        curHealth = Mathf.Max(curHealth - damage, 0);

        if (curHealth <= 0)
        {
            // 죽는 로직
        }

        // 카메라 흔들기
        GameManager.Instance.CameraShake(60, 0.3f);
        GameManager.Instance.ShowEffectImage(0.15f, 0.5f);

        // 시간 느리게
        TimeManager.Instance.SlowTime(0.05f, 0.6f);
    }

    [System.Serializable]
    public class PlayerSounds
    {
        public AudioClip shotSound;
        public AudioClip dashSound;
        public AudioClip propelSound;
        public AudioClip missileShotSound;
    }
}