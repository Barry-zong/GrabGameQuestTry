using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndUIconScene1 : MonoBehaviour
{
    public GameObject open1,close1;
    private Collider endcd;
    public GameObject scene;
    
   
   

    private void Start()
    {
        
        endcd=GetComponent<Collider>();      
    }

    
    private void OnTriggerEnter(Collider other)
    {
        
        open1.SetActive(true);
        close1.SetActive(false);
       // scene.SetActive(false);
        scene.GetComponent<SceneMove>().movingSpeed = 0f;
    }
}
