using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelfRotation : MonoBehaviour
{
    public float rotationSpeed = 1;
    private float rotationvalue=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, rotationvalue, 0f);
    }
    private void FixedUpdate()
    {
        rotationvalue += rotationSpeed ;
    }
}
