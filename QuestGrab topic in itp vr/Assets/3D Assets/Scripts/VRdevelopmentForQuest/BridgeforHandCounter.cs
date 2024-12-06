using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeforHandCounter : MonoBehaviour
{
    public bool canSendValueAdd = false ;
    // Start is called before the first frame update
    public HandBoardCounter handBoardCounter ;

    // Update is called once per frame
    void Update()
    {
        if (canSendValueAdd)
        {
            handBoardCounter.canAddedScore = true;
            canSendValueAdd = false;
        }
    }
}
