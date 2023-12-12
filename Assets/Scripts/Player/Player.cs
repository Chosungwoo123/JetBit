using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rotSpeed;
    public float moveSpeed;
    public float moveMultiply;
    public float fireRate;
    public float dashCoolTime;
    public float dashPower;
    public float dashTime;

    public Transform shotPos;

    public PlayerBullet bulletPrefab;
    public GameObject dashEffectPrefab;

    public ParticleSystem trailParticle;

    public PlayerRocket rocketPrefab;

    private float fireTimer;
    private float dashTimer;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            var rocket = Instantiate(rocketPrefab, shotPos.position, 
                                     Quaternion.Euler(0, 0, transform.eulerAngles.z + Random.Range(-90, 90)));

            rocket.InitRocket(transform.eulerAngles.z);
        }
    }
}