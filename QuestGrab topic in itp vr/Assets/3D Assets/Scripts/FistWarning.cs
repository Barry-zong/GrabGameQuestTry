using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistWarning : MonoBehaviour
{
    private float limitFistValue = 90 ;
    HandControl HDcontrol ;
    public GameObject handControl ;
    HandBoardCounter HDBcounter ;
    public GameObject HandBoardCounter ;
    public GameObject RemindUI;
    private bool wheathrerClose = false ;
    // Start is called before the first frame update
    void Start()
    {
        HDcontrol = handControl.GetComponent<HandControl>();
        HDBcounter = HandBoardCounter.GetComponent<HandBoardCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(HDcontrol.FingerDegreeAll>85f && HDBcounter.touchBoard)
        {
            RemindUI.SetActive(true);
             Invoke("closeObject",2f);
        }
    }
    void closeObject ()
    {
        RemindUI.SetActive(false);
    }
}
