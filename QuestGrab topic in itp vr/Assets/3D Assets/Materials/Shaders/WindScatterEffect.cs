using UnityEngine;

public class WindScatterEffect : MonoBehaviour
{
    [Header("Basic Settings")]
    [SerializeField] private float initialUpwardForce = 2f;    // ��ʼ������
    [SerializeField] private float scatterForce = 3f;          // ɢ������
    [SerializeField] private float windStrength = 2f;          // ����ǿ��
    [SerializeField] private float windChangeSpeed = 0.5f;     // ����仯�ٶ�

    [Header("Random Settings")]
    [SerializeField] private float randomRotationForce = 100f; // �����ת��
    [SerializeField] private float turbulence = 0.5f;          // ����ǿ��

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

        // Ϊÿ���������������ʱ��ƫ�ƣ�ʹ�����ǵ��˶���ͬ��
        timeOffset = Random.Range(0f, 100f);
    }

    private void InitializeMovement()
    {
        // ��ӳ�ʼ���ϵ���
        Vector3 initialForce = Vector3.up * initialUpwardForce;

        // �����΢�����ˮƽƫ��
        initialForce += new Vector3(
            Random.Range(-0.5f, 0.5f),
            0,
            Random.Range(-0.5f, 0.5f)
        ) * scatterForce;

        rb.AddForce(initialForce, ForceMode.Impulse);

        // �������ĳ�ʼ��ת
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
        // ʹ�ð�����������ƽ���仯�ķ���
        float time = Time.time * windChangeSpeed + timeOffset;
        windDirection = new Vector3(
            Mathf.PerlinNoise(time, 0) - 0.5f,
            Mathf.PerlinNoise(time, 1000) * 0.2f, // ��ֱ����ķ��С
            Mathf.PerlinNoise(time, 2000) - 0.5f
        ).normalized;

        // Ӧ�÷���
        rb.AddForce(windDirection * windStrength);

        // ����ʱ���С����Ӱ�죬ʹ��������ױ��紵��
        rb.useGravity = true;
        rb.drag = Mathf.Lerp(rb.drag, 2f, Time.deltaTime); // �����ӿ�������
    }

    private void AddTurbulence()
    {
        // ������������Ч��
        Vector3 turbulenceForce = new Vector3(
            Mathf.PerlinNoise(Time.time + timeOffset, 0) - 0.5f,
            Mathf.PerlinNoise(Time.time + timeOffset, 1000) - 0.5f,
            Mathf.PerlinNoise(Time.time + timeOffset, 2000) - 0.5f
        ) * turbulence;

        rb.AddForce(turbulenceForce);
    }
}