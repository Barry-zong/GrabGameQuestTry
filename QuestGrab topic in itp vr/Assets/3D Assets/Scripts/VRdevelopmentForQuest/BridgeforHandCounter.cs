using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeforHandCounter : MonoBehaviour
{
    public bool canSendValueAdd = false ;
    // Start is called before the first frame update
    public HandBoardCounter handBoardCounter ;
    public EndUIconScene1 endUIconScene ;
    public LevelGenerator levelGenerator ;
    public StartScene1AudioPlay startSceneaudio ;
    // Update is called once per frame
    void Update()
    {
        if (canSendValueAdd)
        {
            handBoardCounter.canAddedScore = true;
            canSendValueAdd = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            endUIconScene.levelEnding = true;
            levelGenerator.timeOver = true;
            startSceneaudio.mainBackgroundMusicPlay = false;
        }
    }
}
