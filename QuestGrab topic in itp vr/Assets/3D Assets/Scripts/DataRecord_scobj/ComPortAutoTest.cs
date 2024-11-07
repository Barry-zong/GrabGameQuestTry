using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ComPortAutoTest : MonoBehaviour
{
    [SerializeField]
    StartComDataScriptObject startComDataScriptObject;
    public TextMeshProUGUI ComText;
    public int Com_port;
    public float com_allnumber;
    public GameObject serialcomport;
    public GameObject addcomvalueButton;
    public GameObject succecedIcon;
    void Start()
    {
        Com_port = startComDataScriptObject.ComPort_Record;
        ComText.text = Com_port + " ";
    }
    void Update()
    {
        com_allnumber = startComDataScriptObject.FingertotalNumber;
        if (com_allnumber!=0)
        {
            addcomvalueButton.gameObject.SetActive(false);
            succecedIcon.gameObject.SetActive(true);
            Debug.Log("COM conected ");
            startComDataScriptObject.ComPort_Record = Com_port;
            SceneManager.LoadSceneAsync("1_Menu");
        }
    }
    private void FixedUpdate()
    {// serialcomport.GetComponent<SerialController>().enabled = true;
     }

    public void restThePortnumber()
    {
        Com_port = 0;
        ComText.text = Com_port + " ";
        Debug.Log(Com_port);
        startComDataScriptObject.ComPort_Record = Com_port;
    }
    public void combuttonGetDown()
    {
       // serialcomport.GetComponent<SerialController>().enabled = false;
        Com_port = startComDataScriptObject.ComPort_Record;
            if (com_allnumber == 0)
            {
                    if (Com_port < 150)
                    {
                        Com_port += 1;
                    }
                    else
                    {
                        Com_port = 0;
                    }
                    startComDataScriptObject.ComPort_Record = Com_port;
            }    
        ComText.text = Com_port + " ";
        Debug.Log(Com_port);
    }
}
