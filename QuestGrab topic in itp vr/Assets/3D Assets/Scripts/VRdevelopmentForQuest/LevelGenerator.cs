using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("Ҫ���ɵ�Ԥ�����б�")]
    public List<GameObject> levelPrefabs;

    [Tooltip("Ҫ���ɵ��������")]
    [Range(1, 20)]
    public int maxObjects = 10;

    [Tooltip("�ؿ����֮��Ļ�������")]
    public float levelDistance = 10f;

    [Tooltip("�������ƫ��ǿ��")]
    [Range(0f, 1f)]
    public float randomIntensity = 0.2f;

    [Tooltip("�ؿ�����ƶ��ٶ�")]
    public float levelMoveSpeed = 5f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private const float OBJECT_SCALE = 0.40779f;
    private int lastPrefabIndex = -1;  // ��¼��һ�����ɵ�Ԥ��������

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

    // ��ȡ���ظ������Ԥ����
    GameObject GetRandomPrefabNonRepeat()
    {
        if (levelPrefabs.Count <= 1)
        {
            return levelPrefabs[0];  // ���ֻ��һ��Ԥ���壬ֱ�ӷ���
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, levelPrefabs.Count);
        } while (newIndex == lastPrefabIndex);  // ���������˺��ϴ���ͬ���������������

        lastPrefabIndex = newIndex;  // ������һ��Ԥ���������
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