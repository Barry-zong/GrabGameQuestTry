using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSelfMove : MonoBehaviour
{
    public Space m_ro;
    public float m_speed ;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * m_speed * Time.deltaTime, m_ro);
    }
}
