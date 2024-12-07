using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteStateController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume globalVolume;

    [Header("Effect Duration")]
    [SerializeField] private float effectDuration = 0.5f;

    // 默认状态参数
    private readonly Color defaultColor = new Color(0.07f, 0.16f, 0.33f); // #132A54
    private const float defaultIntensity = 0.35f;
    private const float defaultSmoothness = 0.25f;

    // 扣血状态参数
    private readonly Color damageColor = new Color(0.69f, 0f, 0f); // #B00000
    private const float damageIntensity = 0.75f;
    private const float damageSmoothness = 0.8f;

    // 回血状态参数
    private readonly Color healColor = new Color(0.035f, 1f, 0.23f); // #09FF3B
    private const float healIntensity = 0.75f;
    private const float healSmoothness = 0.8f;

    private Vignette vignette;
    private float currentEffectTime = 0f;
    private bool isTransitioning = false;

    // 用于存储过渡起始值
    private Color startColor;
    private float startIntensity;
    private float startSmoothness;

    // 用于存储目标值
    private Color targetColor;
    private float targetIntensity;
    private float targetSmoothness;

    private void Start()
    {
        // 获取Volume组件
        if (globalVolume == null)
        {
            globalVolume = GetComponent<Volume>();
        }

        // 获取Vignette效果
        if (globalVolume.profile.TryGet(out Vignette vig))
        {
            vignette = vig;
        }
        else
        {
            Debug.LogError("Vignette effect not found in Volume profile!");
            return;
        }

        // 设置初始默认状态
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
                // 使用平滑的过渡效果
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                // 更新所有参数
                vignette.color.value = Color.Lerp(startColor, targetColor, smoothT);
                vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, smoothT);
                vignette.smoothness.value = Mathf.Lerp(startSmoothness, targetSmoothness, smoothT);
            }
            else
            {
                isTransitioning = false;
                if (targetColor != defaultColor)
                {
                    // 如果不是过渡到默认状态，开始过渡回默认状态
                    StartTransition(defaultColor, defaultIntensity, defaultSmoothness);
                }
            }
        }
    }

    // 开始状态过渡
    private void StartTransition(Color targetCol, float targetInt, float targetSmooth)
    {
        isTransitioning = true;
        currentEffectTime = 0f;

        // 存储当前状态作为起始值
        startColor = vignette.color.value;
        startIntensity = vignette.intensity.value;
        startSmoothness = vignette.smoothness.value;

        // 设置目标值
        targetColor = targetCol;
        targetIntensity = targetInt;
        targetSmoothness = targetSmooth;
    }

    // 设置默认状态
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

    // 触发扣血效果
    public void TriggerDamageState()
    {
        StartTransition(damageColor, damageIntensity, damageSmoothness);
    }

    // 触发回血效果
    public void TriggerHealState()
    {
        StartTransition(healColor, healIntensity, healSmoothness);
    }
}