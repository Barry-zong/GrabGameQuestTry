using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        // 获取父物体
        Transform parentTransform = transform.parent;

        // 如果有父物体，销毁父物体（这会自动销毁所有子物体，包括当前物体）
        if (parentTransform != null)
        {
            Destroy(parentTransform.gameObject);
        }
        // 如果没有父物体，就只销毁当前物体
        else
        {
            Destroy(gameObject);
        }
    }
}
