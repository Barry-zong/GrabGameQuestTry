using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("要生成的预制体列表")]
    public List<GameObject> levelPrefabs;

    [Tooltip("要生成的物件数量")]
    [Range(1, 20)]
    public int maxObjects = 10;

    [Tooltip("关卡物件之间的基础距离")]
    public float levelDistance = 10f;

    [Tooltip("距离随机偏移强度")]
    [Range(0f, 1f)]
    public float randomIntensity = 0.2f;

    [Tooltip("关卡物件移动速度")]
    public float levelMoveSpeed = 5f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private const float OBJECT_SCALE = 0.40779f;
    private int lastPrefabIndex = -1;  // 记录上一个生成的预制体索引

    void Start()
    {
        GenerateLevelObjects();
    }

    void Update()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Vector3 moveDirection = transform.TransformDirection(Vector3.right);
                obj.transform.Translate(moveDirection * levelMoveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    // 获取不重复的随机预制体
    GameObject GetRandomPrefabNonRepeat()
    {
        if (levelPrefabs.Count <= 1)
        {
            return levelPrefabs[0];  // 如果只有一个预制体，直接返回
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, levelPrefabs.Count);
        } while (newIndex == lastPrefabIndex);  // 如果随机到了和上次相同的索引，继续随机

        lastPrefabIndex = newIndex;  // 更新上一个预制体的索引
        return levelPrefabs[newIndex];
    }

    void GenerateLevelObjects()
    {
        if (levelPrefabs == null || levelPrefabs.Count == 0)
        {
            return;
        }

        Vector3 currentLocalPosition = Vector3.zero;

        for (int i = 0; i < maxObjects; i++)
        {
            GameObject prefab = GetRandomPrefabNonRepeat();

            float randomOffset = Random.Range(-levelDistance * randomIntensity, levelDistance * randomIntensity);
            float totalDistance = levelDistance + randomOffset;

            currentLocalPosition.x += totalDistance;

            Vector3 worldPosition = transform.TransformPoint(currentLocalPosition);

            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            GameObject spawnedObject = Instantiate(prefab, worldPosition, rotation);

            spawnedObject.transform.SetParent(transform);
            spawnedObject.transform.localScale = new Vector3(OBJECT_SCALE, OBJECT_SCALE, OBJECT_SCALE);

            spawnedObjects.Add(spawnedObject);
        }
    }
}