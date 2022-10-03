using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneDirectorScript_1 : MonoBehaviour
{
    // the player object from the Hierarchy tab
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject shooterPlayerObject;
    [SerializeField] private GameObject meleePlayerObject;
    
    //[SerializeField] private GameObject pauseUIObject;

    public float mouseDampening = .5f;
    public float stickDampening = 0f;
    public float maxVelocity = 10f;
    public float moveSpeed = 10f;
    public float launchPower = 100f;
    public float rotateSpeed = 1f;
    
    // create empty input objects
    //private Gamepad myGamepad;
    private Keyboard myKeyboard;
    private Mouse myMouse;
    private Rigidbody playerRB;
    private Gamepad myGamepad;

    private float rightMouseRotateMargin;
    private float leftMouseRotateMargin;
    
    // for moving the player
    private Vector3 playerMoveVector = new Vector3(0f,0f,0f);
    private Vector3 playerRotateVector = new Vector3(0f, 0f, 0f);
    
    // Start is called before the first frame update
    void Start()
    {
        playerRB = playerObject.GetComponent<Rigidbody>();
        // populate th input objects with actual things
        myGamepad = Gamepad.current;
        myKeyboard = Keyboard.current;
        myMouse = Mouse.current;
        
        
        if (myGamepad == null)
        {
            Debug.Log("Null Gamepad");
            return;
        }
        
        if (myKeyboard == null)
        {
            Debug.Log("Null Keyboard");
            return;
        }
        if (myMouse == null)
        {
            Debug.Log("Null Mouse");
            return;
        }
        
        meleePlayerObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        leftMouseRotateMargin = (Screen.width / 2) - ((Screen.width / 2) * mouseDampening);
        rightMouseRotateMargin = (Screen.width / 2) + ((Screen.width / 2) * mouseDampening);
        Debug.Log(myMouse.position.x.ReadValue());
        
        // if a gamepad is not connected at game start, listen for keyboard & mouse move input
        if (Gamepad.current == null) 
        {
            // forward & back
            if (myKeyboard.wKey.isPressed) 
            {
                playerMoveVector.z = 1f;
            } else if (myKeyboard.sKey.isPressed)
            {
                playerMoveVector.z = -1;
            }
            else
            {
                playerMoveVector.z = 0f;
            }
            
            // right & left
            if (myKeyboard.dKey.isPressed) 
            {
                playerMoveVector.x = 1f;
            } else if (myKeyboard.aKey.isPressed)
            {
                playerMoveVector.x = -1;
            }
            else
            {
                playerMoveVector.x = 0f;
            }
            
            // ROTATE
            if (myMouse.position.ReadValue().x > rightMouseRotateMargin)
            {
                playerRotateVector.y = 1f;
            } else if (myMouse.position.ReadValue().x < leftMouseRotateMargin)
            {
                playerRotateVector.y = -1f;
            }
            else
            {
                playerRotateVector.y = 0f;
            }

            if (myKeyboard.shiftKey.isPressed && myKeyboard.pKey.wasPressedThisFrame)
            {
                SwitchPlayerMode();
            }
        }
        else // if a gamepad is connected (not null) listen its input
        {
            // FORWARD & BACK
            if (myGamepad.leftStick.ReadValue().y > stickDampening ||
                myGamepad.leftStick.ReadValue().y < -stickDampening)
            {
                playerMoveVector.z = myGamepad.leftStick.ReadValue().y;
            }
            else
            {
                playerMoveVector.z = 0f;
            }
            
            // RIGHT & LEFT
            if (myGamepad.leftStick.ReadValue().x > stickDampening ||
                myGamepad.leftStick.ReadValue().x < -stickDampening)
            {
                playerMoveVector.x = myGamepad.leftStick.ReadValue().x;
            }
            else
            {
                playerMoveVector.x = 0f;
            }
            
            // ROTATE
            if (myGamepad.rightStick.ReadValue().x > stickDampening ||
                myGamepad.rightStick.ReadValue().x < -stickDampening)
            {
                playerRotateVector.y = myGamepad.rightStick.ReadValue().x;
            }
            else
            {
                playerRotateVector.y = 0;
            }

            if (myGamepad.buttonNorth.wasPressedThisFrame)
            {
                SwitchPlayerMode();
            }
            
        }
    }

    private void FixedUpdate()
    {
        // rotation
        playerObject.transform.eulerAngles += playerRotateVector * rotateSpeed;
        
        // movement
        if(playerRB.velocity.magnitude > maxVelocity)
        {
            playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, maxVelocity);
        }

        playerRB.AddForce(playerMoveVector * moveSpeed, ForceMode.Force);
    }

    private void SwitchPlayerMode()
    {
        if (shooterPlayerObject.activeInHierarchy)
        {
            shooterPlayerObject.SetActive(false);
            meleePlayerObject.SetActive(true);
        }
        else
        {
            meleePlayerObject.SetActive(false);
            shooterPlayerObject.SetActive(true);
        }
    }
}
