using UnityEngine;

public class camera : MonoBehaviour
{
    public float SensX;
    public float SensY;
    private Vector3 currentPosition;
    void Start()
    {
        currentPosition = transform.localPosition;
    }

  
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * SensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * SensY * Time.deltaTime;
        float lockMouseY = Mathf.Clamp(mouseY, -90, 90);

        transform.Rotate(mouseX, lockMouseY, 0);

       
    }
}
