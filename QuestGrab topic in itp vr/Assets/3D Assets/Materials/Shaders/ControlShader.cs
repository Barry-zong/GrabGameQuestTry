using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrilShader : MonoBehaviour
{
    public bool WhthereExploded = false;
    public float dissolveSpeed = 2f;
    private List<DissolveEffect> dissolveEffects = new List<DissolveEffect>();
    private bool isDissolving = false; // 添加标志位防止重复触发
   
    void Start()
    {
        
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            // 确保每个物体只添加一次DissolveEffect组件
            DissolveEffect existingDissolve = renderer.gameObject.GetComponent<DissolveEffect>();
            if (existingDissolve == null)
            {
                DissolveEffect dissolve = renderer.gameObject.AddComponent<DissolveEffect>();
                dissolve.Init("ScoreDis");
                dissolveEffects.Add(dissolve);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandDetect") && !isDissolving)
        {
            if(WhthereExploded)
            {
                  ExpoldedEffect exploder = GetComponent<ExpoldedEffect>();
                  exploder.TriggerEffect();
            }

            isDissolving = true; // 设置标志位
            foreach (var dissolve in dissolveEffects)
            {
                if (dissolve != null) // 确保组件还存在
                {
                    dissolve.StartDissolve(dissolveSpeed);
                }
            }
        }
    }
}

public class DissolveEffect : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Material instancedMaterial;
    private Coroutine dissolveCoroutine;
    private bool hasStartedDissolve = false; // 添加标志位防止重复触发

    public void Init(string materialName)
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.sharedMaterials;
        foreach (Material mat in materials)
        {
            if (mat.name == materialName || mat.name == materialName + " (Instance)")
            {
                instancedMaterial = new Material(mat);
                meshRenderer.material = instancedMaterial;
                instancedMaterial.SetFloat("_DissolveThreshold", 0);
                return;
            }
        }
    }

    public void StartDissolve(float speed)
    {
        if (hasStartedDissolve) return; // 如果已经开始溶解，直接返回
        hasStartedDissolve = true;

        if (dissolveCoroutine != null)
            StopCoroutine(dissolveCoroutine);
        dissolveCoroutine = StartCoroutine(DissolveRoutine(speed));
    }

    private IEnumerator DissolveRoutine(float speed)
    {
        float dissolveAmount = 0;
        while (dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime * speed;
            instancedMaterial.SetFloat("_DissolveThreshold", Mathf.Clamp01(dissolveAmount));
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (instancedMaterial != null)
            Destroy(instancedMaterial);
    }
}