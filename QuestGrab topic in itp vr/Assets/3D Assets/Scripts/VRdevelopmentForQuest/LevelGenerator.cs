using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("Ҫ���ɵ�Ԥ�����б�")]
    public List<GameObject> levelPrefabs;

    [Tooltip("���ɷ��� (trueΪX��, falseΪZ��)")]
    public bool generateAlongX = true;

    [Tooltip("�ؿ����֮��Ļ�������")]
    public float levelDistance = 10f;

    [Tooltip("�������ƫ��ǿ��")]
    [Range(0f, 1f)]
    public float randomIntensity = 0.2f;

    [Tooltip("�ؿ�����ƶ��ٶ�")]
    public float levelMoveSpeed = 5f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private const int MAX_OBJECTS = 8;

    void Start()
    {
        GenerateLevelObjects();
    }

    void Update()
    {
        // �ƶ��������ɵ����
        Vector3 moveDirection = generateAlongX ? Vector3.left : Vector3.back;
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                obj.transform.Translate(moveDirection * levelMoveSpeed * Time.deltaTime);
            }
        }

        // �����Ѿ��Ƴ���Ұ�����
        CleanUpOffscreenObjects();
    }

    void GenerateLevelObjects()
    {
        if (levelPrefabs == null || levelPrefabs.Count == 0)
        {
            Debug.LogWarning("û������Ԥ���壡");
            return;
        }

        Vector3 currentPosition = transform.position;

        for (int i = 0; i < MAX_OBJECTS; i++)
        {
            // ���ѡ��һ��Ԥ����
            GameObject prefab = levelPrefabs[Random.Range(0, levelPrefabs.Count)];

            // �����������ƫ��
            float randomOffset = Random.Range(-levelDistance * randomIntensity, levelDistance * randomIntensity);
            float totalDistance = levelDistance + randomOffset;

            // ����λ��
            if (generateAlongX)
            {
                currentPosition.x += totalDistance;
            }
            else
            {
                currentPosition.z += totalDistance;
            }

            // �������
            GameObject spawnedObject = Instantiate(prefab, currentPosition, Quaternion.identity);
            spawnedObjects.Add(spawnedObject);
        }
    }

    void CleanUpOffscreenObjects()
    {
        float threshold = generateAlongX ? -20f : -20f; // ���Ը�����Ҫ����������ֵ

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