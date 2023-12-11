using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    public float rotSpeed;
    public float moveSpeed;
    public float moveMultiply;

    private float angle;

    private Rigidbody2D rigid;

    private Vector2 moveVec;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveUpdate();
        RotationUpdate();
    }

    private void MoveUpdate()
    {
        moveVec.x = Mathf.Lerp(rigid.velocity.x, transform.up.x * moveSpeed, Time.deltaTime * moveMultiply);
        moveVec.y = Mathf.Lerp(rigid.velocity.y, transform.up.y * moveSpeed, Time.deltaTime * moveMultiply);

        rigid.velocity = moveVec;
    }

    private void RotationUpdate()
    {
        Quaternion dirRot = Quaternion.Euler(0, 0, angle);

        this.transform.rotation = Quaternion.Slerp(transform.rotation, dirRot, Time.deltaTime * rotSpeed);
    }

    public void InitRocket(float targetAngle)
    {
        angle = targetAngle;

        Debug.Log(angle);
    }
}