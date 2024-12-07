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

    // ��¼ÿ�����ʵĵ�ǰ�ܽ�ֵ
    private Dictionary<Material, float> dissolveValues = new Dictionary<Material, float>();
    private static readonly string DissolveProperty = "_DissolveThreshold";

    private void Start()
    {
        // ��ʼ���������ֵ�͵�ǰ����ֵ
        maxLife = targetMaterials.Length;
        currentLife = maxLife;

        // ��ʼ�����в��ʵ��ܽ�ֵΪ0
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
        // ȷ��currentLife����Ч��Χ��
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);

        // ����ÿ�����ʵ��ܽ�״̬
        for (int i = 0; i < targetMaterials.Length; i++)
        {
            Material material = targetMaterials[i];
            if (material == null) continue;

            // ȷ����������Ƿ�Ӧ���ܽ�
            bool shouldBeDissolving = i >= currentLife;

            // ��ȡ��ǰ�ܽ�ֵ
            float currentValue = dissolveValues[material];
            float targetValue = shouldBeDissolving ? 1f : 0f;

            // ƽ�������ܽ�ֵ
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

    // ��������ֵ
    public void DecreaseLife()
    {
        if (currentLife > 0)
        {
            controller.TriggerDamageState();
            currentLife--;
        }
    }

    // ��������ֵ
    public void IncreaseLife()
    {
        controller.TriggerHealState();
        if (currentLife < maxLife)
        {
            currentLife++;
        }
    }

    // ���õ���ʼ״̬
    public void Reset()
    {
        currentLife = maxLife;
    }

    // ����Ӧ��Ч������ʹ�ý��䣩
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
