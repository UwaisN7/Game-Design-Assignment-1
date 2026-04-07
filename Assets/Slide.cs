using UnityEngine;

public class Slide : MonoBehaviour
{
    [Header(("Reference"))]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private movement playerMovement;

    [Header("Sliding")]
    public float slideForce;

    public float slideYScale;
    public float startYScale;

    private bool isSliding;
    
    void Start()
    {
        rb =GetComponent<Rigidbody>();
        playerMovement = GetComponent<movement>();
       

        startYScale = playerObj.localScale.y;
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftControl) && (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f))
        {
            StartSlide();
        }

         if (Input.GetKeyUp(KeyCode.LeftControl) && isSliding)
        {
            StopSlide();
            Debug.Log("Helloooo unity stop sliding wtf");
        }
    }

    void FixedUpdate()
    {
       if (isSliding)
        {
            SlidingMovement();
        } 
    }

    void StartSlide()
    {
        isSliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);

        SlidingMovement();
            

    }
    
    void SlidingMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 velocityInDirection = Vector3.Project(currentVelocity, inputDirection.normalized);
        rb.linearVelocity -= velocityInDirection;

        
        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

    }

    void StopSlide()
    {
        isSliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
