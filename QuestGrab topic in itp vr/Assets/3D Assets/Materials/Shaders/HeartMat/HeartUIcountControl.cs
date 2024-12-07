using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUIcountControl : MonoBehaviour
{

    [SerializeField] private Material[] targetMaterials;
    [SerializeField] private float dissolveSpeed = 1f;
    public VignetteStateController controller;

    private int maxLife;
    public int currentLife;

    // 记录每个材质的当前溶解值
    private Dictionary<Material, float> dissolveValues = new Dictionary<Material, float>();
    private static readonly string DissolveProperty = "_DissolveThreshold";

    private void Start()
    {
        // 初始化最大生命值和当前生命值
        maxLife = targetMaterials.Length;
        currentLife = maxLife;

        // 初始化所有材质的溶解值为0
        foreach (var material in targetMaterials)
        {
            if (material != null)
            {
                dissolveValues[material] = 0f;
                material.SetFloat(DissolveProperty, 0f);
            }
        }
    }

    private void Update()
    {
        // 确保currentLife在有效范围内
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);

        // 更新每个材质的溶解状态
        for (int i = 0; i < targetMaterials.Length; i++)
        {
            Material material = targetMaterials[i];
            if (material == null) continue;

            // 确定这个材质是否应该溶解
            bool shouldBeDissolving = i >= currentLife;

            // 获取当前溶解值
            float currentValue = dissolveValues[material];
            float targetValue = shouldBeDissolving ? 1f : 0f;

            // 平滑更新溶解值
            if (currentValue != targetValue)
            {
                if (shouldBeDissolving)
                {
                    currentValue = Mathf.Min(currentValue + dissolveSpeed * Time.deltaTime, targetValue);
                }
                else
                {
                    currentValue = Mathf.Max(currentValue - dissolveSpeed * Time.deltaTime, targetValue);
                }

                dissolveValues[material] = currentValue;
                material.SetFloat(DissolveProperty, currentValue);
            }
        }
    }

    // 减少生命值
    public void DecreaseLife()
    {
        if (currentLife > 0)
        {
            controller.TriggerDamageState();
            currentLife--;
        }
    }

    // 增加生命值
    public void IncreaseLife()
    {
        controller.TriggerHealState();
        if (currentLife < maxLife)
        {
            currentLife++;
        }
    }

    // 重置到初始状态
    public void Reset()
    {
        currentLife = maxLife;
    }

    // 立即应用效果（不使用渐变）
    public void ApplyImmediate()
    {
        for (int i = 0; i < targetMaterials.Length; i++)
        {
            if (targetMaterials[i] != null)
            {
                float value = i >= currentLife ? 1f : 0f;
                dissolveValues[targetMaterials[i]] = value;
                targetMaterials[i].SetFloat(DissolveProperty, value);
            }
        }
    }
}
