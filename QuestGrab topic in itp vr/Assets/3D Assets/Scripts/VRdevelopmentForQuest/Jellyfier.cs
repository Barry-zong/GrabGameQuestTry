using UnityEngine;

public class Jellyfier : MonoBehaviour
{

    [Header("Jelly Physics")]
    public float bounceSpeed = 10f;
    public float stiffness = 0.3f;
    public float pressureRadius = 0.05f;
    public float velocityMultiplier = 30f;

    [Header("Stability Settings")]
    public float maxVelocity = 1f;
    public float returnSpeed = 2f;
    public float stabilityThreshold = 0.01f;

    [Header("Deformation Settings")]
    public float influenceStartHeight = 0.2f;  // 开始影响的高度（从底部算起）
    public float influenceEndHeight = 1f;      // 最大影响的高度
    public AnimationCurve influenceCurve = new AnimationCurve(  // 影响力曲线
        new Keyframe(0, 0),
        new Keyframe(0.5f, 0.3f),
        new Keyframe(1, 1)
    );

    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] initialVertices;
    private Vector3[] currentVertices;
    private Vector3[] vertexVelocities;
    private Vector3 previousPosition;
    private float updateInterval = 0.016f;
    private float timeSinceLastUpdate = 0f;
    private Bounds meshBounds;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        initialVertices = mesh.vertices;
        currentVertices = new Vector3[initialVertices.Length];
        vertexVelocities = new Vector3[initialVertices.Length];

        System.Array.Copy(initialVertices, currentVertices, initialVertices.Length);
        for (int i = 0; i < vertexVelocities.Length; i++)
        {
            vertexVelocities[i] = Vector3.zero;
        }

        previousPosition = transform.position;
        meshBounds = mesh.bounds;
    }

    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateInterval)
        {
            Vector3 velocity = (transform.position - previousPosition) / timeSinceLastUpdate;

            if (velocity.magnitude > maxVelocity)
            {
                velocity = velocity.normalized * maxVelocity;
            }

            if (velocity.magnitude > 0.001f)
            {
                ApplyDeformation(velocity);
            }

            UpdateVertices();
            timeSinceLastUpdate = 0f;
            previousPosition = transform.position;
        }
    }

    private float CalculateVertexInfluence(Vector3 vertex)
    {
        // 将顶点位置标准化到0-1范围
        float normalizedHeight = (vertex.y - meshBounds.min.y) / (meshBounds.max.y - meshBounds.min.y);

        // 计算影响力
        if (normalizedHeight < influenceStartHeight)
            return 0;
        if (normalizedHeight > influenceEndHeight)
            return 1;

        float t = (normalizedHeight - influenceStartHeight) / (influenceEndHeight - influenceStartHeight);
        return influenceCurve.Evaluate(t);
    }

    private void UpdateVertices()
    {
        bool isStable = true;

        for (int i = 0; i < currentVertices.Length; i++)
        {
            Vector3 displacement = currentVertices[i] - initialVertices[i];

            if (displacement.magnitude < stabilityThreshold)
            {
                currentVertices[i] = Vector3.Lerp(currentVertices[i], initialVertices[i], Time.deltaTime * returnSpeed);
                vertexVelocities[i] = Vector3.zero;
                continue;
            }

            float influence = CalculateVertexInfluence(initialVertices[i]);

            // 应用渐变的返回力
            Vector3 returnForce = -displacement * returnSpeed * influence;

            vertexVelocities[i] += returnForce * Time.deltaTime;
            vertexVelocities[i] -= displacement * bounceSpeed * Time.deltaTime * influence;

            vertexVelocities[i] *= 1f - (stiffness * Time.deltaTime);

            if (vertexVelocities[i].magnitude > maxVelocity)
            {
                vertexVelocities[i] = vertexVelocities[i].normalized * maxVelocity;
            }

            currentVertices[i] += vertexVelocities[i] * Time.deltaTime;

            if (vertexVelocities[i].magnitude > stabilityThreshold)
            {
                isStable = false;
            }
        }

        if (isStable)
        {
            for (int i = 0; i < currentVertices.Length; i++)
            {
                currentVertices[i] = Vector3.Lerp(currentVertices[i], initialVertices[i], Time.deltaTime * returnSpeed);
            }
        }

        mesh.vertices = currentVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void ApplyDeformation(Vector3 velocity)
    {
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        for (int i = 0; i < currentVertices.Length; i++)
        {
            float influence = CalculateVertexInfluence(initialVertices[i]);

            Vector3 addedVelocity = localVelocity * velocityMultiplier * influence;
            if (addedVelocity.magnitude > maxVelocity)
            {
                addedVelocity = addedVelocity.normalized * maxVelocity;
            }

            vertexVelocities[i] += addedVelocity;
        }
    }

    public void AddDeformationAtPoint(Vector3 worldPoint, float force)
    {
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

        for (int i = 0; i < currentVertices.Length; i++)
        {
            float influence = CalculateVertexInfluence(initialVertices[i]);

            Vector3 offset = currentVertices[i] - localPoint;
            float distance = offset.magnitude;

            if (distance < pressureRadius)
            {
                float pointInfluence = 1f - (distance / pressureRadius);
                Vector3 addedVelocity = offset.normalized * force * pointInfluence * influence;

                if (addedVelocity.magnitude > maxVelocity)
                {
                    addedVelocity = addedVelocity.normalized * maxVelocity;
                }

                vertexVelocities[i] += addedVelocity;
            }
        }
    }
}