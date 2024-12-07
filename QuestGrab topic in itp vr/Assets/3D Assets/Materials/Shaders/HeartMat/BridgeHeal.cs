using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHeal : MonoBehaviour
{
    public HeartUIcountControl heartUIcountControl;
    // Start is called before the first frame update
    
    public void HealAddLife()
    {
        heartUIcountControl.IncreaseLife();
    }
    
}
