using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndingArea : MonoBehaviour
{
    public GameObject endingUI;

    
    void Start()
    {
         Time.timeScale = 1 ;
    }

    
    void Update()
    {
        
    }
    /* private void OnCollisionEnter(Collision other) {
         if (other.gameobject.tag=="EndArea")
        {
            GameStop();
        }
    }*/
   private void OnTriggerEnter(Collider other) {
        
            GameStop();
        
    } 
    private void GameStop()
    {
        Time.timeScale = 0 ;
        endingUI.SetActive(true);

    }
}
