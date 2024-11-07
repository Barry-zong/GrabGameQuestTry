using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LaunchBall launcher;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoardArea"))
        {
            
            Invoke("invokeObject",0.5f);
        }
    }
    private void invokeObject()
    {
        launcher.ReaetTarget();
    }
}
