using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fingerDetectMeta : MonoBehaviour
{
    public OVRHand leftHand;   // ���ֶ���
    public OVRHand rightHand;  // ���ֶ���

    // ���ڱ���˫��ʮ����ָ�������ȣ���˳������Ĵָ��Сָ������Ĵָ��Сָ
    public float[] fingerBendValues = new float[10];

    void Update()
    {
        // �������������ָ��������
        fingerBendValues[0] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Thumb);
        fingerBendValues[1] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Index);
        fingerBendValues[2] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Middle);
        fingerBendValues[3] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Ring);
        fingerBendValues[4] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Pinky);

        // �������������ָ��������
        fingerBendValues[5] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Thumb);
        fingerBendValues[6] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Index);
        fingerBendValues[7] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Middle);
        fingerBendValues[8] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Ring);
        fingerBendValues[9] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Pinky);
    }

    // ��������������ָ����ָ��������
    private float GetFingerBendValue(OVRHand hand, OVRHand.HandFinger finger)
    {
        return hand.GetFingerPinchStrength(finger); // ��ȡ�����̶ȣ���ΧΪ0��1
    }
}

