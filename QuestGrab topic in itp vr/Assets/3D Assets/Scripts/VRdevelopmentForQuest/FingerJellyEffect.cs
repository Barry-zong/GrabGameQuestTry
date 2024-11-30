using UnityEngine;

public class FingerJellyEffect : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float springForce = 140f;           // 弹簧力度
    [SerializeField] private float damping = 5f;                 // 阻尼
    [SerializeField] private float maxAngle = 40f;              // 最大摆动角度
    [SerializeField] private Vector3 springDirection = Vector3.forward; // 摆动方向

    [Header("References")]
    [SerializeField] private Transform targetBone;               // 目标骨骼

    private Vector3 currentRotation;
    private Vector3 currentAngularVelocity;
    private Quaternion initialRotation;

    private void Start()
    {
        if (targetBone == null)
        {
            targetBone = transform;
        }
        initialRotation = targetBone.localRotation;
    }

    private void Update()
    {
        // 计算弹簧力
        Vector3 springForceVector = -springForce * currentRotation;

        // 应用阻尼
        Vector3 dampingForce = -damping * currentAngularVelocity;

        // 计算加速度
        Vector3 acceleration = (springForceVector + dampingForce);

        // 更新速度和位置
        currentAngularVelocity += acceleration * Time.deltaTime;
        currentRotation += currentAngularVelocity * Time.deltaTime;

        // 限制最大角度
        currentRotation = Vector3.ClampMagnitude(currentRotation, maxAngle);

        // 应用旋转
        Vector3 finalRotation = Vector3.Scale(currentRotation, springDirection);
        targetBone.localRotation = initialRotation * Quaternion.Euler(finalRotation);
    }

    // 添加外力
    public void AddForce(Vector3 force)
    {
        currentAngularVelocity += force;
    }

    // 重置位置
    public void Reset()
    {
        currentRotation = Vector3.zero;
        currentAngularVelocity = Vector3.zero;
        targetBone.localRotation = initialRotation;
    }
}

[System.Serializable]
public class FingerTipController : MonoBehaviour
{
    [SerializeField] private FingerJellyEffect[] fingerTips;    // 所有手指尖端的果冻效果
    [SerializeField] private float velocityMultiplier = 0.1f;   // 速度乘数

    private Vector3[] previousPositions;
    private void Start()
    {
        previousPositions = new Vector3[fingerTips.Length];
        for (int i = 0; i < fingerTips.Length; i++)
        {
            previousPositions[i] = fingerTips[i].transform.position;
        }
    }

    private void Update()
    {
        for (int i = 0; i < fingerTips.Length; i++)
        {
            // 计算速度
            Vector3 velocity = (fingerTips[i].transform.position - previousPositions[i]) / Time.deltaTime;

            // 添加力
            if (velocity.magnitude > 0.01f)
            {
                fingerTips[i].AddForce(velocity * velocityMultiplier);
            }

            previousPositions[i] = fingerTips[i].transform.position;
        }
    }
}