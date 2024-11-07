using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Homepage_UI : MonoBehaviour
{
    public TextMeshProUGUI com;
    private float comnumber;
    [SerializeField]
    StartComDataScriptObject startComDataScriptObject;
    // Start is called before the first frame update
    void Start()
    {
        comnumber = startComDataScriptObject.ComPort_Record;
        com.text = comnumber+" ";
    }

    public void loadScene2_1()
    {
        SceneManager.LoadSceneAsync("2.1_test_level");
    }
    public void loadScene2_2()
    {
        SceneManager.LoadSceneAsync("2.2_test_level");
    }
    public void loadScene2_1_1()
    {
        SceneManager.LoadSceneAsync("2.1.1_demo_short_level");
    }
    public void loadSceneLittleGame_1_Music()
    {
        SceneManager.LoadSceneAsync("NewTestSmallGame_1");
    }
    public void loadSceneLittleGame_2_Bird()
    {
        SceneManager.LoadSceneAsync("NewTestSmallGame_2");
    }
    public void loadSceneLittleGame_3_Power()
    {
        SceneManager.LoadSceneAsync("NewTestSmallGame_3");
    }
    public void loadSceneLittleGame_4_LeftRight()
    {
        SceneManager.LoadSceneAsync("NewTestSmallGame_4");
    }
    public void loadSceneLittleGame_5_Jump()
    {
        SceneManager.LoadSceneAsync("NewTestSmallGame_5");
    }
    public void exitGame()
    {
       // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}

