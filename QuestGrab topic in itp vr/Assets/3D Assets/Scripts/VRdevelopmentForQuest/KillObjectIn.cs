using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjectIn : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerExit(Collider other)
    {
        // �����뿪����������Ϸ����
        Destroy(other.gameObject);
    }
}
