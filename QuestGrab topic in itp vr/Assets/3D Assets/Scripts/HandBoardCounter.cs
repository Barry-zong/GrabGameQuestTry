using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandBoardCounter : MonoBehaviour
{
    private int score = 0;
    public Text YourScore;
    public TextMeshProUGUI endScore;
    public bool touchBoard = false ;

    private void Update() {
        if(touchBoard)
        {
            Invoke("invokeObject",0.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BoardArea")
        {
            Destroy(other.gameObject);
            touchBoard = true ;
             score += 1;
            YourScore.text = score.ToString();
            endScore.text = score.ToString();
           
        }
    }
    
    private void resetBool()
    {
        touchBoard = false ;
    }
}
