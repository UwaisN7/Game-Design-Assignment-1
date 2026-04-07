using UnityEngine;

public class correctBlock : MonoBehaviour
{

    public Material mat;
   
    void Start()
    {
       mat.color = Color.white; 
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) {

            mat.color = Color.green;
        }
    }
}
