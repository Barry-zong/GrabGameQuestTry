using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BirdJumpLevel : MonoBehaviour
{
    public GameObject gameOver;
    
    SceneMove sceneD;
    // Start is called before the first frame update
    void Start()
    {
        sceneD = GetComponent<SceneMove>();
    }

    // Update is called once per frame
    
    private void OnCollisionEnter(Collision other) {
        Debug.Log("Touched Fail");
        sceneD.enabled = false;
        gameOver.SetActive(true);
    }
}
