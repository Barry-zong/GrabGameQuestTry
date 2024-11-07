using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommanSceneStart : MonoBehaviour
{
    [SerializeField]
    StartComDataScriptObject startComDataScriptObject;
    public GameObject startUI1;
    public GameObject otherStartObject1;
    public GameObject otherStartObject2;
    public GameObject mirroOtherStartObject2;
    public bool restricktTime = false ;
    private bool gameOpened = false ;
    public float endingTime = 60f ;
    public TextMeshProUGUI TimeRecoder; 
    public GameObject engingUI;
    private int gameShu = 1;

    

    // Update is called once per frame
    void Update()
    {
        if(TimeRecoder!= null)
        {TimeRecoder.text=endingTime+"s";}
        if (gameOpened)
        {
            StartCoroutine(Countdown());
            gameOpened = false ;
        }
        if (gameShu == 1)
         {if(startComDataScriptObject.FingertotalNumber>450)
        {
            OpenGame();
           gameShu = 0;
        }}
        if(restricktTime)
        {
            if(endingTime<=0)
            {
                otherStartObject1.SetActive(false);
                 if(otherStartObject2 != null )
                  {otherStartObject2.SetActive(false);}
                  engingUI.SetActive(true);
            }
        }
    }

    public void OpenGame()
    {
        gameOpened = true ;
        startUI1.SetActive(false);
        otherStartObject1.SetActive(true);
        if(otherStartObject2 != null )
        {otherStartObject2.SetActive(true);}
        if(mirroOtherStartObject2 != null)
        {
            mirroOtherStartObject2.SetActive(false);
        }
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
