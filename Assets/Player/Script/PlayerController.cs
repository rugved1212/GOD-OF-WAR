using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField]
    private float normalSpeed = 5.0f;
    [SerializeField]
    private float sprintSpeed = 9.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 10f;

    private float playerSpeed;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    private Animator animator;
    private bool isJumping;
    public bool isRunning;

    private InputAction moveAction;
    private InputAction jumpAction;

    // Animation
    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;

    public float acceleration = 2.0f;
    public float deceleration = 2.0f;

    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = sprintSpeed;
        }
        else
        {
            playerSpeed = normalSpeed;
        }

        controller.Move(move * Time.deltaTime * playerSpeed);

        bool isRunning = move.magnitude > 0;
        animator.SetBool("isRunning", isRunning);


        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            animator.SetTrigger("Jump");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            isJumping = true;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        // **************************************** ANIMATION **********************************************
        
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool sprintPressed = Input.GetKey(KeyCode.LeftShift);

        // set current maxVelocity
        float currentMaxVelocity = sprintPressed ? maximumRunVelocity : maximumWalkVelocity;

        //MOVEMENT OF CHARACTER

        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        if (backwardPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        if (forwardPressed && sprintPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }



        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        if (!sprintPressed && velocityZ > 0.5f)
        {
            velocityZ -= Time.deltaTime * 5f;
        }
        if (!backwardPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        animator.SetFloat("Velocity Z", velocityZ);
        animator.SetFloat("Velocity X", velocityX);




    }


    public void OnJumpAnimationEnd()
    {
        isJumping = false;
    }
}
