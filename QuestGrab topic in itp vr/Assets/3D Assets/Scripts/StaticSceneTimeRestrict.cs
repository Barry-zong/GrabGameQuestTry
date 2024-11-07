using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StaticSceneTimeRestrict : MonoBehaviour
{
    [SerializeField]
    StartComDataScriptObject startComDataScriptObject;
    public bool restricktTime = false ;
    private bool gameOpened = false ;
    public float endingTime = 120f ;
    public TextMeshProUGUI TimeRecoder; 
    public GameObject engingUI;
    public GameObject openUI;
    private int valueRe = 1;

    private void Start() {
         Time.timeScale = 1 ;
    }

    // Update is called once per frame
    void Update()
    {
        TimeRecoder.text=endingTime+"s";
        if (gameOpened)
        {
            StartCoroutine(Countdown());
            gameOpened = false ;
        }
        if (valueRe == 1)
         {if(startComDataScriptObject.FingertotalNumber>450)
        {
           OpenGame();
           valueRe = valueRe -1;
        }}
        if(restricktTime)
        {
            if(endingTime<=0)
            {
                  engingUI.SetActive(true);
                  Time.timeScale = 0 ;
            }
        }
    }

    public void OpenGame()
    {
        gameOpened = true ;
        openUI.SetActive(false);
       
    }
    IEnumerator Countdown()
    {
        while (endingTime > 0)
        {
            yield return new WaitForSeconds(1f); // 暂停1秒
            endingTime--;
            //Debug.Log("Remaining time: " + leftTime);
        }
    }
}
