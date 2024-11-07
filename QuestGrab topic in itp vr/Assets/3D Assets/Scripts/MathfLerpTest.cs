using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathfLerpTest : MonoBehaviour
{

    public float maxNumber = 10,minNumber = 1 ;
    static public float t = 0;
    public float a = 5;
    private float b ;
    private float outPut=1;
    private bool change=true;
    public float iinPut = 1;
    // Start is called before the first frame update
    void Start()
    {
        b = outPut;
    }

    // Update is called once per frame
    void Update()
    {

        // outPut = Mathf.Lerp(minNumber, maxNumber, a);
        float w = iinPut/a ;
        float q = Mathf.Round(w);
        float e = q * a;
        
        outPut = e ;





        if (b != outPut)
        {
            change = true;
        }

        
        if (change)
        {
            Debug.Log(outPut);
            change = false;
            b = outPut;
        }






        







    }
}
