using UnityEngine;

public class LifeCollisionDetect : MonoBehaviour
{
    public HeartUIcountControl heartUIcountControl;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoardArea"))
        {
            //Debug.Log(1);
            heartUIcountControl.DecreaseLife();
        }
    }
}