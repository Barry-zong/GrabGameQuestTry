using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneSyncHand : MonoBehaviour
{
    // Start is called before the first frame update
    public OVRHand sourceHand;                // �û��ֲ�����
    public GameObject targetHandModel;        // Ŀ���ֲ�ģ��
    private OVRSkeleton sourceSkeleton;       // �û��ֲ��Ĺ�����Ϣ
    private OVRSkeleton targetSkeleton;       // Ŀ���ֲ��Ĺ�����Ϣ

    private Dictionary<OVRSkeleton.BoneId, Transform> targetBoneTransforms = new Dictionary<OVRSkeleton.BoneId, Transform>();

    void Start()
    {
        // ��ȡ�û��ֲ��Ĺ������
        sourceSkeleton = sourceHand.GetComponent<OVRSkeleton>();

        // ��ȡĿ���ֲ�ģ�͵Ĺ������
        targetSkeleton = targetHandModel.GetComponent<OVRSkeleton>();

        // ����Ŀ�����ӳ��
        foreach (var bone in targetSkeleton.Bones)
        {
            targetBoneTransforms[bone.Id] = bone.Transform;
        }
    }

    void Update()
    {
        // ����ֲ�������δ��ʼ����������
        if (sourceSkeleton.Bones.Count == 0 || targetSkeleton.Bones.Count == 0)
            return;

        // �����û��ֲ��Ĺ�������������תͬ����Ŀ���ֲ�ģ��
        for (int i = 0; i < sourceSkeleton.Bones.Count; i++)
        {
            var sourceBone = sourceSkeleton.Bones[i];
            if (targetBoneTransforms.TryGetValue(sourceBone.Id, out Transform targetBoneTransform))
            {
                // ��Դ��������ת��ֵ��Ŀ�����
                targetBoneTransform.rotation = sourceBone.Transform.rotation;
            }
        }
    }
}
