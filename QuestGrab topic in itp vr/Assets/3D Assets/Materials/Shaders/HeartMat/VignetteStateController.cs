using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteStateController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume globalVolume;

    [Header("Effect Duration")]
    [SerializeField] private float effectDuration = 0.5f;

    // Ĭ��״̬����
    private readonly Color defaultColor = new Color(0.07f, 0.16f, 0.33f); // #132A54
    private const float defaultIntensity = 0.35f;
    private const float defaultSmoothness = 0.25f;

    // ��Ѫ״̬����
    private readonly Color damageColor = new Color(0.69f, 0f, 0f); // #B00000
    private const float damageIntensity = 0.75f;
    private const float damageSmoothness = 0.8f;

    // ��Ѫ״̬����
    private readonly Color healColor = new Color(0.035f, 1f, 0.23f); // #09FF3B
    private const float healIntensity = 0.75f;
    private const float healSmoothness = 0.8f;

    private Vignette vignette;
    private float currentEffectTime = 0f;
    private bool isTransitioning = false;

    // ���ڴ洢������ʼֵ
    private Color startColor;
    private float startIntensity;
    private float startSmoothness;

    // ���ڴ洢Ŀ��ֵ
    private Color targetColor;
    private float targetIntensity;
    private float targetSmoothness;

    private void Start()
    {
        // ��ȡVolume���
        if (globalVolume == null)
        {
            globalVolume = GetComponent<Volume>();
        }

        // ��ȡVignetteЧ��
        if (globalVolume.profile.TryGet(out Vignette vig))
        {
            vignette = vig;
        }
        else
        {
            Debug.LogError("Vignette effect not found in Volume profile!");
            return;
        }

        // ���ó�ʼĬ��״̬
        SetDefaultState(true);
    }

    private void Update()
    {
        if (isTransitioning)
        {
            currentEffectTime += Time.deltaTime;
            float t = currentEffectTime / effectDuration;

            if (t <= 1f)
            {
                // ʹ��ƽ���Ĺ���Ч��
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                // �������в���
                vignette.color.value = Color.Lerp(startColor, targetColor, smoothT);
                vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, smoothT);
                vignette.smoothness.value = Mathf.Lerp(startSmoothness, targetSmoothness, smoothT);
            }
            else
            {
                isTransitioning = false;
                if (targetColor != defaultColor)
                {
                    // ������ǹ��ɵ�Ĭ��״̬����ʼ���ɻ�Ĭ��״̬
                    StartTransition(defaultColor, defaultIntensity, defaultSmoothness);
                }
            }
        }
    }

    // ��ʼ״̬����
    private void StartTransition(Color targetCol, float targetInt, float targetSmooth)
    {
        isTransitioning = true;
        currentEffectTime = 0f;

        // �洢��ǰ״̬��Ϊ��ʼֵ
        startColor = vignette.color.value;
        startIntensity = vignette.intensity.value;
        startSmoothness = vignette.smoothness.value;

        // ����Ŀ��ֵ
        targetColor = targetCol;
        targetIntensity = targetInt;
        targetSmoothness = targetSmooth;
    }

    // ����Ĭ��״̬
    public void SetDefaultState(bool immediate = false)
    {
        if (immediate)
        {
            vignette.color.value = defaultColor;
            vignette.intensity.value = defaultIntensity;
            vignette.smoothness.value = defaultSmoothness;
            isTransitioning = false;
        }
        else
        {
            StartTransition(defaultColor, defaultIntensity, defaultSmoothness);
        }
    }

    // ������ѪЧ��
    public void TriggerDamageState()
    {
        StartTransition(damageColor, damageIntensity, damageSmoothness);
    }

    // ������ѪЧ��
    public void TriggerHealState()
    {
        StartTransition(healColor, healIntensity, healSmoothness);
    }
}