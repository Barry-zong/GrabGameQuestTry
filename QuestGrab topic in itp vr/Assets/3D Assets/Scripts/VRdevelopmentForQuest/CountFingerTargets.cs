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
    {// 使用 Transform 组件来获取所有子对象
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        // 从 Transform 数组中筛选出带有 FingerTarget 标签的对象
        var targets = allChildren
            .Where(t => t.CompareTag("FingerTarget"))
            .ToArray();

        // 获取同级对象
        var siblingTargets = transform.parent != null
            ? transform.parent.GetComponentsInChildren<Transform>(true)
                .Where(t => t.CompareTag("FingerTarget") && t.parent == transform.parent)
                .ToArray()
            : new Transform[0];

        // 计算总数并输出
        int totalCount = targets.Length + siblingTargets.Length;
      //  Debug.Log($"找到 FingerTarget 标签的物体总数: {totalCount}");
      //  Debug.Log($"- 子级物体中的数量: {targets.Length}");
      //  Debug.Log($"- 同级物体中的数量: {siblingTargets.Length}");
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