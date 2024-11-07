using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightHandMove : MonoBehaviour
{
    public GameObject hand;
    HandControl handcontrol;
    // Start is called before the first frame update
    void Start()
    {
        handcontrol = hand.GetComponent<HandControl>();
    }

    // Update is called once per frame
    void Update()
    {
        float handvalue = handcontrol.FingerDegreeAll/100;
        float moveValue = Mathf.Lerp(0,14,handvalue);
        gameObject.transform.position = new Vector3 (moveValue,0,0);
    }
}
