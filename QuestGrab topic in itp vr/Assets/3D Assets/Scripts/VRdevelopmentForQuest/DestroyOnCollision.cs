using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        // ��ȡ������
        Transform parentTransform = transform.parent;

        // ����и����壬���ٸ����壨����Զ��������������壬������ǰ���壩
        if (parentTransform != null)
        {
            Destroy(parentTransform.gameObject);
        }
        // ���û�и����壬��ֻ���ٵ�ǰ����
        else
        {
            Destroy(gameObject);
        }
    }
}
