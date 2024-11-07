using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WASDcontrol : MonoBehaviour
{
    public float acceleration = 2f; // �ƶ����ٶ�
    public float maxSpeed = 5f; // ����ƶ��ٶ�

    private Vector3 movement; // �ƶ�����
    private bool isMoving = false; // �Ƿ������ƶ�

    void Update()
    {
        HandleInput();
        UpdateMovement();
    }

    void HandleInput()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // ���W��A��S��D����
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

        // �������뷽�������ƶ�����
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // ����Ƿ�������
        isMoving = movement.magnitude > 0f;
    }

    void UpdateMovement()
    {
        if (isMoving)
        {
            // ���ݼ��ٶȺ�����ٶ��������ٶ�
            float speed = Mathf.Min(maxSpeed, acceleration * Time.deltaTime);
            transform.Translate(movement * speed);
        }
        else
        {
            // ���û�����룬�𽥼�С�ٶ�
            transform.Translate(movement * acceleration * Time.deltaTime);
        }
    }

}
