using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUIconScene1 : MonoBehaviour
{
    public GameObject open1,open2,open3, close1;
    private Collider endcd;
    public GameObject scene;
    public bool levelEnding = false;
  
   

    private void Start()
    {
        levelEnding = false;
        endcd =GetComponent<Collider>();      
    }

    
    private void OnTriggerEnter(Collider other)
    {
        
        open1.SetActive(true);
        open2.SetActive(true);
        open3.SetActive(true);
        close1.SetActive(false);
        levelEnding = true;
       // scene.SetActive(false);
        scene.GetComponent<SceneMove>().movingSpeed = 0f;
    }
}
