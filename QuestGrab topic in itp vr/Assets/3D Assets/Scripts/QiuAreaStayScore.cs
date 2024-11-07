using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QiuAreaStayScore : MonoBehaviour
{
    public Text qiuScore ;
    public TextMeshProUGUI endQiuscore;
    private int score = 0;
    private float ss = 0;
    public float Speed = 0.2f;
    public bool lockRotate = true ;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        if(lockRotate)
        {gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;}

    }

    // Update is called once per frame
    void Update()
    {
        // Repeating("valueAdd", 1f, 1f);

        score = Mathf.RoundToInt(ss);
        if(qiuScore!= null)
        {qiuScore.text = score.ToString();}
        if(endQiuscore!=null)
        {endQiuscore.text = score.ToString();}
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag =="BoardArea")
        {         
            ss+=Speed;           
        }
    }
    void valueAdd()
    {
        score++;
    }
}
