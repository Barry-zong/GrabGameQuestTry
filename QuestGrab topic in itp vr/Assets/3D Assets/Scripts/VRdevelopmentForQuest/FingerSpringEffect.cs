using UnityEngine;

public class FingerSpringEffect : MonoBehaviour
{
    [System.Serializable]
    public class SpringTarget
    {
        public Transform ikTarget;        // ԭʼIKĿ�꣨С��
        public Transform springTarget;    // ����Ŀ�꣨�½��ĸ������壩
        [HideInInspector]
        public Vector3 velocity;          // �ٶ�
        [HideInInspector]
        public Quaternion rotationVelocity; // ��ת�ٶ�
        public IKtargetPositionFollow ikFollowScript;
    }

    public SpringTarget[] fingers;        // ÿ����ָ�ĵ���Ŀ������
    public float springStrength = 20f;    // ����ǿ��
    public float damping = 5f;           // ����
    public float rotationSpringStrength = 15f; // ��ת����ǿ��
    public float rotationDamping = 4f;    // ��ת����
    public float maxStretch = 0.1f;      // ����������

    void Start()
    {
        // Ϊÿ��IKĿ�괴�����Ը�������
        for (int i = 0; i < fingers.Length; i++)
        {
            // ����springTarget
            GameObject springObj = new GameObject("SpringTarget_" + i);
            fingers[i].springTarget = springObj.transform;
            springObj.transform.SetParent(transform);
            springObj.transform.position = fingers[i].ikTarget.position;
            springObj.transform.rotation = fingers[i].ikTarget.rotation;

            // �ҵ�������IKtargetPositionFollow
            if (fingers[i].ikTarget != null)
            {
                // ��IKĿ�������ϻ�ȡIKtargetPositionFollow���
                fingers[i].ikFollowScript = fingers[i].ikTarget.GetComponent<IKtargetPositionFollow>();
                if (fingers[i].ikFollowScript != null)
                {
                    // ����followedObjectΪ�´�����springTarget
                    fingers[i].ikFollowScript.FollowedObject = fingers[i].springTarget;
                }
            }
        }
    }

    void LateUpdate()
    {
        foreach (var finger in fingers)
        {
            // λ�õ���
            Vector3 targetPos = finger.ikTarget.position;
            Vector3 currentPos = finger.springTarget.position;

            // ���㵯��
            Vector3 force = (targetPos - currentPos) * springStrength;
            // Ӧ������
            finger.velocity = Vector3.Lerp(finger.velocity, Vector3.zero, damping * Time.deltaTime);
            // �����
            finger.velocity += force * Time.deltaTime;

            // �����������
            if (Vector3.Distance(targetPos, currentPos) > maxStretch)
            {
                Vector3 direction = (targetPos - currentPos).normalized;
                currentPos = targetPos - direction * maxStretch;
                finger.velocity = Vector3.zero;
            }

            // ����λ��
            finger.springTarget.position = currentPos + finger.velocity * Time.deltaTime;

            // ��ת����
            Quaternion targetRot = finger.ikTarget.rotation;
            Quaternion currentRot = finger.springTarget.rotation;

            // ������ת��ֵ
            Quaternion rotationDelta = targetRot * Quaternion.Inverse(currentRot);
            float angle;
            Vector3 axis;
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180f) angle -= 360f;

            // Ӧ����ת����
            Vector3 rotationForce = (axis * angle * Mathf.Deg2Rad) * rotationSpringStrength;
            finger.rotationVelocity = Quaternion.Lerp(finger.rotationVelocity, Quaternion.identity, rotationDamping * Time.deltaTime);
            finger.springTarget.rotation = finger.springTarget.rotation * Quaternion.Euler(rotationForce * Time.deltaTime);
        }
    }
}