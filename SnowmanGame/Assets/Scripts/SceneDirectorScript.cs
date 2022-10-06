using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using EZCameraShake;

public class SceneDirectorScript : MonoBehaviour
{
    // the player object from the Hierarchy tab
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject shooterPlayerObject;
    [SerializeField] private Transform shootingStartPosition;
    [SerializeField] private GameObject meleePlayerObject;
    [SerializeField] private GameObject projectileObjectPrefab;
    [SerializeField] private CinemachineVirtualCamera shooterCam;
    [SerializeField] private CinemachineVirtualCamera meleeCam;
    List<CinemachineVirtualCamera> allMyCameras = new List<CinemachineVirtualCamera>();

    //[SerializeField] private GameObject pauseUIObject;

    public float mouseDampening = .5f;
    public float stickDampening = 0f;
    public float maxVelocity = 10f;
    public float moveSpeed = 10f;
    public float launchPower = 100f;
    public float rotateSpeed = 1f;
    public float meleeSpeed = 500f;
    
    // create empty input objects
    //private Gamepad myGamepad;
    private Keyboard myKeyboard;
    private Mouse myMouse;
    private Rigidbody playerRB;
    private Gamepad myGamepad;

    private float rightMouseRotateMargin;
    private float leftMouseRotateMargin;
    private int meleeSpinDirection = 1;
    
    // for moving the player
    private Vector3 playerMoveVector = new Vector3(0f,0f,0f);
    private Vector3 playerRotateVector = new Vector3(0f, 0f, 0f);

    public static bool meleeModeOn = false;
    public static bool iceModeOn = false;
    public static bool meleeAttack = false;

    // Tana's stuff
    public BroomPickup broom;
    public PlayerShooting playerShooting;

    // Start is called before the first frame update
    void Start()
    {

        allMyCameras.Add(shooterCam);
        allMyCameras.Add(meleeCam);

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
        ActivateCamera(shooterCam);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (myGamepad.buttonEast.wasPressedThisFrame)
        {
            Application.Quit();
        }

        if (broom.isMeleeMode == true)
        {
            meleeModeOn = true;
        }

        if (meleeModeOn)
        {
            PlayerMeleeMode();

            playerRB.constraints &= ~RigidbodyConstraints.FreezeRotationY;

            
            if (myGamepad.rightTrigger.wasPressedThisFrame)
            {
                StartCoroutine(PlayerMeleeAttack(meleeSpeed));
            } else
            {
                meleeAttack = false;
            }
            
            ActivateCamera(meleeCam);
        } 
        else
        {
            
            if (myGamepad.rightTrigger.wasPressedThisFrame)
            {
                playerShooting.Shoot();
                //CameraShaker.Instance.ShakeOnce(.14f, .4f, .1f, .1f);
            }

        }
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
        /*
        if (myGamepad.rightStick.ReadValue().x > stickDampening ||
            myGamepad.rightStick.ReadValue().x < -stickDampening)
        {
            playerRotateVector.y = myGamepad.rightStick.ReadValue().x;
        }
        else
        {
            playerRotateVector.y = 0;
        }
        */
        float rotateAngle = Mathf.Atan2(myGamepad.rightStick.ReadValue().x, myGamepad.rightStick.ReadValue().y);
        rotateAngle *= Mathf.Rad2Deg;
        playerRotateVector.y = rotateAngle;
    }

    private void FixedUpdate()
    {
        if (iceModeOn == true)
        {
            maxVelocity = 15f;
            moveSpeed = 12;
        }
        else
        {
            maxVelocity = 20f;
            moveSpeed = 15f;
        }
        
        // rotation
        //playerObject.transform.eulerAngles += playerRotateVector * rotateSpeed;
        if (playerRotateVector.y != 0)
        {
            playerObject.transform.rotation = Quaternion.Lerp(playerObject.transform.rotation, Quaternion.Euler(playerRotateVector), Time.deltaTime * rotateSpeed);
        }
        
        
        // movement
        if(playerRB.velocity.magnitude > maxVelocity)
        {
            playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, maxVelocity);
        }

        playerRB.AddForce(playerMoveVector * moveSpeed * 10f, ForceMode.Force);
    }

    public void PlayerMeleeMode()
    {
        if (!meleePlayerObject.activeInHierarchy)
        {
            meleePlayerObject.SetActive(true);
            shooterPlayerObject.SetActive(false);
        }
    }

    private void PlayerShooterMode()
    {
        if (!shooterPlayerObject.activeInHierarchy)
        {
            meleePlayerObject.SetActive(false);
            shooterPlayerObject.SetActive(true);
        }
    }
/*
    void PlayerFireProjectile()
    {
       

        float whereIWantToBeX = shootingStartPosition.position.x;
        float whereIWantToBeY = shootingStartPosition.position.y;
        float whereIWantToBeZ = shootingStartPosition.position.z;
        
        GameObject projectile = Instantiate(projectileObjectPrefab, new Vector3(whereIWantToBeX, whereIWantToBeY, whereIWantToBeZ), playerObject.transform.rotation); 
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.AddRelativeForce(new Vector3(0, 0, launchPower), ForceMode.Impulse);

    }
*/
    private IEnumerator PlayerMeleeAttack(float speed)
    {
        
        playerRB.AddTorque(0, speed * meleeSpinDirection *10f, 0, ForceMode.Impulse);
        yield return new WaitForSecondsRealtime(.25f);
        meleeAttack = false;
        meleeSpinDirection *= -1;
    }

    private void ActivateCamera(CinemachineVirtualCamera activeCamera)
    {
        if(activeCamera.Priority != 10)
        {
            foreach(CinemachineVirtualCamera cam in allMyCameras)
            {
                cam.Priority = 0;
            }
            activeCamera.Priority = 10;
        }
    }
}