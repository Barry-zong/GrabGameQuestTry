using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerValueDetectRight : MonoBehaviour
{

    public OVRHand hand;
    public bool isRightHand = true;
    private float[] fingerBends = new float[5];  // 存储五根手指的弯曲度

    void Update()
    {
        if (hand != null && hand.IsTracked)
        {
            CalculateAllFingerBends();
            DisplayFingerBends();
        }
    }

    void CalculateAllFingerBends()
    {
        OVRSkeleton skeleton = hand.GetComponent<OVRSkeleton>();

        // 拇指
        fingerBends[0] = CalculateThumbBend(skeleton)*360;

        // 食指
        fingerBends[1] = CalculateFingerBend(skeleton,
            OVRSkeleton.BoneId.Hand_Index1,
            OVRSkeleton.BoneId.Hand_Index2,
            OVRSkeleton.BoneId.Hand_Index3)*300;

        // 中指
        fingerBends[2] = CalculateFingerBend(skeleton,
            OVRSkeleton.BoneId.Hand_Middle1,
            OVRSkeleton.BoneId.Hand_Middle2,
            OVRSkeleton.BoneId.Hand_Middle3)*260;

        // 无名指
        fingerBends[3] = CalculateFingerBend(skeleton,
            OVRSkeleton.BoneId.Hand_Ring1,
            OVRSkeleton.BoneId.Hand_Ring2,
            OVRSkeleton.BoneId.Hand_Ring3)*300;

        // 小指
        fingerBends[4] = CalculateFingerBend(skeleton,
            OVRSkeleton.BoneId.Hand_Pinky1,
            OVRSkeleton.BoneId.Hand_Pinky2,
            OVRSkeleton.BoneId.Hand_Pinky3)*250;
    }

    float CalculateFingerBend(OVRSkeleton skeleton, OVRSkeleton.BoneId proximal, OVRSkeleton.BoneId intermediate, OVRSkeleton.BoneId distal)
    {
        Vector3 proximalPos = skeleton.Bones[(int)proximal].Transform.position;
        Vector3 intermediatePos = skeleton.Bones[(int)intermediate].Transform.position;
        Vector3 distalPos = skeleton.Bones[(int)distal].Transform.position;

        Vector3 vectorA = intermediatePos - proximalPos;
        Vector3 vectorB = distalPos - intermediatePos;

        float angle = Vector3.Angle(vectorA, vectorB);
        return Mathf.Clamp01(angle / 180f);
    }

    float CalculateThumbBend(OVRSkeleton skeleton)
    {
        return CalculateFingerBend(skeleton,
            OVRSkeleton.BoneId.Hand_Thumb1,
            OVRSkeleton.BoneId.Hand_Thumb2,
            OVRSkeleton.BoneId.Hand_Thumb3);
    }

    void DisplayFingerBends()
    {
        string handName = isRightHand ? "Right" : "Left";
      //  Debug.Log($"{handName} Hand Bends - Thumb: {fingerBends[0]:F2}, Index: {fingerBends[1]:F2}, " +
        //         $"Middle: {fingerBends[2]:F2}, Ring: {fingerBends[3]:F2}, Pinky: {fingerBends[4]:F2}");
    }

    // 获取指定手指的弯曲度
    public float GetFingerBend(int fingerIndex)
    {
        if (fingerIndex >= 0 && fingerIndex < 5)
            return fingerBends[fingerIndex];
        return 0f;
    }
}
