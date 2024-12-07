using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExpoldedEffect : MonoBehaviour
{
    [Header("触发设置")]
    public bool triggerExplosion = false;
    public bool processChildrenSequentially = false;
    public float delayBetweenChildren = 0.5f;

    [Header("粒子运动设置")]
    public float particleScale = 0.1f;
    public Material particleMaterial;
    public int subdivisions = 10;
    public float fadeOutTime = 3f;

    [Header("曲线运动设置")]
    public float upwardSpeed = 2f;
    public float waveMagnitude = 0.5f;
    public float waveFrequency = 2f;
    public float noiseScale = 1f;
    public float spiralForce = 0.5f;
    public float randomness = 0.5f;

    [Header("拖尾效果")]
    public bool enableTrail = true;
    public float trailTime = 0.3f;
    public float trailWidth = 0.05f;
    public Material trailMaterial;
    public Gradient trailColor;

    private List<ParticleInfo> particles;
    private bool hasExploded = false;
    private Queue<MeshFilter> pendingMeshes;
    private float nextExplosionTime;

    private class ParticleInfo
    {
        public GameObject particle;
        public float startTime;
        public Material originalMaterial;
        public Color originalColor;
        public TrailRenderer trail;
        public Vector3 baseOffset;
        public Vector3 noiseOffset;
        public float rotationSpeed;
        public float pathOffset;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;  // 初始速度
        public Vector3 currentVelocity;  // 当前速度
    }

    void Start()
    {
        particles = new List<ParticleInfo>();
        pendingMeshes = new Queue<MeshFilter>();
        InitializeTrailColor();
    }

    void Update()
    {
        if (triggerExplosion && !hasExploded)
        {
            CollectChildMeshes();
            triggerExplosion = false;
            hasExploded = true;
            nextExplosionTime = Time.time;
        }

        if (pendingMeshes.Count > 0 && Time.time >= nextExplosionTime)
        {
            ProcessNextMesh();
            if (processChildrenSequentially)
            {
                nextExplosionTime = Time.time + delayBetweenChildren;
            }
        }

        UpdateParticles();
    }

    private void InitializeTrailColor()
    {
        if (trailColor == null || trailColor.colorKeys.Length == 0)
        {
            trailColor = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(Color.white, 0.0f);
            colorKeys[1] = new GradientColorKey(new Color(1, 1, 1, 0), 1.0f);
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f);
            trailColor.SetKeys(colorKeys, alphaKeys);
        }
    }

    private void UpdateParticles()
    {
        if (particles == null) return;

        for (int i = particles.Count - 1; i >= 0; i--)
        {
            ParticleInfo info = particles[i];
            if (info.particle == null)
            {
                particles.RemoveAt(i);
                continue;
            }

            float elapsed = Time.time - info.startTime;
            float t = elapsed / fadeOutTime;

            if (t >= 1f)
            {
                if (info.trail != null)
                {
                    info.trail.time = 0;
                }
                Destroy(info.particle);
                particles.RemoveAt(i);
                continue;
            }

            // 更新速度（添加重力影响）
            info.currentVelocity += Physics.gravity * Time.deltaTime * 0.3f; // 减小重力影响

            // 更新位置
            info.particle.transform.position = CalculateParticlePosition(info, Time.deltaTime);

            // 更新旋转
            info.particle.transform.Rotate(Vector3.up * info.rotationSpeed * Time.deltaTime);

            // 更新透明度
            if (info.originalMaterial != null)
            {
                Color newColor = info.originalColor;
                newColor.a = Mathf.Lerp(1f, 0f, Mathf.SmoothStep(0f, 1f, t));
                info.particle.GetComponent<MeshRenderer>().material.color = newColor;
            }

            // 更新拖尾
            if (info.trail != null)
            {
                info.trail.time = Mathf.Lerp(trailTime, 0f, t);
                info.trail.startWidth = Mathf.Lerp(trailWidth, 0f, t);
            }
        }
    }

    private Vector3 CalculateParticlePosition(ParticleInfo info, float deltaTime)
    {
        float timeSinceStart = Time.time - info.startTime;

        // 计算基础运动
        Vector3 position = info.particle.transform.position;

        // 应用当前速度
        position += info.currentVelocity * deltaTime;

        // 添加螺旋运动
        float spiralAngle = timeSinceStart * waveFrequency;
        Vector3 spiralMotion = new Vector3(
            Mathf.Cos(spiralAngle) * spiralForce,
            0,
            Mathf.Sin(spiralAngle) * spiralForce
        );

        // 添加随机偏移
        Vector3 randomOffset = new Vector3(
            Mathf.PerlinNoise(timeSinceStart + info.noiseOffset.x, 0) - 0.5f,
            Mathf.PerlinNoise(0, timeSinceStart + info.noiseOffset.y) - 0.5f,
            Mathf.PerlinNoise(timeSinceStart + info.noiseOffset.z, 0) - 0.5f
        ) * randomness;

        return position + (spiralMotion + randomOffset) * deltaTime;
    }

    private void CollectChildMeshes()
    {
        pendingMeshes.Clear();
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.transform != transform)
            {
                pendingMeshes.Enqueue(meshFilter);
            }
        }

        if (!processChildrenSequentially)
        {
            while (pendingMeshes.Count > 0)
            {
                ProcessNextMesh();
            }
        }
    }

    private void ProcessNextMesh()
    {
        if (pendingMeshes.Count == 0) return;

        MeshFilter meshFilter = pendingMeshes.Dequeue();
        if (meshFilter != null && meshFilter.mesh != null)
        {
            ExplodeMesh(meshFilter);
        }
    }

    private void ExplodeMesh(MeshFilter meshFilter)
    {
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        foreach (Vector3 vertex in vertices)
        {
            if (Random.value > 1f / subdivisions) continue;

            Vector3 worldVertex = meshFilter.transform.TransformPoint(vertex);
            GameObject particle = CreateParticle(worldVertex);

            // 计算爆炸方向（从中心向外）
            Vector3 explosionDir = (worldVertex - meshFilter.transform.position).normalized;

            // 创建初始速度（上升 + 向外扩散）
            Vector3 initialVelocity = (Vector3.up * upwardSpeed + explosionDir * Random.Range(1f, 2f))
                                    * Random.Range(0.8f, 1.2f);

            ParticleInfo info = new ParticleInfo
            {
                particle = particle,
                startTime = Time.time,
                originalMaterial = particle.GetComponent<MeshRenderer>().material,
                originalColor = particle.GetComponent<MeshRenderer>().material.color,
                baseOffset = Random.insideUnitSphere,
                noiseOffset = Random.insideUnitSphere * 1000f,
                rotationSpeed = Random.Range(-180f, 180f),
                pathOffset = Random.Range(0f, Mathf.PI * 2f),
                initialPosition = worldVertex,
                initialVelocity = initialVelocity,
                currentVelocity = initialVelocity
            };

            if (enableTrail)
            {
                info.trail = AddTrailEffect(particle);
            }

            particles.Add(info);
        }

        meshFilter.GetComponent<MeshRenderer>().enabled = false;
    }

    private GameObject CreateParticle(Vector3 position)
    {
        GameObject particle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        particle.transform.localScale = Vector3.one * particleScale;
        particle.transform.position = position;

        Material mat;
        if (particleMaterial != null)
        {
            mat = new Material(particleMaterial);
        }
        else
        {
            mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        mat.SetFloat("_Surface", 1);
        mat.SetFloat("_Blend", 0);
        mat.SetFloat("_AlphaClip", 0);

        particle.GetComponent<MeshRenderer>().material = mat;

        return particle;
    }

    private TrailRenderer AddTrailEffect(GameObject particle)
    {
        TrailRenderer trail = particle.AddComponent<TrailRenderer>();
        trail.time = trailTime;
        trail.startWidth = trailWidth;
        trail.endWidth = 0f;
        trail.colorGradient = trailColor;

        Material trailMat;
        if (trailMaterial != null)
        {
            trailMat = new Material(trailMaterial);
        }
        else
        {
            trailMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        }

        trail.material = trailMat;
        trail.sortingOrder = 1;

        return trail;
    }

    public void Reset()
    {
        hasExploded = false;
        pendingMeshes.Clear();

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = true;
        }

        foreach (var info in particles)
        {
            if (info.particle != null)
            {
                Destroy(info.particle);
            }
        }
        particles.Clear();
    }

    public void TriggerEffect()
    {
        triggerExplosion = true;
    }
}