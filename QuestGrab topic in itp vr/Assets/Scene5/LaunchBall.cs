using System.Collections;
using System.Collections.Generic;
//using TreeEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LaunchBall : MonoBehaviour
{
    HandControl handcontroL;
    public GameObject hand;
    public GameObject timeRecoder;
    [Header("SetBall")]
    public GameObject Ball;
    public Transform BallResetPoint;//球的生成点
    [Header("SetForce")]
    public float ForceRate;//力的增加速度
    [Header("SetTarget")]
    public Transform Target;
    public float MaxDistance;//框的最远距离
    public float MinDistance;//框的最近距离
    public float LimitRange = 5;
    [Header("SetUI")]
    public TMP_Text ScoreText;
    public TMP_Text ScoreText2;
    public TMP_Text QiuForceText;
    public Slider slider;
    private bool uiShow= false;

    public float Force = 0;
    Rigidbody rb;
    bool isHolding=false;
    bool canHold=false;
    GameObject curBall;
    float score=0;
    private float handValue ;
    private bool launched = true;
    private float sliderValue = 0;
    private float resetTime;
    private float B = 0;
    StaticSceneTimeRestrict SSTR;
    public GameObject TimeAddRemind;
    public AudioSource releaseBallAudio;
    public AudioSource BallInHoleAudio;
    public AudioSource BallInRefreshAudio;
    void Start()
    {   SSTR = timeRecoder.GetComponent<StaticSceneTimeRestrict>();
        slider.value = sliderValue ;
        ResetBall();
        handcontroL = hand.GetComponent<HandControl>();
    }

    void Update()
    {
        slider.value = sliderValue ;
        float DemoForce = Force/10 ;
        sliderValue = Mathf.InverseLerp(0, 75, DemoForce);
        int intForce = Mathf.RoundToInt(DemoForce);
        QiuForceText.text = intForce.ToString();
        handValue = handcontroL.FingerDegreeAll;
        resetTime = 0.5f + sliderValue*4 ;
        if(handValue>40)
        {
            canHold = true;
        }

        if (canHold)
        {
            if (handValue>75) //替换为紧握阈值
            {
                isHolding = true;
            }
            if (handValue<30 && isHolding) //替换为松开阈值
            {
                isHolding = false;
                Launch();
            }
        }
        if (isHolding)
        {
            if(launched)
           { Force += Time.deltaTime * ForceRate;}
        }

        if(uiShow)
        {
            float a = SSTR.endingTime ;
            Invoke("TimeAddShouwOff",1f);
        }
    }
    private void TimeAddShouwOff()
    {
        TimeAddRemind.SetActive(false);
        uiShow = false ;
    }
    public void Launch()
    {
        if(launched)
        {if (Force > 0)
        {
            canHold = false;
        }
        Vector3 launchDeraction=BallResetPoint.up;
        rb.AddForce(launchDeraction * Force);
        launched = false ;
        releaseBallAudio.Play();
        Invoke(nameof(ResetBall), resetTime);}
     
    }
    

    public void ResetBall()
    {
        if(Force> 0)
        {
            Destroy(curBall);
            curBall = Instantiate(Ball, BallResetPoint.position, Quaternion.identity);
            BallInRefreshAudio.Play();
            rb = curBall.GetComponent<Rigidbody>();
            curBall.GetComponent<Ball>().launcher = this;
             launched = true;
        }
        Force = 0;
        canHold = true;
    }

    public void ReaetTarget()
    { 
        ResetBall();
        Debug.Log("enterArea");
        BallInHoleAudio.Play();
        score += 1;
        SSTR.endingTime+=15;
        B = SSTR.endingTime;
        TimeAddRemind.SetActive(true);
        uiShow = true ;
        ScoreText.text = score.ToString();
        ScoreText2.text = score.ToString();
        float x = Random.Range(MinDistance, MaxDistance);
        float exX = Target.position.z ;
        if(Mathf.Abs(x-exX)<LimitRange)
        {
             x = Random.Range(MinDistance, MaxDistance);
             Target.position = new Vector3(Target.position.x, Target.position.y,x);
        }
        else
        {Target.position = new Vector3(Target.position.x, Target.position.y,x);}
    }
}
