using UnityEngine;

public class FingerSpringEffect : MonoBehaviour
{
    [System.Serializable]
    public class SpringTarget
    {
        public Transform ikTarget;        // 原始IK目标（小球）
        public Transform springTarget;    // 弹性目标（新建的跟随物体）
        [HideInInspector]
        public Vector3 velocity;          // 速度
        [HideInInspector]
        public Quaternion rotationVelocity; // 旋转速度
        public IKtargetPositionFollow ikFollowScript;
    }

    public SpringTarget[] fingers;        // 每个手指的弹性目标配置
    public float springStrength = 20f;    // 弹簧强度
    public float damping = 5f;           // 阻尼
    public float rotationSpringStrength = 15f; // 旋转弹簧强度
    public float rotationDamping = 4f;    // 旋转阻尼
    public float maxStretch = 0.1f;      // 最大拉伸距离

    void Start()
    {
        // 为每个IK目标创建弹性跟随物体
        for (int i = 0; i < fingers.Length; i++)
        {
            // 创建springTarget
            GameObject springObj = new GameObject("SpringTarget_" + i);
            fingers[i].springTarget = springObj.transform;
            springObj.transform.SetParent(transform);
            springObj.transform.position = fingers[i].ikTarget.position;
            springObj.transform.rotation = fingers[i].ikTarget.rotation;

            // 找到并设置IKtargetPositionFollow
            if (fingers[i].ikTarget != null)
            {
                // 从IK目标物体上获取IKtargetPositionFollow组件
                fingers[i].ikFollowScript = fingers[i].ikTarget.GetComponent<IKtargetPositionFollow>();
                if (fingers[i].ikFollowScript != null)
                {
                    // 设置followedObject为新创建的springTarget
                    fingers[i].ikFollowScript.FollowedObject = fingers[i].springTarget;
                }
            }
        }
    }

    void LateUpdate()
    {
        foreach (var finger in fingers)
        {
            // 位置弹性
            Vector3 targetPos = finger.ikTarget.position;
            Vector3 currentPos = finger.springTarget.position;

            // 计算弹力
            Vector3 force = (targetPos - currentPos) * springStrength;
            // 应用阻尼
            finger.velocity = Vector3.Lerp(finger.velocity, Vector3.zero, damping * Time.deltaTime);
            // 添加力
            finger.velocity += force * Time.deltaTime;

            // 限制最大拉伸
            if (Vector3.Distance(targetPos, currentPos) > maxStretch)
            {
                Vector3 direction = (targetPos - currentPos).normalized;
                currentPos = targetPos - direction * maxStretch;
                finger.velocity = Vector3.zero;
            }

            // 更新位置
            finger.springTarget.position = currentPos + finger.velocity * Time.deltaTime;

            // 旋转弹性
            Quaternion targetRot = finger.ikTarget.rotation;
            Quaternion currentRot = finger.springTarget.rotation;

            // 计算旋转差值
            Quaternion rotationDelta = targetRot * Quaternion.Inverse(currentRot);
            float angle;
            Vector3 axis;
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180f) angle -= 360f;

            // 应用旋转弹力
            Vector3 rotationForce = (axis * angle * Mathf.Deg2Rad) * rotationSpringStrength;
            finger.rotationVelocity = Quaternion.Lerp(finger.rotationVelocity, Quaternion.identity, rotationDamping * Time.deltaTime);
            finger.springTarget.rotation = finger.springTarget.rotation * Quaternion.Euler(rotationForce * Time.deltaTime);
        }
    }
}