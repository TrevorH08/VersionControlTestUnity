
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
 
//This is made by Bobsi Unity - Youtube
public class PlayerController : NetworkBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float buttSize = 300000.0f;

    public float jigglePhysics = 50000000000.0f;
 
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    [HideInInspector]
    public bool canMove = true;
 
    [SerializeField]
    private float cameraYOffset = 15f;
    private Camera playerCamera;

    private Vector3 mouse_pos;
    private Vector3 object_pos;
    private float angle;
 
 //this is the only multiplayer part
    public override void OnStartClient()
    {
    //this allows us to only spawn one camera per session
    //it assigns the main camera to the owner when their client starts
        base.OnStartClient();
        if (base.IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
            playerCamera.transform.Rotate(90.0f, 0.0f, 0.0f);
        }
        else
        {   
            //disables player controller scripts we are not the owner of.
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }
 
    void Start()
    {
        characterController = GetComponent<CharacterController>();
 
        // Lock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
 
    void Update()
    {
        bool isRunning = false;
        //this is a really, really shitty way of making the camera attach to the player, but not be influenced by its position/rotation (since it's not the child)
        playerCamera.transform.position = new Vector3(transform.position.x, cameraYOffset, transform.position.z);
 
        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
 
        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
 
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
 
        // Player and Camera rotation
        if (canMove && playerCamera != null)
        {

            // rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            // rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            // transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

            // mouse_pos = Input.mousePosition;
            // mouse_pos.z = cameraYOffset; 
            // object_pos = Camera.main.WorldToScreenPoint(transform.position);
            // mouse_pos.x = mouse_pos.x - object_pos.x;
            // mouse_pos.y = mouse_pos.y - object_pos.y;
            // angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            // transform.rotation *= Quaternion.Euler(new Vector3(0, -angle + 90, 0));
     mouse_pos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y, cameraYOffset));
     transform.LookAt (mouse_pos);
        }
    }
}