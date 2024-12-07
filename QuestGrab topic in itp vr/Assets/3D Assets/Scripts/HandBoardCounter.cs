using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandBoardCounter : MonoBehaviour
{

    private int score = 0;
    public Text YourScore;
   // public TextMeshProUGUI endScore;
    public bool touchBoard = false ;
    //public AudioClip win;
    private AudioSource winSource;
    public bool canAddedScore = false;
    public FireworkSpawner fireworkSpawner;

    private void Start()
    {
        winSource = GetComponent<AudioSource>();
    }

    private void Update() {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(canAddedScore) {
           // Debug.Log("getcanaddscore");
          if (other.gameObject.tag == "BoardArea")
          {
            Destroy(other.gameObject);
            touchBoard = true ;
                fireworkSpawner.fire = true;
             score += 1;
            YourScore.text = score.ToString();
          //  endScore.text = score.ToString();
            winSource.Play();
          //  Debug.Log("audioShouldPlay");
                canAddedScore = false ;
           }
        }
    }
    
    private void resetBool()
    {
        touchBoard = false ;
    }
}
