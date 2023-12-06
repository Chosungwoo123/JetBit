using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rotSpeed;
    public float moveSpeed;
    public float fireRate;

    public Transform shotPos;

    public PlayerBullet bulletPrefab;

    public ParticleSystem particle;

    private float fireTimer;

    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RotationUpdate();
        MoveUpdate();
        AttackUpdate();
    }

    private void RotationUpdate()
    {
        Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        Quaternion dirRot = Quaternion.Euler(0, 0, angle - 90);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, Time.deltaTime * rotSpeed);
    }

    private void MoveUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigid.velocity = transform.up * moveSpeed;
            particle.Play();
            return;
        }
        else
        {
            particle.Stop();
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
}