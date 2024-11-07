using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Time_limitor : MonoBehaviour
{
    public TextMeshProUGUI TimeRecoder;   
    private float leftTime;
    public float TotalTime=60f;
    private bool isended=false;

    public GameObject endlayoutUI;
    public GameObject startOrMenuUI;
    public GameObject previewUI1;
    public GameObject previewUI2;

    public GameObject ScoreRecord;


    // Start is called before the first frame update
    void Start()
    {
        leftTime=TotalTime;
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {
        TimeRecoder.text=leftTime+"s";
        //EndingGameUIcontrol.isEnd=false;
        if(!isended)
        {
             if(leftTime<=0)
          {
            isended= true;
            emergeEndUIlayout();
            Debug.Log("time run out");
          }
        }
       
    }

     IEnumerator Countdown()
    {
        while (leftTime > 0)
        {
            yield return new WaitForSeconds(1f); // 暂停1秒
            leftTime--;
            //Debug.Log("Remaining time: " + leftTime);
        }
    }

    void emergeEndUIlayout()
    {
        endlayoutUI.SetActive(true);
        startOrMenuUI.SetActive(true);
        previewUI1.SetActive(false);
        previewUI2.SetActive(false);
        ScoreRecord.GetComponent<QiuAreaStayScore>().Speed=0;
    }
}
