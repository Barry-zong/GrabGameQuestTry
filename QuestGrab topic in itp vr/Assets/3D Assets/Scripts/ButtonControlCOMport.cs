using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonControlCOMport : MonoBehaviour
{
    public TextMeshProUGUI comNumberText;
    int comNumber = 0 ;
    // Start is called before the first frame update
    public void buttonpressed()
    {
        comNumber++;
        comNumberText.text = comNumber + " ";
        if (comNumber > 19)
        {
            comNumber = -1;
        }
    }
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
    }


    public string ComValue()
    {
        // return comNumberText.text;
        return comNumber.ToString();
    }
}
