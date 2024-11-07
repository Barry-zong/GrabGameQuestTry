using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WASDcontrol : MonoBehaviour
{
    public float acceleration = 2f; // 移动加速度
    public float maxSpeed = 5f; // 最大移动速度

    private Vector3 movement; // 移动方向
    private bool isMoving = false; // 是否正在移动

    void Update()
    {
        HandleInput();
        UpdateMovement();
    }

    void HandleInput()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // 检测W、A、S、D按键
        if (Input.GetKey(KeyCode.W))
        {
            verticalInput -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            verticalInput += 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput -= 1f;
        }

        // 根据输入方向设置移动方向
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // 检测是否有输入
        isMoving = movement.magnitude > 0f;
    }

    void UpdateMovement()
    {
        if (isMoving)
        {
            // 根据加速度和最大速度逐渐增加速度
            float speed = Mathf.Min(maxSpeed, acceleration * Time.deltaTime);
            transform.Translate(movement * speed);
        }
        else
        {
            // 如果没有输入，逐渐减小速度
            transform.Translate(movement * acceleration * Time.deltaTime);
        }
    }

}
