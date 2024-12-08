using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUIconScene1 : MonoBehaviour
{
    public GameObject open1,open2,open3,open4, close1;
   
    
    public bool levelEnding = false;
    public bool levellosing = false;
    public StartScene1AudioPlay startScene1AudioPlay;


    private void Start()
    {
        levelEnding = false;
        open4.SetActive(false);
    }

    private void Update()
    {
        if (levelEnding)
        {
            endingFunction();
            
        }
        if (levellosing)
        {
            losingFunction();
           
        }
    }
    void losingFunction()
    {
        startScene1AudioPlay.StopNormalBackgroundAudio();
      //  open1.SetActive(true);
        open2.SetActive(true);
        open3.SetActive(true);
        open4.SetActive(true);
        close1.SetActive(false);
      
    }
    void endingFunction() 
    {
        
        open1.SetActive(true);
        open2.SetActive(true);
        open3.SetActive(true);
        open4.SetActive(true);
        close1.SetActive(false);
        
    }
}
