using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("要生成的预制体列表")]
    public List<GameObject> levelPrefabs;

    [Tooltip("生成方向 (true为X轴, false为Z轴)")]
    public bool generateAlongX = true;

    [Tooltip("关卡物件之间的基础距离")]
    public float levelDistance = 10f;

    [Tooltip("距离随机偏移强度")]
    [Range(0f, 1f)]
    public float randomIntensity = 0.2f;

    [Tooltip("关卡物件移动速度")]
    public float levelMoveSpeed = 5f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private const int MAX_OBJECTS = 8;

    void Start()
    {
        GenerateLevelObjects();
    }

    void Update()
    {
        // 移动所有生成的物件
        Vector3 moveDirection = generateAlongX ? Vector3.left : Vector3.back;
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                obj.transform.Translate(moveDirection * levelMoveSpeed * Time.deltaTime);
            }
        }

        // 清理已经移出视野的物件
        CleanUpOffscreenObjects();
    }

    void GenerateLevelObjects()
    {
        if (levelPrefabs == null || levelPrefabs.Count == 0)
        {
            Debug.LogWarning("没有配置预制体！");
            return;
        }

        Vector3 currentPosition = transform.position;

        for (int i = 0; i < MAX_OBJECTS; i++)
        {
            // 随机选择一个预制体
            GameObject prefab = levelPrefabs[Random.Range(0, levelPrefabs.Count)];

            // 计算随机距离偏移
            float randomOffset = Random.Range(-levelDistance * randomIntensity, levelDistance * randomIntensity);
            float totalDistance = levelDistance + randomOffset;

            // 更新位置
            if (generateAlongX)
            {
                currentPosition.x += totalDistance;
            }
            else
            {
                currentPosition.z += totalDistance;
            }

            // 生成物件
            GameObject spawnedObject = Instantiate(prefab, currentPosition, Quaternion.identity);
            spawnedObjects.Add(spawnedObject);
        }
    }

    void CleanUpOffscreenObjects()
    {
        float threshold = generateAlongX ? -20f : -20f; // 可以根据需要调整清理阈值

        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] != null)
            {
                float position = generateAlongX ?
                    spawnedObjects[i].transform.position.x :
                    spawnedObjects[i].transform.position.z;

                if (position < threshold)
                {
                    Destroy(spawnedObjects[i]);
                    spawnedObjects.RemoveAt(i);
                }
            }
        }
    }
}