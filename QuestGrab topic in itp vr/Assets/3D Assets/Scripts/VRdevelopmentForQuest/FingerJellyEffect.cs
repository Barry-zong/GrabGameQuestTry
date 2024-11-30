using UnityEngine;

public class FingerJellyEffect : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float springForce = 140f;           // ��������
    [SerializeField] private float damping = 5f;                 // ����
    [SerializeField] private float maxAngle = 40f;              // ���ڶ��Ƕ�
    [SerializeField] private Vector3 springDirection = Vector3.forward; // �ڶ�����

    [Header("References")]
    [SerializeField] private Transform targetBone;               // Ŀ�����

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
        // ���㵯����
        Vector3 springForceVector = -springForce * currentRotation;

        // Ӧ������
        Vector3 dampingForce = -damping * currentAngularVelocity;

        // ������ٶ�
        Vector3 acceleration = (springForceVector + dampingForce);

        // �����ٶȺ�λ��
        currentAngularVelocity += acceleration * Time.deltaTime;
        currentRotation += currentAngularVelocity * Time.deltaTime;

        // �������Ƕ�
        currentRotation = Vector3.ClampMagnitude(currentRotation, maxAngle);

        // Ӧ����ת
        Vector3 finalRotation = Vector3.Scale(currentRotation, springDirection);
        targetBone.localRotation = initialRotation * Quaternion.Euler(finalRotation);
    }

    // �������
    public void AddForce(Vector3 force)
    {
        currentAngularVelocity += force;
    }

    // ����λ��
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
    [SerializeField] private FingerJellyEffect[] fingerTips;    // ������ָ��˵Ĺ���Ч��
    [SerializeField] private float velocityMultiplier = 0.1f;   // �ٶȳ���

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
            // �����ٶ�
            Vector3 velocity = (fingerTips[i].transform.position - previousPositions[i]) / Time.deltaTime;

            // �����
            if (velocity.magnitude > 0.01f)
            {
                fingerTips[i].AddForce(velocity * velocityMultiplier);
            }

            previousPositions[i] = fingerTips[i].transform.position;
        }
    }
}