using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotateHeart : MonoBehaviour
{
    // 旋转速度(度/秒)
    public float rotationSpeed = 180f; // 加快速度以便快速完成单次旋转

    // 旋转间隔时间(秒)
    public float rotationInterval = 3f;

    // 选择要旋转的轴向
    public bool rotateX = false;
    public bool rotateY = true;
    public bool rotateZ = false;

    // 计时器
    private float timer = 0f;
    // 是否正在旋转
    private bool isRotating = false;
    // 当前旋转角度
    private float currentRotation = 0f;

    void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;

        // 检查是否到达间隔时间
        if (timer >= rotationInterval)
        {
            isRotating = true;
            timer = 0f; // 重置计时器
        }

        // 执行旋转
        if (isRotating)
        {
            // 构建旋转向量
            Vector3 rotation = new Vector3(
                rotateX ? rotationSpeed : 0f,
                rotateY ? rotationSpeed : 0f,
                rotateZ ? rotationSpeed : 0f
            );

            // 应用本地旋转
            transform.Rotate(rotation * Time.deltaTime, Space.Self);

            // 累计已旋转角度
            currentRotation += rotationSpeed * Time.deltaTime;

            // 检查是否完成一次完整旋转
            if (currentRotation >= 180f)
            {
                isRotating = false;
                currentRotation = 0f;
            }
        }
    }
    }
