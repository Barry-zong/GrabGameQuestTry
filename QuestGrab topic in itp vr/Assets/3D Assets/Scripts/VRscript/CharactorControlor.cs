using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class CharactorControlor : MonoBehaviour
{
    [SerializeField]
    StartComDataScriptObject startComDataScriptObject;
    [Range(0, 10)]
    public int AssignFingerNumber = 0;
    [Range(1, 3)]
    public int RotateXYZ = 1;
    [Range(1, 3)]
    public int Degree60_90_130 = 1;
    public bool convertDirection = false;

    private float LFinger1, LFinger2, LFinger3, LFinger4, LFinger5,LFinger6,LFinger7,LFinger8,LFinger9,LFinger10 = 0f ;
    private Quaternion currentRotation;
    private Vector3 currentEulerAngles;

    // Start is called before the first frame update
    void Start()
    {
         currentRotation = gameObject.transform.localRotation; //get local gameobject's self rotation value
         currentEulerAngles = currentRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        GetFingerState();
        RotateBoneMove();
       
    }

    void GetFingerState ()
    {
        LFinger1 = startComDataScriptObject.Finger1;
        LFinger2 = startComDataScriptObject.Finger2;
        LFinger3 = startComDataScriptObject.Finger3;
        LFinger4 = startComDataScriptObject.Finger4;
        LFinger5 = startComDataScriptObject.Finger5;
        LFinger6 = startComDataScriptObject.Finger6;
        LFinger7 = startComDataScriptObject.Finger7;
        LFinger8 = startComDataScriptObject.Finger8;
        LFinger9 = startComDataScriptObject.Finger9;
        LFinger10 = startComDataScriptObject.Finger10;
    }

    void RotateBoneMove()
    {
        float DegreeValue = 0;
        float AdjustedValue = 0;
        if(AssignFingerNumber == 1)
        {
           // DegreeValue = LFinger1;
         }
        if (AssignFingerNumber == 2)
        {
            DegreeValue = LFinger2;
        }
        if (AssignFingerNumber == 3)
        {
            DegreeValue = LFinger3;
        }
        if (AssignFingerNumber == 4)
        {
            DegreeValue = LFinger4;
        }
        if (AssignFingerNumber == 5)
        {
            DegreeValue = LFinger5;
        }
        if (AssignFingerNumber == 6)
        {
            DegreeValue = LFinger6;
        }
        if (AssignFingerNumber == 7)
        {
            DegreeValue = LFinger7;
        }
        if (AssignFingerNumber == 8)
        {
            DegreeValue = LFinger8;
        }
        if (AssignFingerNumber == 9)
        {
            DegreeValue = LFinger9;
        }
        if (AssignFingerNumber == 10)
        {
            DegreeValue = LFinger10;
        }

        if ( Degree60_90_130 == 1)
        {
            AdjustedValue = (DegreeValue / 13) * 6;
        }
        if (Degree60_90_130 == 2)
        {
            AdjustedValue = (DegreeValue / 13) * 9; 
        }
        if (Degree60_90_130 == 3)
        {
            AdjustedValue = DegreeValue;
        }
        if(convertDirection)
        {
            AdjustedValue = -AdjustedValue;
        }

       

        if (RotateXYZ == 1)
        { gameObject.transform.localRotation = Quaternion.Euler(currentEulerAngles.x + AdjustedValue, currentEulerAngles.y, currentEulerAngles.z); }
        if (RotateXYZ == 2)
        { gameObject.transform.localRotation = Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y + AdjustedValue, currentEulerAngles.z); }
        if (RotateXYZ == 3)
        { gameObject.transform.localRotation = Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y, currentEulerAngles.z + AdjustedValue); }

        //Debug.Log(AdjustedValue);
    }
}
