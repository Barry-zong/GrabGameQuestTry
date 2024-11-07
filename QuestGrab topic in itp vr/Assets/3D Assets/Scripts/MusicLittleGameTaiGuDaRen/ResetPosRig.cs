using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosRig : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
     Rigidbody rb = other.GetComponent<Rigidbody>();
    if (rb != null)
    {
        Destroy(rb);
    }
    Transform otherTransform = other.gameObject.GetComponent<Transform>();
    if (otherTransform != null) {
        Vector3 position = otherTransform.position;
        position.y = 0;
        otherTransform.position = position;
    }
    }
}
