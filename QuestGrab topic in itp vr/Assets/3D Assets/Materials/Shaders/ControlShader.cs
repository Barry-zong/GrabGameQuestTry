using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrilShader : MonoBehaviour
{
    public float dissolveSpeed = 2f; // 溶解速度控制
    private List<DissolveEffect> dissolveEffects = new List<DissolveEffect>();

    void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            DissolveEffect dissolve = renderer.gameObject.AddComponent<DissolveEffect>();
            dissolve.Init("ScoreDis");
            dissolveEffects.Add(dissolve);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandDetect"))
        {
            foreach (var dissolve in dissolveEffects)
            {
                dissolve.StartDissolve(dissolveSpeed);
            }
        }
    }
}

public class DissolveEffect : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Material instancedMaterial;
    private Coroutine dissolveCoroutine;

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

        Destroy(gameObject); // 溶解完成后销毁游戏物体
    }

    void OnDestroy()
    {
        if (instancedMaterial != null)
            Destroy(instancedMaterial);
    }
}