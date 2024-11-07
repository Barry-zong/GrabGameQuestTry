using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorTopControlor : MonoBehaviour
{ 
    Collider Colliderselfcol;
    public bool openRagdoll = false;
    private bool selfCon = false;

    void Start()
    {
        Colliderselfcol = GetComponent<Collider>();
        GetRagdollBits();
        RagdollModeOff();
    }

    
    void Update()
    {

            if (openRagdoll && !selfCon)
        {
            RagdollModeOn();
            selfCon= true;
            Debug.Log("on");
        }
        if( !openRagdoll && selfCon)
        {
            RagdollModeOff();
            selfCon= false;
             Debug.Log("off");
        }

    }

    Collider[] ragDollColliders;
    Rigidbody[] limbsRigidbodies;
    void GetRagdollBits ()
    {
        ragDollColliders = gameObject.GetComponentsInChildren<Collider>();
        limbsRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
    }
    void RagdollModeOff()
    {
        foreach(Collider col in ragDollColliders) { col.enabled= false; }
        foreach(Rigidbody rig in limbsRigidbodies) { rig.isKinematic = true; }
        Colliderselfcol.enabled= true;
        GetComponent<Rigidbody>().isKinematic= false;
    }

    void RagdollModeOn()
    {
        foreach (Collider col in ragDollColliders) { col.enabled = true; }
        foreach (Rigidbody rig in limbsRigidbodies) { rig.isKinematic = false ; }
        Colliderselfcol.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

}
