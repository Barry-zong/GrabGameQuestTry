using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSyncHand : MonoBehaviour
{
    // Start is called before the first frame update
    public OVRHand sourceHand;                // 用户手部对象
    public GameObject targetHandModel;        // 目标手部模型
    private OVRSkeleton sourceSkeleton;       // 用户手部的骨骼信息
    private OVRSkeleton targetSkeleton;       // 目标手部的骨骼信息

    private Dictionary<OVRSkeleton.BoneId, Transform> targetBoneTransforms = new Dictionary<OVRSkeleton.BoneId, Transform>();

    void Start()
    {
        // 获取用户手部的骨骼组件
        sourceSkeleton = sourceHand.GetComponent<OVRSkeleton>();

        // 获取目标手部模型的骨骼组件
        targetSkeleton = targetHandModel.GetComponent<OVRSkeleton>();

        // 建立目标骨骼映射
        foreach (var bone in targetSkeleton.Bones)
        {
            targetBoneTransforms[bone.Id] = bone.Transform;
        }
    }

    void Update()
    {
        // 如果手部骨骼尚未初始化，则跳过
        if (sourceSkeleton.Bones.Count == 0 || targetSkeleton.Bones.Count == 0)
            return;

        // 遍历用户手部的骨骼，并将其旋转同步到目标手部模型
        for (int i = 0; i < sourceSkeleton.Bones.Count; i++)
        {
            var sourceBone = sourceSkeleton.Bones[i];
            if (targetBoneTransforms.TryGetValue(sourceBone.Id, out Transform targetBoneTransform))
            {
                // 将源骨骼的旋转赋值给目标骨骼
                targetBoneTransform.rotation = sourceBone.Transform.rotation;
            }
        }
    }
}
