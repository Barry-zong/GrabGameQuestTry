using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("要生成的预制体列表")]
    public List<GameObject> levelPrefabs;

    [Tooltip("最后一个物件的预制体")]
    public GameObject lastObj;

    [Tooltip("初始生成的物件数量")]
    [Range(1, 20)]
    public int maxObjects = 10;

    [Tooltip("总共需要生成的目标数量")]
    public int targetTotalObjects = 20;

    [Tooltip("关卡物件之间的基础距离")]
    public float levelDistance = 10f;

    [Tooltip("距离随机偏移强度")]
    [Range(0f, 1f)]
    public float randomIntensity = 0.2f;

    [Tooltip("关卡物件移动速度")]
    public float levelMoveSpeed = 5f;

    [Tooltip("计时是否结束")]
    public bool timeOver = false;

    [Tooltip("游戏是否暂停")]
    public bool isPaused = false;

    [Tooltip("显示时间的UI文本")]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text calculateText;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<Rigidbody> spawnedRigidbodies = new List<Rigidbody>(); // 新增：存储所有物体的Rigidbody组件
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
    private Vector3[] lastPositions; // 新增：存储暂停时的位置
    private bool isOKGesture = false;
    public float maxSpeed = 0.1f;
    public float increaseSpeed = 0.05f;  // 每秒增加的速度
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
            // OK手势的条件：
            // 拇指和食指弯曲度大于0.7（表示捏合）
            // 其他手指弯曲度小于0.3（表示伸展）
            //   Debug.Log("Fingers: " + thumbBend + ", " + indexBend + ", " + middleBend + ", " + ringBend + ", " + pinkyBend);
            isOKGesture = (thumbBend > 0.7f && indexBend > 0.6f) &&
                          (middleBend > 0.6f && ringBend > 0.1f && pinkyBend >= 0f);
            // 根据手势状态更新计数器
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
        // 只在非暂停状态下执行游戏逻辑
        if (startGenGame && !isPaused)
        {
            // 移动所有物件
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i] != null)
                {
                    Vector3 moveDirection = transform.TransformDirection(Vector3.right);
                    spawnedObjects[i].transform.Translate(moveDirection * newSpeed * Time.deltaTime, Space.World);
                }
            }

            // 检查是否需要生成新物件
            CheckAndGenerateNewObject();

            // 更新计时器
            if (isTimerRunning && !timeOver)
            {
                timer += Time.deltaTime;
            }
        }

        // 这些UI更新应该在暂停检查之外，确保UI始终更新
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

    // 新增的暂停功能
    public void TogglePause()
    {
        isPaused = !isPaused;

        // 暂停时保存所有物体的位置
        if (isPaused)
        {
            lastPositions = new Vector3[spawnedObjects.Count];
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i] != null)
                {
                    lastPositions[i] = spawnedObjects[i].transform.position;

                    // 如果物体有Rigidbody组件，暂停其物理模拟
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
            // 恢复时确保物体位置不变
            for (int i = 0; i < spawnedObjects.Count && i < lastPositions.Length; i++)
            {
                if (spawnedObjects[i] != null)
                {
                    spawnedObjects[i].transform.position = lastPositions[i];

                    // 恢复Rigidbody的物理模拟
                    Rigidbody rb = spawnedObjects[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
        }

        // 暂停/恢复计时器
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