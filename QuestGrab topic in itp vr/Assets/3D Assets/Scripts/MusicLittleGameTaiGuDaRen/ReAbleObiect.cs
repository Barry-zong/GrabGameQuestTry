using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReAbleObiect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
         private void OnTriggerStay(Collider other)    
     {
           Renderer renderer = other.GetComponent<Renderer>();
          if (renderer != null)
          {
            renderer.enabled = true;
          }
        
     }
}
