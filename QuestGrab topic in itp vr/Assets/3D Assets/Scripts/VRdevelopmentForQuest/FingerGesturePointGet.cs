using UnityEngine;

public class HandGestureManager : MonoBehaviour
{
    public static HandGestureManager Instance { get; private set; }

    public OVRHand leftHand;
    public OVRHand rightHand;

    public float[] leftFingerBends = new float[5];
    public float[] rightFingerBends = new float[5];

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
      
    }

    void Update()
    {
        UpdateHandData(leftHand, leftFingerBends, "Left");
        UpdateHandData(rightHand, rightFingerBends, "Right");
    }

    private void UpdateHandData(OVRHand hand, float[] fingerBends, string handName)
    {
        if (hand != null && hand.IsTracked)
        {
            fingerBends[0] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);
            fingerBends[1] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
            fingerBends[2] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Middle);
            fingerBends[3] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Ring);
            fingerBends[4] = hand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky);

          //  Debug.Log($"{handName} Hand - Thumb: {fingerBends[0]:F2}, Index: {fingerBends[1]:F2}, " +
                   //  $"Middle: {fingerBends[2]:F2}, Ring: {fingerBends[3]:F2}, Pinky: {fingerBends[4]:F2}");
        }
    }

    // ��ȡָ���ֵ�ָ����ָ������
    public float GetFingerBend(bool isRightHand, int fingerIndex)
    {
        if (fingerIndex >= 0 && fingerIndex < 5)
        {
            return isRightHand ? rightFingerBends[fingerIndex] : leftFingerBends[fingerIndex];
        }
        return 0f;
    }

    // ������Ƿ�׷��
    public bool IsHandTracked(bool isRightHand)
    {
        return isRightHand ?
            (rightHand != null && rightHand.IsTracked) :
            (leftHand != null && leftHand.IsTracked);
    }
}