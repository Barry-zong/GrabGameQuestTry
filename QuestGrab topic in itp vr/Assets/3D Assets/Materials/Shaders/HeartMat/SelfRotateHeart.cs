using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotateHeart : MonoBehaviour
{
    // ��ת�ٶ�(��/��)
    public float rotationSpeed = 180f; // �ӿ��ٶ��Ա������ɵ�����ת

    // ��ת���ʱ��(��)
    public float rotationInterval = 3f;

    // ѡ��Ҫ��ת������
    public bool rotateX = false;
    public bool rotateY = true;
    public bool rotateZ = false;

    // ��ʱ��
    private float timer = 0f;
    // �Ƿ�������ת
    private bool isRotating = false;
    // ��ǰ��ת�Ƕ�
    private float currentRotation = 0f;

    void Update()
    {
        // ���¼�ʱ��
        timer += Time.deltaTime;

        // ����Ƿ񵽴���ʱ��
        if (timer >= rotationInterval)
        {
            isRotating = true;
            timer = 0f; // ���ü�ʱ��
        }

        // ִ����ת
        if (isRotating)
        {
            // ������ת����
            Vector3 rotation = new Vector3(
                rotateX ? rotationSpeed : 0f,
                rotateY ? rotationSpeed : 0f,
                rotateZ ? rotationSpeed : 0f
            );

            // Ӧ�ñ�����ת
            transform.Rotate(rotation * Time.deltaTime, Space.Self);

            // �ۼ�����ת�Ƕ�
            currentRotation += rotationSpeed * Time.deltaTime;

            // ����Ƿ����һ��������ת
            if (currentRotation >= 180f)
            {
                isRotating = false;
                currentRotation = 0f;
            }
        }
    }
    }
