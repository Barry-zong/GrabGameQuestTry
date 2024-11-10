using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public bool ZtrueXfales = true;
    public float movingSpeed;
    private float initialSpeed;
    public Slider slider;
    private Vector3 sceneStartPosition;
    float defultSpeed = 0;
    public bool isStart = false ;
    // Start is called before the first frame update
    void Start()
    {
        sceneStartPosition = transform.position;
        defultSpeed = movingSpeed;
        if (slider != null)
        { slider.onValueChanged.AddListener(SliderValueChanged); }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if (ZtrueXfales)
            { transform.position = new Vector3(sceneStartPosition.x, sceneStartPosition.y, sceneStartPosition.z + initialSpeed); }
            else
            { transform.position = new Vector3(sceneStartPosition.x + initialSpeed, sceneStartPosition.y, sceneStartPosition.z); }
        }
    }
    private void FixedUpdate()
    {
        if (isStart)
        {
            initialSpeed = initialSpeed + movingSpeed;
        }
    }

    void SliderValueChanged(float newValue)
    {
        movingSpeed = defultSpeed;
        movingSpeed += newValue / 50;
    }

    public void sceneStart()
    {
        isStart = true;

    }
}



