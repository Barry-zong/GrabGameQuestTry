using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneMove : MonoBehaviour
{
    public bool ZtrueXfales = true;
    public float movingSpeed;
    private float initialSpeed = 0f;
    public float maxSpeed = 0.1f; 
    public Slider slider;
    private Vector3 sceneStartPosition;
    float defultSpeed = 0;
    public bool isStart = false;
    public HandGestureManager leftHandCalculator;
    public float counter = 0f;
    public float increaseSpeed = 0.05f;  // 每秒增加的速度
    private bool isOKGesture = false;

    public EndUIconScene1 endScript;
    // Start is called before the first frame update
    void Start()
    {
        sceneStartPosition = transform.position;
        defultSpeed = movingSpeed;
        if (slider != null)
        { slider.onValueChanged.AddListener(SliderValueChanged); }
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
            isOKGesture = (thumbBend > 0.6f && indexBend > 0.6f) &&
                          (middleBend > 0.6f && ringBend > 0.6f && pinkyBend > 0.6f);
            // 根据手势状态更新计数器
            if (isOKGesture )
            {
                counter += Time.deltaTime * increaseSpeed;
                Debug.Log("uppppppp");
                if (counter > maxSpeed)
                {
                    counter = maxSpeed;
                }
            }
            else
            {
                counter = 0f;
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        okSpeedUp();
        if (isStart)
        {
            if (ZtrueXfales)
            { transform.position = new Vector3(sceneStartPosition.x, sceneStartPosition.y, sceneStartPosition.z + initialSpeed); }
            else
            { transform.position = new Vector3(sceneStartPosition.x + initialSpeed, sceneStartPosition.y, sceneStartPosition.z); }
        }
    }
    private void FixedUpdate()
    {
        if (isStart )
        {
            initialSpeed = initialSpeed + movingSpeed + counter;
            
        }
    }
    void SliderValueChanged(float newValue)
    {
        movingSpeed = defultSpeed;
        movingSpeed += newValue / 50;
    }
    public void sceneStart()
    {
        isStart = true;
    }
    public void RestartScene()
    {
        if(endScript.levelEnding)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}