using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fingerDetectMeta : MonoBehaviour
{
    public OVRHand leftHand;   // 左手对象
    public OVRHand rightHand;  // 右手对象

    // 用于保存双手十根手指的弯曲度，按顺序：左手拇指到小指、右手拇指到小指
    public float[] fingerBendValues = new float[10];

    void Update()
    {
        // 更新左手五根手指的弯曲度
        fingerBendValues[0] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Thumb);
        fingerBendValues[1] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Index);
        fingerBendValues[2] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Middle);
        fingerBendValues[3] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Ring);
        fingerBendValues[4] = GetFingerBendValue(leftHand, OVRHand.HandFinger.Pinky);

        // 更新右手五根手指的弯曲度
        fingerBendValues[5] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Thumb);
        fingerBendValues[6] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Index);
        fingerBendValues[7] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Middle);
        fingerBendValues[8] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Ring);
        fingerBendValues[9] = GetFingerBendValue(rightHand, OVRHand.HandFinger.Pinky);
    }

    // 辅助方法，返回指定手指的弯曲度
    private float GetFingerBendValue(OVRHand hand, OVRHand.HandFinger finger)
    {
        return hand.GetFingerPinchStrength(finger); // 获取弯曲程度，范围为0到1
    }
}

