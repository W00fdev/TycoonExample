
using UnityEditor.VersionControl;
using UnityEngine;

/*
    This file has a commented version with details about how each line works. 
    The commented version contains code that is easier and simpler to read. This file is minified.
*/


/// <summary>
/// Main script for third-person movement of the character in the game.
/// Make sure that the object that will receive this script (the player) 
/// has the Player tag and the Character Controller component.
/// </summary>
public class ThirdPersonController : MonoBehaviour
{

    public Camera MainCamera;
    
    [Tooltip("Speed ​​at which the character moves. It is not affected by gravity or jumping.")]
    public float velocity = 5f;
    [Tooltip("The higher the value, the higher the character will jump.")]
    public float jumpForce = 18f;
    [Tooltip("Stay in the air. The higher the value, the longer the character floats before falling.")]
    public float jumpTime = 0.85f;
    [Space]
    [Tooltip("Force that pulls the player down. Changing this value causes all movement, jumping and falling to be changed as well.")]
    public float gravity = 9.8f;

    float jumpElapsedTime = 0;

    // Player states

    // Inputs
    [SerializeField] float inputHorizontal;
    [SerializeField] float inputVertical;
    bool inputJump;

    [SerializeField] bool isJumping = false;
    [SerializeField]  private bool _isRunning;
    [SerializeField]  private bool _isIdling;

    Animator animator;
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        _isIdling = true;
    }

    void Update()
    {
        // Input checkers
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputJump = Input.GetAxisRaw("Jump") == 1f;
        // Unfortunately GetAxis does not work with GetKeyDown, so inputs must be taken individually

        // Run and Crouch animation
        // If dont have animator component, this block wont run
        if (cc.isGrounded && _isIdling == false && isJumping == false && _isRunning == false
                          && inputVertical == 0 && inputHorizontal == 0)
        {
            _isIdling = true;
            animator.SetTrigger("Idle");
        }

        // Jump animation
        // Handle can jump or not
        if (inputJump && cc.isGrounded && isJumping == false)
        {
            isJumping = true;
            _isIdling = false;
            _isRunning = false;
            animator.SetTrigger("Jump");
            // Disable crounching when jumping
            //isCrouching = false; 
        }
        else if (cc.isGrounded)
        {
            isJumping = false;
        }

        HeadHittingDetect();
    }


    // With the inputs and animations defined, FixedUpdate is responsible for applying movements and actions to the player
    private void FixedUpdate()
    {
        // Direction movement
        float directionX = inputHorizontal * velocity * Time.deltaTime;
        float directionZ = inputVertical * velocity * Time.deltaTime;
        float directionY = 0;

        // Jump handler
        if (isJumping)
        {
            // Apply inertia and smoothness when climbing the jump
            // It is not necessary when descending, as gravity itself will gradually pulls
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;

            // Jump timer
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        // Add gravity to Y axis
        directionY = directionY - gravity * Time.deltaTime;

        // --- Character rotation --- 

        Vector3 forward = MainCamera.transform.forward;
        Vector3 right = MainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Relate the front with the Z direction (depth) and right with X (lateral movement)
        forward = forward * directionZ;
        right = right * directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);

            if (isJumping == false)
            {
                if (_isRunning == false)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slow Run") == false)
                        animator.SetTrigger("Run");
                    
                    _isRunning = true;
                }
            
                _isIdling = false;
            }
        }
        else
        {
            _isRunning = false;
        }

        // --- End rotation ---

        
        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;

        Vector3 movement = verticalDirection + horizontalDirection;
        cc.Move( movement );
    }


    //This function makes the character end his jump if he hits his head on something
    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        // Uncomment this line to see the Ray drawed in your characters head
        // Debug.DrawRay(ccCenter, Vector3.up * headHeight, Color.red);

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }

}
