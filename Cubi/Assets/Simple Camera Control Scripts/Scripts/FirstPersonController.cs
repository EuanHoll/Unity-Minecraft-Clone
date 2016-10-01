using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

    //General Movement Speed
	public float movementSpeed = 5.0f;
    
    //Sprint multiplier
    public float sprintM = 2.0f;
    
    //Camera Sensitivity
	public float Sense = 5.0f;
    
    //Jump Speed
	public float jumpSpeed = 5.0f;

    //Verticle Rotation
    float verticalRotation = 0;

    //First Person Camera Y Rotation Caps
	public float upDownRange = 59.0f;

    //Verticle Velocity
    float verticalVelocity = 0;

    //Sidewards Speed
	float sideSpeed;

    //Forward Speed
	float forwardSpeed;

    //First Person Camera
    public Camera PlayerCamera;

    //Character Controller
	CharacterController characterController;



    //Recalls Start Method
    public void Restart()
    {
        Start();
    }

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
        CameraSwitcher cs = gameObject.GetComponent<CameraSwitcher>();
        PlayerCamera = GameObject.FindWithTag(cs.PlayerCameraTag).GetComponent<Camera>();
	}

    // Update is called once per frame
    void Update()
    {

        if (PlayerCamera != null)
        {
            //Rotate FPS Camera X Mouse
            float rotLeftRight = Input.GetAxis("Mouse X") * Sense;
            transform.Rotate(0, rotLeftRight, 0);
            //Rotate FPS Camera Y Mouse
            verticalRotation -= Input.GetAxis("Mouse Y") * Sense;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
            PlayerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            /* JoyStick Support
            //Rotate FPS Camera X JoyStick
            float rotLeftRightCon = Input.GetAxis("JoyStick X") * Sense;
            transform.Rotate(0, rotLeftRightCon, 0);
            //Rotate FPS Camera Y JoyStick
            verticalRotation -= Input.GetAxis("JoyStick Y") * Sense;
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
            PlayerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
            */ 
        }


        //Move Character 
        //Checking for Run
        if (Input.GetButton("Run"))
        {
            //Setting Forward Speed * Run
            forwardSpeed = Input.GetAxis("Vertical") * movementSpeed * sprintM;
            //Setting Sideward Speed * Run
            sideSpeed = Input.GetAxis("Horizontal") * movementSpeed * sprintM;
        }
        else {
            //Setting Forward Speed(Walk)
            forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
            //Setting Sideward Speed(Walk)
            sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        }

        //Adding Gravity
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        //Checking For Jump conditions (is on the Ground)(Input Button "Jump" is Down)
        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            //Setting Jump Speed
            verticalVelocity = jumpSpeed;
        }

        //Creating Var for Character Movement
        Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

        //Adding Rotation
        speed = transform.rotation * speed;

        //Moving Character
        characterController.Move(speed * Time.deltaTime);

    }

}
