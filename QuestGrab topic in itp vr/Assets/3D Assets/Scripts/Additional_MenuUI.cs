using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Additional_MenuUI : MonoBehaviour
{
   
    public void LoadMenuscene()
    {
        SceneManager.LoadSceneAsync("1_Menu");
    }
    public void reloadCurrentScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
