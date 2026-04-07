using System.Collections;
using UnityEngine;

public class wrongBlock : MonoBehaviour
{
    public Material mat;
    private Rigidbody rb;

    void Start()
    {
        mat.color = Color.white;
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            mat.color = Color.red;

            StartCoroutine(StartFall());
        }
    }

    IEnumerator StartFall()
    {
        //When player steps on block , 
        //block begins to shake 
        //block falls down 
        float shakeDuration = 2f;
        float shakeMagnitude = 1.0f;
        float shakeSpeed = 50f;
        float shakeTime = 0f;
         Vector3 originalPos = transform.position;

        while (shakeTime  < shakeDuration)
        {
            float shakeX = Mathf.Sin (shakeTime*shakeSpeed) *shakeMagnitude;
            float shaeY = Mathf.Cos(shakeTime * shakeSpeed *0.5f) * shakeMagnitude;

            transform.position = originalPos+new Vector3 (shakeX, shaeY, 0);

            shakeTime += Time.deltaTime;
            yield return null;  
        }
       transform.position = originalPos;
        yield return new WaitForSeconds (0.5f);

        rb.isKinematic = false;
    }
}
