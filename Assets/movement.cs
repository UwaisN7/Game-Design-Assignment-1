//If player inputs shift than double movement speed or plus sprint speed to move speed boom
// If player input space than add an upward force to the character controller i think its still a rigidbody 

using UnityEngine;
using UnityEngine.InputSystem.XR;

public class movement : MonoBehaviour
{
    [Header ("Movement")]
    public float movementSpeed;
    public float walkSpeed;
    public float run;
    public float jumpForce;
    public float gravity;
    private bool isSprinting;

    private CharacterController cc;
    private Camera cam;
    private Vector3 velocity;
    
    private float verticalRotation;
    [Header("Camera Settings")]
    public float cameraSensitivity;
    public float maxLookAngle;

    [Header("Slide")]
    public Transform orientation;
    public Transform playerObj;
    public float slideSpeed;
    public float slideFriction;
    public float slideHeight;
    public float slideLength;
    public float slideTimer;
    private bool isSliding;
    private Vector3 slideVelocity;
    private float slideYScale = 0.5f;
    private float startYScale;
    private float originalHeight;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        movementSpeed= walkSpeed;
        originalHeight = cc.height;
        startYScale = playerObj.localScale.y;

    }


    void Update()
    {
        Movement();
        HandleCamera();  
    }


    void Movement()
    {
        //Walk
        if (cc.isGrounded && velocity.y < 0f) 
        {
            velocity.y = -2f;
        
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
         
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        float movedirection =  velocity.y += gravity * Time.deltaTime;

        if ( !isSliding)
        {
            cc.Move(move * movementSpeed * Time.deltaTime);

            cc.Move(velocity * Time.deltaTime);
        }
        
        
        //sprint
        if (cc.isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = run;
            isSprinting = true;
            //slideSpeed += 2f;   haaaai this causes bugs leave it out they wont even know until we figure it out
        }
        else
        {
            movementSpeed = walkSpeed;
            isSprinting = false;
            slideSpeed *= 1f;
        }

        //jump here
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isSliding && !isSprinting)
            {

                Vector3 horizontalVelocity = new Vector3(velocity.x,0,velocity.z);


                
                float slideJumpForce = jumpForce + slideSpeed/30; //verticla shi need horizontal for slide speed do not touch this its perfect
                velocity.y = Mathf.Sqrt(slideJumpForce * -0.8f * gravity);
                Vector3 slideLaunch = new Vector3(slideLength, slideJumpForce, 0);

                velocity.x = horizontalVelocity.x * slideLength;
                velocity.z = horizontalVelocity.z * 3f;

                isSliding = false; 
                Debug.Log("Slide Jump Works");

                StopSlide();
            }
            else if (cc.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravity);
            }
        }



        //sliding
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
        {
            isSliding = true;
            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
            slideTimer = 5;
            Vector3 inputDirection = orientation.forward * vertical + orientation.right * horizontal;
            Vector3 currentVelocity = new Vector3(velocity.x, slideLength, velocity.z);
            slideVelocity = Vector3.Project(inputDirection.normalized,currentVelocity);
            slideVelocity = inputDirection * slideSpeed;

            cc.height = slideHeight;
        }
        if (isSliding)
        {
            slideTimer += slideFriction* Time.deltaTime;
            cc.Move(slideVelocity * Time.deltaTime);

            slideVelocity = Vector3.Lerp( slideVelocity, Vector3.zero, slideTimer * Time.deltaTime);

            if (!cc.isGrounded)
            {
                slideVelocity.y += gravity + -5 * Time.deltaTime;
            }
            
            if (slideVelocity.magnitude < 2f )
            {
                StopSlide();
            }
        }

    }

    void HandleCamera()
    {
        if (cam!=null)
        {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity *Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);
            Debug.Log("Camera found");

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
            cam.transform.localRotation = Quaternion.Euler(verticalRotation,0,0);

        }
    }
    void StopSlide()
    {
        isSliding = false;
        isSprinting =false;
        slideTimer = 0;
        cc.height = originalHeight;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

}
