using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO.Ports;


public class HandControl : MonoBehaviour
{
    public float figurAddVelocity = 60;
    public float fingerDa, fingerShi, fingerZhon, fingerWu, fingerXiao = 0f;
    public Transform tFingerDa2;
    public Transform tFingerShi2;
    public Transform tFingerZhon2;
    public Transform tFingerWu2;
    public Transform tFingerXiao2;
    public float adjustValue = 3;
    public Rigidbody FlyQiuRib ;
    public Transform QiuPenpen;
    public float flyForce = 10;
    private bool goFly = false;
    private bool goBreak = true;
    public float fingerflyLimit = 200;
    public float FingerDegreeAll ;
    public AudioSource grabAudio ;
    MessageManager messageManager;
    public bool ActiveDynamicState = false ;

    float oriY;

    void Start()
    {  
        messageManager=GetComponent<MessageManager>(); 
        FingerDegreeAll = 0;
        oriY=transform.position.y;
    }
    void Update()
    { 
        FuZhi(); 
        QiuFly();
        FingerDegreeAll = (fingerWu + fingerShi + fingerDa + fingerZhon + fingerXiao)/5;

        if(ActiveDynamicState)
        {
            transform.Rotate(messageManager.Gyroscope * Time.deltaTime, Space.Self);
            // transform.Rotate(messageManager.Acceleration * Time.deltaTime, Space.Self);
            // transform.Translate(messageManager.Acceleration*Time.deltaTime,Space.Self);
            //transform.Translate(messageManager.Gyroscope* Time.deltaTime,Space.Self);
            // transform.position=new Vector3(transform.position.x,oriY,transform.position.z);
        }
    }
    private void FixedUpdate()
    { FingerAct();  }
    void FuZhi ()
    {
         float qd    = messageManager.Degree_Thumb;
         float wd = qd / adjustValue;
         float ed = Mathf.Round(wd);
         float rd = ed * adjustValue;
         if(rd < 0||rd>100)
        {
            if(rd > 130)
            {
                rd = 130;
               
            }
            if (rd < 0)
            {
                rd = 0;
            }
        }
        fingerDa = rd ;
        float qs   = messageManager.Degree_Index ;
        float ws = qs / adjustValue;
        float es = Mathf.Round(ws);
        float rs = es * adjustValue;
        if (rs < 0 || rs > 100)
        {
            if (rs > 100)
            {
                rs = 100;

            }
            if (rs < 0)
            {
                rs = 0;
            }
        }
        fingerShi = rs ;
        float qz    = messageManager.Degree_Middle ;
        float wz = qz / adjustValue;
        float ez = Mathf.Round(wz);
        float rz = ez * adjustValue;
        if (rz < 0 || rz > 100)
        {
            if (rz > 100)
            {
                rz = 100;

            }
            if (rz < 0)
            {
                rz = 0;
            }
        }
        fingerZhon = rz ;
        float qw   = messageManager.Degree_Ring ;
        float ww = qw / adjustValue;
        float ew = Mathf.Round(ww);
        float rw = ew * adjustValue;
        if (rw < 0 || rw > 100)
        {
            if (rw > 100)
            {
                rw = 100;

            }
            if (rw < 0)
            {
                rw = 0;
            }
        }
        fingerWu = rw ;
        float qx    = messageManager.Degree_Pinky ;
        float wx = qx / adjustValue;
        float ex = Mathf.Round(wx);
        float rx = ex * adjustValue;
        if (rx < 0 || rx > 100)
        {
            if (rx > 100)
            {
                rx = 100;

            }
            if (rx < 0)
            {
                rx = 0;
            }
        }
        fingerXiao = rx ;
    }
    void FingerAct()
    {
        FingerDaBend();
        FingerShiBend();
        FingerZhonBend();
        FingerWuBend();
        FingerXiaoBend();
    }
    void FingerDaBend()
    { tFingerDa2.transform.localRotation = Quaternion.Euler( fingerDa,0, 0f); }
    void FingerShiBend()
    { tFingerShi2.transform.localRotation = Quaternion.Euler(fingerShi , 0, 0f); }
    void FingerZhonBend()
    { tFingerZhon2.transform.localRotation = Quaternion.Euler(fingerZhon, 0, 0f); }
    void FingerWuBend()
    { tFingerWu2.transform.localRotation = Quaternion.Euler(fingerWu, 0, 0f); }
    void FingerXiaoBend()
    { tFingerXiao2.transform.localRotation = Quaternion.Euler(fingerXiao, 0, 0f); }
    void QiuFly ()
    {
        if (FlyQiuRib != null)
        {
            if (goBreak)
            {
                if (fingerWu + fingerShi + fingerDa + fingerZhon + fingerXiao > fingerflyLimit)
                {
                    goFly = true;
                    goBreak = false;
                }
            }
            if (goFly)
            {
                if(grabAudio!=null)
                {
                    grabAudio.Play();
                }
                FlyQiuRib.velocity = new Vector3(0, flyForce, 0f);
                goFly = false;
            }
            if (!goBreak)
            {
                if (fingerWu + fingerShi + fingerDa + fingerZhon + fingerXiao < fingerflyLimit)
                { goBreak = true; }
            }
            float qiuscale;
            float aFall, bFall;
            aFall = 20f;
            bFall = 100f;    
            qiuscale = 1-(FingerDegreeAll-aFall)/(bFall-aFall) *0.4f;
            if(qiuscale > 1)
            { qiuscale = 1; }
            if(qiuscale < 0.4f)
            { qiuscale = 0.4f; }
          QiuPenpen.transform.localScale = new Vector3(qiuscale, qiuscale, qiuscale);
        }
    }
}
