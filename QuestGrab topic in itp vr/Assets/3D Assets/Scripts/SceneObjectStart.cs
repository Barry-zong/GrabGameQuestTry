using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneObjectStart : MonoBehaviour
{
    [SerializeField]
    StartComDataScriptObject startComDataScriptObject;
    public GameObject sceneStartScript;
    public GameObject sceneQiuscripts;
    public GameObject buttonUI;
    public GameObject TextExplainUI;
    public GameObject scoreRecordUI;


    void Update()
    {
        
        if(startComDataScriptObject.FingertotalNumber>450)
        {
            ButtonOpenscene();
        }
    }
    public void ButtonOpenscene()
    {
        buttonUI.SetActive(false);
        TextExplainUI.SetActive(false);
        scoreRecordUI.SetActive(true);
        try { sceneStartScript.GetComponent<SceneMove>().enabled = true; }
        catch { }
        try { sceneQiuscripts.GetComponent<QiuAreaStayScore>().enabled = true; }
        catch { }
    }
}
