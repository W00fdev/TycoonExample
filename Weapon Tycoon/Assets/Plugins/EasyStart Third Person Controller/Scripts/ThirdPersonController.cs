using UnityEngine;


public class ThirdPersonController : MonoBehaviour
{
    public string IdleTriggerName;
    public string JumpTriggerName;
    public string RunTriggerName;
    
    public Camera MainCamera;
    
    public float velocity = 5f;
    public float jumpForce = 18f;
    public float jumpTime = 0.85f;
    public float gravity = 9.8f;

    float jumpElapsedTime = 0;

    // Inputs
    [SerializeField] float inputHorizontal;
    [SerializeField] float inputVertical;
    bool inputJump;

    [SerializeField] bool isJumping = false;
    [SerializeField]  private bool _isRunning;
    [SerializeField]  private bool _isIdling;

    Animator animator;
    CharacterController cc;

    private int _idleTriggerHash;
    private int _jumpTriggerHash;
    private int _runTriggerHash;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        _isIdling = true;

        _idleTriggerHash = Animator.StringToHash(IdleTriggerName);
        _jumpTriggerHash = Animator.StringToHash(JumpTriggerName);
        _runTriggerHash = Animator.StringToHash(RunTriggerName);
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputJump = Input.GetAxisRaw("Jump") == 1f;

        if (cc.isGrounded && _isIdling == false && isJumping == false && _isRunning == false
                          && inputVertical == 0 && inputHorizontal == 0)
        {
            _isIdling = true;

            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName("Idle") == false
                && animatorStateInfo.IsName("Dwarf Idle") == false
                && animatorStateInfo.IsName("Sad Idle") == false
                && animatorStateInfo.IsName("Warrior Idle") == false)
            {
                animator.SetTrigger(_idleTriggerHash);
            }
        }

        if (inputJump && cc.isGrounded && isJumping == false)
        {
            isJumping = true;
            _isIdling = false;
            _isRunning = false;
            
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") == false)
                animator.SetTrigger(_jumpTriggerHash);
        }
        else if (cc.isGrounded)
        {
            jumpElapsedTime = 0f;
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
                        animator.SetTrigger(_runTriggerHash);
                    
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
