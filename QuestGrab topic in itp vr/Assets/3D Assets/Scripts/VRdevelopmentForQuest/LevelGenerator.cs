using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("Ҫ���ɵ�Ԥ�����б�")]
    public List<GameObject> levelPrefabs;

    [Tooltip("���һ�������Ԥ����")]
    public GameObject lastObj;

    [Tooltip("��ʼ���ɵ��������")]
    [Range(1, 20)]
    public int maxObjects = 10;

    [Tooltip("�ܹ���Ҫ���ɵ�Ŀ������")]
    public int targetTotalObjects = 20;

    [Tooltip("�ؿ����֮��Ļ�������")]
    public float levelDistance = 10f;

    [Tooltip("�������ƫ��ǿ��")]
    [Range(0f, 1f)]
    public float randomIntensity = 0.2f;

    [Tooltip("�ؿ�����ƶ��ٶ�")]
    public float levelMoveSpeed = 5f;

    [Tooltip("��ʱ�Ƿ����")]
    public bool timeOver = false;

    [Tooltip("��Ϸ�Ƿ���ͣ")]
    public bool isPaused = false;

    [Tooltip("��ʾʱ���UI�ı�")]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text calculateText;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<Rigidbody> spawnedRigidbodies = new List<Rigidbody>(); // �������洢���������Rigidbody���
    private const float OBJECT_SCALE = 0.40779f;
    private int lastPrefabIndex = -1;
    private int totalSpawnedCount = 0;
    private GameObject lastSpawnedObject;
    private bool hasSpawnedLastObj = false;
    private bool isOKspeed = false;
    private float timer = 0f;
    private bool isTimerRunning = false;
    public bool startGenGame = false;
    private bool openGameOnce = false;
    public StartScene1AudioPlay startScene1AudioPlay;
    public GameObject beforeStartUI;
    public GameObject afterStartUI;
    public int currentScore = 0;
    private float calcuteDexterity = 0f;
    public EndUIconScene1 endUIconScene1;
    public float counter = 0f;
    public HandGestureManager leftHandCalculator;
    private Vector3[] lastPositions; // �������洢��ͣʱ��λ��
    private bool isOKGesture = false;
    public float maxSpeed = 0.1f;
    public float increaseSpeed = 0.05f;  // ÿ�����ӵ��ٶ�
    private bool startSpeed = true;
    private void Start()
    {
        openGameOnce = true;
        lastPositions = new Vector3[0];
    }
    void okSpeedUp()
    {

        if (leftHandCalculator != null)
        {
           
           // Debug.Log("innnn");
            float thumbBend = leftHandCalculator.leftFingerBends[0];
            float indexBend = leftHandCalculator.leftFingerBends[1];
            float middleBend = leftHandCalculator.leftFingerBends[2];
            float ringBend = leftHandCalculator.leftFingerBends[3];
            float pinkyBend = leftHandCalculator.leftFingerBends[4];
            // OK���Ƶ�������
            // Ĵָ��ʳָ�����ȴ���0.7����ʾ��ϣ�
            // ������ָ������С��0.3����ʾ��չ��
            //   Debug.Log("Fingers: " + thumbBend + ", " + indexBend + ", " + middleBend + ", " + ringBend + ", " + pinkyBend);
            isOKGesture = (thumbBend > 0.7f && indexBend > 0.6f) &&
                          (middleBend > 0.6f && ringBend > 0.1f && pinkyBend >= 0f);
            // ��������״̬���¼�����
            if (isOKGesture)
            {
                if(startSpeed)
                {
                    counter = 1f;
                    startSpeed = false;
                }
                
                counter += Time.deltaTime * increaseSpeed;
               // Debug.Log("uppppppp");
               
                
                if (counter > maxSpeed)
                {
                    counter = maxSpeed;
                   
                }
            }
            else
            {
                counter = 0f;
                startSpeed = true;
            }

        }
    }
    void Update()
    {
       
        if (isOKspeed)
        {
            okSpeedUp();
        }
       
        if (startGenGame && openGameOnce)
        {
            isOKspeed = true;
            GenerateInitialObjects();
            startScene1AudioPlay.mainBackgroundMusicPlay = true;
            openGameOnce = false;
        }
        float newSpeed = levelMoveSpeed + counter;
        // ֻ�ڷ���ͣ״̬��ִ����Ϸ�߼�
        if (startGenGame && !isPaused)
        {
            // �ƶ��������
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i] != null)
                {
                    Vector3 moveDirection = transform.TransformDirection(Vector3.right);
                    spawnedObjects[i].transform.Translate(moveDirection * newSpeed * Time.deltaTime, Space.World);
                }
            }

            // ����Ƿ���Ҫ���������
            CheckAndGenerateNewObject();

            // ���¼�ʱ��
            if (isTimerRunning && !timeOver)
            {
                timer += Time.deltaTime;
            }
        }

        // ��ЩUI����Ӧ������ͣ���֮�⣬ȷ��UIʼ�ո���
        if (isPaused || timeOver)
        {
            calcuteDexterity = CalculateDexterity(timer, currentScore);
        }

        UpdateTimer();
    }
    public void DieNoHeartRemain()
    {
        TogglePause();
        endUIconScene1.levellosing = true;
    }

    // ��������ͣ����
    public void TogglePause()
    {
        isPaused = !isPaused;

        // ��ͣʱ�������������λ��
        if (isPaused)
        {
            lastPositions = new Vector3[spawnedObjects.Count];
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i] != null)
                {
                    lastPositions[i] = spawnedObjects[i].transform.position;

                    // ���������Rigidbody�������ͣ������ģ��
                    Rigidbody rb = spawnedObjects[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.velocity = Vector3.zero;
                    }
                }
            }
        }
        else
        {
            // �ָ�ʱȷ������λ�ò���
            for (int i = 0; i < spawnedObjects.Count && i < lastPositions.Length; i++)
            {
                if (spawnedObjects[i] != null)
                {
                    spawnedObjects[i].transform.position = lastPositions[i];

                    // �ָ�Rigidbody������ģ��
                    Rigidbody rb = spawnedObjects[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
        }

        // ��ͣ/�ָ���ʱ��
        isTimerRunning = !isPaused && !timeOver;
    }

    public float CalculateDexterity(float timeInSeconds, float score)
    {
        float baseTimeInSeconds = targetTotalObjects;
        float baseScore = targetTotalObjects;
        float timeScore = (baseTimeInSeconds - timeInSeconds) / baseTimeInSeconds * 50 + 50;
        float scoreValue = (score / baseScore) * 50;
        float dexterity = (timeScore + scoreValue) / 2;
        return Mathf.Clamp(dexterity, 0f, 100f);
    }

    void UpdateTimer()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            int milliseconds = Mathf.FloorToInt((timer * 100f) % 100f);
            timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        }

        scoreText.text = currentScore.ToString();
        calculateText.text = $"{calcuteDexterity}%";

        if (timeOver)
        {
            isTimerRunning = false;
        }
    }

    GameObject GetRandomPrefabNonRepeat()
    {
        if (levelPrefabs.Count <= 1)
        {
            return levelPrefabs[0];
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, levelPrefabs.Count);
        } while (newIndex == lastPrefabIndex);

        lastPrefabIndex = newIndex;
        return levelPrefabs[newIndex];
    }

    void GenerateInitialObjects()
    {
        if (levelPrefabs == null || levelPrefabs.Count == 0)
        {
            return;
        }

        Vector3 currentLocalPosition = Vector3.zero;

        for (int i = 0; i < maxObjects && totalSpawnedCount < targetTotalObjects; i++)
        {
            GameObject prefab = GetRandomPrefabNonRepeat();

            float randomOffset = Random.Range(-levelDistance * randomIntensity, levelDistance * randomIntensity);
            float totalDistance = levelDistance + randomOffset;

            currentLocalPosition.x += totalDistance;

            Vector3 worldPosition = transform.TransformPoint(currentLocalPosition);

            GameObject spawnedObject = Instantiate(prefab, worldPosition, Quaternion.Euler(0, 0, 0));
            spawnedObject.transform.SetParent(transform);
            spawnedObject.transform.localScale = new Vector3(OBJECT_SCALE, OBJECT_SCALE, OBJECT_SCALE);

            spawnedObjects.Add(spawnedObject);
            lastSpawnedObject = spawnedObject;
            totalSpawnedCount++;
        }

        timer = 0f;
        isTimerRunning = true;
        timeOver = false;
    }

    void CheckAndGenerateNewObject()
    {
        if (hasSpawnedLastObj || lastSpawnedObject == null)
        {
            return;
        }

        Vector3 lastObjectLocalPos = transform.InverseTransformPoint(lastSpawnedObject.transform.position);

        if (lastObjectLocalPos.x >= levelDistance)
        {
            if (totalSpawnedCount >= targetTotalObjects - 1 && lastObj != null)
            {
                Vector3 spawnPosition = transform.position;
                GameObject spawnedObject = Instantiate(lastObj, spawnPosition, Quaternion.Euler(0, 0, 0));
                spawnedObject.transform.SetParent(transform);
                spawnedObject.transform.localScale = new Vector3(OBJECT_SCALE, OBJECT_SCALE, OBJECT_SCALE);

                spawnedObjects.Add(spawnedObject);
                lastSpawnedObject = spawnedObject;
                totalSpawnedCount++;
                hasSpawnedLastObj = true;
            }
            else if (totalSpawnedCount < targetTotalObjects)
            {
                GameObject prefab = GetRandomPrefabNonRepeat();
                Vector3 spawnPosition = transform.position;

                GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.Euler(0, 0, 0));
                spawnedObject.transform.SetParent(transform);
                spawnedObject.transform.localScale = new Vector3(OBJECT_SCALE, OBJECT_SCALE, OBJECT_SCALE);

                spawnedObjects.Add(spawnedObject);
                lastSpawnedObject = spawnedObject;
                totalSpawnedCount++;
            }
        }
    }

    public void ButtonClickInteraction()
    {
        if (!startGenGame)
        {
            startGenGame = true;
            beforeStartUI.SetActive(false);
            afterStartUI.SetActive(true);
        }
        /*
        if (startGenGame&&openGameOnce&&!timeOver)
        {
            TogglePause();
        }
        */
        if (endUIconScene1.levellosing || endUIconScene1.levelEnding)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}