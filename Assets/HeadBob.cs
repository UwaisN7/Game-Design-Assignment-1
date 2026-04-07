//When player moves , camera holder moves in a motion not too much to make things dizzy
//When player jumps cam holder moves slightly up and than down as if to catch the fall when landing
//Make two methods and put it to track in update 


using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.XR;


public class HeadBob : MonoBehaviour
{


    private Vector3 initialPosition; //this script is on the cam holder 

    private CharacterController cc;
    private float timer;

    [Header("Walk bob")]
    public float bobSpeed;
    public float bobAmount;
    public float bobRunSpeed;
    public float bobRunAmount;

    [Header("Jump controls ")]
    public float jumpHeight;
    public float jumpSpeed;
    public float landHeight;
    public float landSpeed;
    private bool isJumping;
    private float jumpLerpTimer;
    private float landLerpTimer;
    private float returnLerpTimer;

    private float jumpDuration;
    void Start()
    {
        cc = GetComponentInParent<CharacterController>();
        if (cc == null)
            Debug.Log("Head bob cc not found dumbass");
        initialPosition = transform.localPosition;


        // Get component character controller
        //Initialise position and rotation 
    }


    void Update()
    {
        HeadbobWalk();
        HeadbobJump();
    }

    void HeadbobWalk()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f);



        if (cc != null)
        {
            if (!cc.isGrounded)
            {
                Debug.Log("cc not grounded fix it somehow ");
                return;

            }
            if (isMoving)
            {
                timer += Time.deltaTime * bobSpeed;
                float bobX = Mathf.Sin(timer * 0.5f) * bobAmount * 0.5f;
                float bobY = Mathf.Cos(timer * 2f) * bobAmount;

                Vector3 newPosition = initialPosition + new Vector3(bobX, bobY, 0);
                transform.localPosition = newPosition;
                Debug.Log("Head bob works");
            }
            if (!isMoving)
            {
                timer = 0f;
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * 10f);
            }
            if (Input.GetKey(KeyCode.LeftShift) && isMoving)
            {
                timer += Time.deltaTime * bobRunSpeed;
                float bobX = Mathf.Sin(timer * 0.5f) * bobRunAmount * 0.5f;
                float bobY = Mathf.Cos(timer * 2f) * bobRunAmount;

                Vector3 newPosition = initialPosition + new Vector3(bobX, bobY, 0);
                transform.localPosition = newPosition;
            }

        }
        //When the char controller veolocity changes this script detects that
        // It will then cause the cam controllers transform to change constantly in a flowing like motion
        //if player stops or jumps the cam holder will return to intial position
    }

    void HeadbobJump()
    {
        float startY = initialPosition.y + jumpHeight;//calculating a pos for where the cam will go
        if (Input.GetButtonDown("Jump") && cc.isGrounded && !isJumping)
        {
            isJumping = true;
            jumpLerpTimer = 0f;
         
            Debug.Log("Jump started");
        }


        if (isJumping)
        {

            jumpLerpTimer += jumpSpeed / jumpHeight * Time.deltaTime;


            if (jumpLerpTimer > 0.1f)
            {

                Vector3 startPos = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
                Vector3 targetPosition = new Vector3(initialPosition.x, startY, initialPosition.z);

               transform.localPosition = Vector3.Lerp(startPos, targetPosition, jumpLerpTimer);
                Debug.Log("ReachedApex");

                
            }

            if (isJumping && jumpLerpTimer > 2.5f)
            {
                landLerpTimer += landSpeed / landHeight * Time.deltaTime;
                float landPos = transform.localPosition.y-initialPosition.y-landHeight;


                Vector3 startPos = new Vector3(transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
                Vector3 targetLandPosition = new Vector3(initialPosition.x,landPos,initialPosition.z);

                transform.localPosition = Vector3.Lerp(startPos, targetLandPosition, landLerpTimer);


                if (landLerpTimer >2 && cc.isGrounded)
                {
                    returnLerpTimer += landSpeed / landHeight * Time.deltaTime;
                    transform.localPosition = Vector3.Lerp(targetLandPosition,initialPosition,returnLerpTimer);
                    if (returnLerpTimer > 2)
                    {
                    isJumping = false;
                    jumpLerpTimer = 0f;
                    landLerpTimer = 0f;
                    returnLerpTimer = 0f;
                    Debug.Log("Jump complete");
                    }
                   
                }

            }
        }
    }
}
    //when player inputs jump and a bool that is grounded is true than the player will jump
    // this script will activate and cause the cam holder to go to a slightly low point as if the character wants to jump
    //Than from the lowest point it will move to a high point when the character is at its peak
    //than we need to find a way to show that the player landed by returning the cam to initial position 


    //need to give lerp some time to cook or shit will just teleport around sigggggggggggh back to tube

