using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField]
    StartComDataScriptObject startComDataScriptObject;
   // fingerDetectMeta handData = FindObjectOfType<fingerDetectMeta>();
   // private SerialController serialController;
    public bool whetherLeftHand = false;
    public float Degree_Thumb;
    public float Degree_Index;
    public float Degree_Middle;
    public float Degree_Ring;
    public float Degree_Pinky;
    public bool whetherNewVersion = false;
    public float Accel_x;
    public float Accel_y;
    public float Accel_z;
    public float Gyro_x;
    public float Gyro_y;
    public float Gyro_z;
    public float Allnumbers;
    //移动加速度和旋转加速度
    public Vector3 Acceleration;
    public Vector3 Gyroscope;
    public float AccelerSensitivity=1.0f;

    // Initialization
    void Start()
    {
       // serialController=GetComponent<SerialController>();
    }

    public float FingerValueCalculate(float a)
    {
        float b = a / 2;
        float c = Mathf.Round(b);
        float rd = c*2;
        if (rd < 0 || rd > 100)
        {
            if (rd > 130)
            {
                rd = 130;

            }
            if (rd < 0)
            {
                rd = 0;
            }
        }
        return rd;
    }

    // Executed each frame
    void Update()
    {
        // string message = serialController.ReadSerialMessage();
        /*  if (message == null)
              return;

        //  Debug.Log(message);
        */


        // Check if the message is plain data or a connect/disconnect event.
        // if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
        //     Debug.Log("Connection established");
        //  else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
        //       Debug.Log("Connection attempt failed or disconnection detected");
        //else
        //{
        // string[] m = message.Split(",");
        

        Degree_Thumb = 1;
            Degree_Index = 1;
        Degree_Middle = 1;
        Degree_Ring = 1;
        Degree_Pinky = 1;
        // other input
        if (whetherNewVersion)
            {
                
                Accel_x = 1;
                Accel_y = 2;
                Accel_z = 3;
                Gyro_x = 4;
                Gyro_y =5;
                Gyro_z = 6;
                //移动和旋转向量赋值
                //Acceleration = new Vector3(Mathf.Abs(float.Parse(m[5])) > AccelerSensitivity ? float.Parse(m[5]) :0,Mathf.Abs(float.Parse(m[7])) > AccelerSensitivity ? float.Parse(m[7]) : 0 ,Mathf.Abs(float.Parse(m[6])) > AccelerSensitivity ? float.Parse(m[6]) : 0);
      //          Acceleration = new Vector3(Mathf.Abs(float.Parse(m[5])) > AccelerSensitivity ? float.Parse(m[5]) : 0, Mathf.Abs(float.Parse(m[6])) > AccelerSensitivity ? float.Parse(m[6]) : 0, Mathf.Abs(float.Parse(m[7])) > AccelerSensitivity ? float.Parse(m[7]) : 0);

       //         Gyroscope = new Vector3(float.Parse(m[8]), -float.Parse(m[9]), float.Parse(m[10]));
            }
       // }

        if (whetherLeftHand)
        {
            Allnumbers = Degree_Thumb + Degree_Index + Degree_Middle + Degree_Pinky + Degree_Ring;

            startComDataScriptObject.FingertotalNumberLeft = Allnumbers;



            startComDataScriptObject.Finger10= FingerValueCalculate(Degree_Thumb);
            startComDataScriptObject.Finger9 = FingerValueCalculate(Degree_Index);
            startComDataScriptObject.Finger8 = FingerValueCalculate(Degree_Middle);
            startComDataScriptObject.Finger7 = FingerValueCalculate(Degree_Ring);
            startComDataScriptObject.Finger6= FingerValueCalculate(Degree_Pinky);
        }
        else
        {
            Allnumbers = Degree_Thumb + Degree_Index + Degree_Middle + Degree_Pinky + Degree_Ring;

            startComDataScriptObject.FingertotalNumber = Allnumbers;



            startComDataScriptObject.Finger5 = FingerValueCalculate(Degree_Thumb);
            startComDataScriptObject.Finger4 = FingerValueCalculate(Degree_Index);
            startComDataScriptObject.Finger3 = FingerValueCalculate(Degree_Middle);
            startComDataScriptObject.Finger2 = FingerValueCalculate(Degree_Ring);
            startComDataScriptObject.Finger1 = FingerValueCalculate(Degree_Pinky);
        }
    }
}
