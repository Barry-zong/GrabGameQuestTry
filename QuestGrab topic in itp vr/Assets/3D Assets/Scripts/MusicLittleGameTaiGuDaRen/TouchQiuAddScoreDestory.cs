using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TouchQiuAddScoreDestory : MonoBehaviour
{
    // Start is called before the first frame update
    HandControl FingerDegree;
    public TextMeshProUGUI ScoreGrabText ;
    public TextMeshProUGUI ScoreGrabText2 ;
    private int ScoreValue = 0 ;
    private float FingerDegreeValue = 0 ;
    private bool numberSwitch = false ;
    public GameObject hand;
    private bool fingeropen = true ;
    public AudioSource AudioBoom ;
    void Start()
    {
        //FingerDegree = GetComponent<HandControl>();
        FingerDegree = hand.GetComponent<HandControl>();
    }

    // Update is called once per frame
    void Update()
    {
        FingerDegreeValue = FingerDegree.FingerDegreeAll;
        ScoreGrabText.text = ScoreValue + " ";
        ScoreGrabText2.text = ScoreValue + " ";
        //Debug.Log(FingerDegreeValue);
        
    }
  
    private void OnTriggerEnter(Collider other) {
        if(other.tag != "HandArea" )
        {
            numberSwitch = true ;
            if(FingerDegreeValue<60)
          {
            fingeropen = true;
          }
        }
    }
    private void OnTriggerStay(Collider other)    
    {
      if(other.tag != "HandArea" )
       { 
         if (numberSwitch)
         {
             if (FingerDegreeValue>80 && fingeropen)
           {
             ScoreValue += 1;
             numberSwitch=false;
             fingeropen = false;
             Renderer renderer = other.GetComponent<MeshRenderer>();
              if (renderer != null)
              {
                renderer.enabled = false;
                AudioBoom.Play();
              }
           }
           
         }
       }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag != "HandArea" )
        { 
            numberSwitch=false;
         Rigidbody rb = other.GetComponent<Rigidbody>();
          if (rb == null)
          {
            // 如果没有刚体组件，就添加一个
           rb = other.gameObject.AddComponent<Rigidbody>();
          }
        }
    }





}
