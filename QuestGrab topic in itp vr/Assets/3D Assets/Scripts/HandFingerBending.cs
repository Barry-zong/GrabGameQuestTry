using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFingerBending : MonoBehaviour
{
    public float r_speed;
    private float time=0f;
    private float time1 = 0f;
    public float delayTime;
    public float midtime;
    private float degreeTest = 1f;
    private bool right,left=false;
    // Start is called before the first frame update
    void Start()
    {
        right = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        //transform.Rotate(-90, 0, 0, Space.Self);
    }
    private void FixedUpdate()
    {
        Timer();

        if(time > delayTime)
        {
            if (time1 > midtime)
            {
                if (right)
                {
                    if (degreeTest < 90)
                    {
                        PoRo();
                    }
                    else
                    {
                        degreeTest = 0;
                        right = false;
                        left = true;
                        time1 = 0;
                    }
                }
                if (left)
                {
                    if (degreeTest < 90)
                    {
                        NeRo();
                    }
                    else
                    {
                        degreeTest = 0;
                        left = false;
                        right = true;
                        time1 = 0;
                    }
                }
            }
        }
       
    }

    void PoRo()
    {
        transform.Rotate(r_speed, 0, 0, Space.Self);
        degreeTest = degreeTest+1;
        //print(degreeTest);
    }
    void NeRo()
    {
        transform.Rotate(-r_speed, 0, 0, Space.Self);
        degreeTest = degreeTest + 1;
    }
    void Timer()
    {
        time += Time.deltaTime;
        time1 += Time.deltaTime;
        
    }

}
