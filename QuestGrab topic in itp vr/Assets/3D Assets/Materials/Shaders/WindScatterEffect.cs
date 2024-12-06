using UnityEngine;

public class WindScatterEffect : MonoBehaviour
{
    [Header("Basic Settings")]
    [SerializeField] private float initialUpwardForce = 2f;    // 初始向上力
    [SerializeField] private float scatterForce = 3f;          // 散开力度
    [SerializeField] private float windStrength = 2f;          // 风力强度
    [SerializeField] private float windChangeSpeed = 0.5f;     // 风向变化速度

    [Header("Random Settings")]
    [SerializeField] private float randomRotationForce = 100f; // 随机旋转力
    [SerializeField] private float turbulence = 0.5f;          // 湍流强度

    private Rigidbody rb;
    private Vector3 windDirection;
    private float timeOffset;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb)
        {
            InitializeMovement();
        }

        // 为每个物体设置随机的时间偏移，使得它们的运动不同步
        timeOffset = Random.Range(0f, 100f);
    }

    private void InitializeMovement()
    {
        // 添加初始向上的力
        Vector3 initialForce = Vector3.up * initialUpwardForce;

        // 添加轻微的随机水平偏移
        initialForce += new Vector3(
            Random.Range(-0.5f, 0.5f),
            0,
            Random.Range(-0.5f, 0.5f)
        ) * scatterForce;

        rb.AddForce(initialForce, ForceMode.Impulse);

        // 添加随机的初始旋转
        rb.AddTorque(Random.insideUnitSphere * randomRotationForce);
    }

    private void FixedUpdate()
    {
        if (!rb) return;

        UpdateWindEffect();
        AddTurbulence();
    }

    private void UpdateWindEffect()
    {
        // 使用柏林噪声创建平滑变化的风向
        float time = Time.time * windChangeSpeed + timeOffset;
        windDirection = new Vector3(
            Mathf.PerlinNoise(time, 0) - 0.5f,
            Mathf.PerlinNoise(time, 1000) * 0.2f, // 垂直方向的风较小
            Mathf.PerlinNoise(time, 2000) - 0.5f
        ).normalized;

        // 应用风力
        rb.AddForce(windDirection * windStrength);

        // 随着时间减小重力影响，使物体更容易被风吹动
        rb.useGravity = true;
        rb.drag = Mathf.Lerp(rb.drag, 2f, Time.deltaTime); // 逐渐增加空气阻力
    }

    private void AddTurbulence()
    {
        // 添加随机的湍流效果
        Vector3 turbulenceForce = new Vector3(
            Mathf.PerlinNoise(Time.time + timeOffset, 0) - 0.5f,
            Mathf.PerlinNoise(Time.time + timeOffset, 1000) - 0.5f,
            Mathf.PerlinNoise(Time.time + timeOffset, 2000) - 0.5f
        ) * turbulence;

        rb.AddForce(turbulenceForce);
    }
}