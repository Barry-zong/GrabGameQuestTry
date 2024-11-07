using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCopyer : MonoBehaviour
{
    public GameObject targetObject; // 声明一个用于复制坐标的目标物体

    private void Update()
    {
        // 检查目标物体是否存在
        if (targetObject != null)
        {
            // 复制目标物体的位置和旋转到当前物体
            transform.position = targetObject.transform.position;
            
        }
    }
}
