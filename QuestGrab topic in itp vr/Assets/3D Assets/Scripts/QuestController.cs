using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Debug.Log("des");
    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
