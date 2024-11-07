using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level3PowerConsist : MonoBehaviour
{   HandControl handcontroL;
    public GameObject hand;
    // locate three scale objects
    public GameObject scaleOne;
    public GameObject scaleTwo;
    public GameObject scaleThree;
    public GameObject pressButton;
    private float handValue ;
    private float objectScaleBasicValue = 1 ;
    private float scaleRecord = 1 ;
    private float scaleRecord2 = 1 ;
    private float objectScaleChangeValue = 0.1f;
    public float pressDepth = -0.4f ;
    // finger state detect varient
    public float fistDetectValue = 70 ;
    public float fistReleaseValue = 20 ;
    // three scale restrick value
    private float valueCheck = 0 ;
    public float limitLevelOne = 35 ;
    private float limitLevelTwo ;
    private float limitLevelThree ;
    //estimate of hand and object scale
    private bool fingerFist = false ;
    private bool fingerRelease = true ;
    private float leftTime;
    public float TotalTime=60f;
    public TextMeshProUGUI TimeRecoder;  
    public TextMeshProUGUI ScoreRecord;
    public TextMeshProUGUI ScoreRecord2;
    private bool isended=false;
    public GameObject needCloseUI;
    public GameObject needOpenUI;
    private bool opengamelevelt = true ;
    private bool gameStart = false;
    public AudioSource grabAudio ;
    



    // Start is called before the first frame update
    void Start()
    {
        leftTime=TotalTime;
        handcontroL = hand.GetComponent<HandControl>();
        limitLevelTwo = 2 * limitLevelOne;
        limitLevelThree = limitLevelTwo + limitLevelOne;
        
    }

    // Update is called once per frame
    void Update()
    {
      if(!opengamelevelt)
         {TimeRecoder.text=leftTime+"s";
           
         }
         ScoreRecord.text= valueCheck + "00" ;
         ScoreRecord2.text= valueCheck + "00" ;
        //EndingGameUIcontrol.isEnd=false;
        if(!isended)
        {
             if(leftTime<=0)
          {
            isended = true;
           
            needOpenUI.SetActive(true);
            Debug.Log("time run out");
          }
        }
        //Debug.Log(handcontroL.FingerDegreeAll);
        if(opengamelevelt)
        {if(handcontroL.FingerDegreeAll>50)
        { opengamelevelt=false;
           needCloseUI.SetActive(false);
           gameStart = true ;
           StartCoroutine(Countdown());
        }}
         
        //Debug.Log(handValue);
        handValue = handcontroL.FingerDegreeAll;
        
        if(fingerRelease)
        {
         if(handValue>fistDetectValue)
         {
            fingerFist = true;
            fingerRelease = false;
         }
        }
 if(!isended)
        {if(fingerFist)
        {
            objectScaleBasicValue += objectScaleChangeValue;
            valueCheck += 1 ;
            fingerFist = false;
            grabAudio.Play();
            pressButton.transform.position = new Vector3(pressButton.transform.position.x,pressDepth,pressButton.transform.position.z);
            Debug.Log(objectScaleBasicValue);
            Debug.Log(valueCheck);
        }

        if(handValue<fistReleaseValue)
        {
            fingerRelease = true ;
            pressButton.transform.position = new Vector3(pressButton.transform.position.x,0f,pressButton.transform.position.z);
        }


        if(valueCheck < limitLevelOne )
        {
            Vector3 newScale = new Vector3(objectScaleBasicValue,objectScaleBasicValue,objectScaleBasicValue) ;
            scaleOne.transform.localScale = newScale ;
        }
        else
        {
            if(valueCheck == limitLevelOne )
            {
                objectScaleBasicValue = 1 ;
            }
            if(valueCheck < limitLevelTwo )
          {
            Vector3 newScale = new Vector3(objectScaleBasicValue,objectScaleBasicValue,objectScaleBasicValue) ;
            scaleTwo.transform.localScale = newScale ;
          }
          else
           {
            if(valueCheck == limitLevelTwo )
            {
                objectScaleBasicValue = 1 ;
            }
            if(valueCheck < limitLevelThree  )
            {
              Vector3 newScale = new Vector3(objectScaleBasicValue,objectScaleBasicValue,objectScaleBasicValue) ;
              scaleThree.transform.localScale = newScale ;
            }
          }
        }}
    }
    IEnumerator Countdown()
    {
      if(gameStart)
        {Debug.Log("666");
          while (leftTime > 0)
          {
            yield return new WaitForSeconds(1f); // 暂停1秒
            leftTime--;
            //Debug.Log("Remaining time: " + leftTime);
          }
        }
    }
}
