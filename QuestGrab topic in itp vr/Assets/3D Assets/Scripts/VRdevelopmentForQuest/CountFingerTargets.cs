using System.Linq;
using UnityEngine;

public class CountFingerTargets : MonoBehaviour
{

    BridgeforHandCounter bridgeforHandCounter;
    private bool valueBig = false;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("HandCounter"))
        {
            bridgeforHandCounter = other.GetComponent<BridgeforHandCounter>();
            CountTargets();
        }
       
    }
    public void CountTargets()
    {// ʹ�� Transform �������ȡ�����Ӷ���
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        // �� Transform ������ɸѡ������ FingerTarget ��ǩ�Ķ���
        var targets = allChildren
            .Where(t => t.CompareTag("FingerTarget"))
            .ToArray();

        // ��ȡͬ������
        var siblingTargets = transform.parent != null
            ? transform.parent.GetComponentsInChildren<Transform>(true)
                .Where(t => t.CompareTag("FingerTarget") && t.parent == transform.parent)
                .ToArray()
            : new Transform[0];

        // �������������
        int totalCount = targets.Length + siblingTargets.Length;
      //  Debug.Log($"�ҵ� FingerTarget ��ǩ����������: {totalCount}");
      //  Debug.Log($"- �Ӽ������е�����: {targets.Length}");
      //  Debug.Log($"- ͬ�������е�����: {siblingTargets.Length}");
        if( totalCount > 0 )
        {
            valueBig = true;
        }
        if (valueBig)
        {
            if (totalCount == 0)
            {
                bridgeforHandCounter.canSendValueAdd = true;
                valueBig = false;
            }
        }
    }
}