using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour {

    //Camera GameObject Tags
    public string PlayerCameraTag;

    //Do you use a Keycode for First and Third Person swapping(if so set True in editor)
    public bool isKeyCode;

    //First Person/Third Person Camera Switching Key
    public KeyCode Key;

    //First Person/Third Person Camera Switching Input
    public string inputName;

    //Do you want to use the RTS Camera
    public bool useRTSCamera;

    //Do you use KeyCode for RTS Camera Switching
    public bool RTSIsKeyCode;

    //RTS Camera Switching Key
    public KeyCode RTSKey;

    //RTS Camera Switching Input
    public string RTSInputName;

    //First Person Camera Position (Local)
    public Vector3 localFirstPersonPos;

    //Third Person Camera Position (Local)
    public Vector3 localThirdPersonPos = new Vector3(0, 2, -5);

    //RTS Camera Position (Local)
    public Vector3 localRTSCameraPos = new Vector3(0, 200, 0);

    //Sets Starting Camera View (3rd or 1st Person)
    public bool isFirstPerson = true;
    
    //Is RTS Camera Enabled
    public bool isRTSCameraEnabled = false;

    //Camera GameObjects
    GameObject PlayerCamera;

	// Use this for initialization
	void Start () {
        //Getting Player Camera
        if (GameObject.FindWithTag(PlayerCameraTag) != null)
        {
            PlayerCamera = GameObject.FindWithTag(PlayerCameraTag);
        } else
        {
            Debug.LogError("There is no First Person Camera");
        }
        //Setting Camera for First Person
        if (isFirstPerson)
        {
            //Enabling Player Control
            gameObject.GetComponent<CharacterController>().enabled = true;
            gameObject.GetComponent<FirstPersonController>().enabled = true;
            //Setting up 1st Person Mode
            PlayerCamera.transform.localPosition = localFirstPersonPos;
            PlayerCamera.GetComponent<CameraCollision>().enabled = false;
            PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = false;
            //Disabling RTSCamera Mode
            PlayerCamera.GetComponent<RTSCamera>().enabled = false;
        }
        //Setting Camera for Third Person
        else
        {
            //Enabling Player Controls
            gameObject.GetComponent<CharacterController>().enabled = true;
            gameObject.GetComponent<FirstPersonController>().enabled = true;
            //Setting up 3rd Person Mode
            PlayerCamera.transform.localPosition = localThirdPersonPos;
            PlayerCamera.GetComponent<CameraCollision>().enabled = true;
            PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = true;
            //Setting localThirdPersonPos Maximun distance away from Player
            localThirdPersonPos = PlayerCamera.transform.localPosition;
            //Disabling RTS Camera
            PlayerCamera.GetComponent<RTSCamera>().enabled = false;
        }
        if (useRTSCamera == true)
        {
            if (isRTSCameraEnabled)
            {
                //Disabling Player Controls
                gameObject.GetComponent<CharacterController>().enabled = false;
                gameObject.GetComponent<FirstPersonController>().enabled = false;
                //Setting up RTS Mode
                PlayerCamera.transform.localPosition = localRTSCameraPos;
                PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                //RTS Camera Enabling
                PlayerCamera.GetComponent<RTSCamera>().enabled = true;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
        
        //using KeyCode
        if (isKeyCode)
        {
            //Is KeyCode Down?
            if (Input.GetKeyDown(Key))
            {
               //Is First Person enabled and RTS disabled
               if(isFirstPerson == true && isRTSCameraEnabled == false)
                {
                    //Setting up Third Person Mode
                    PlayerCamera.transform.localPosition = localThirdPersonPos;
                    PlayerCamera.GetComponent<CameraCollision>().enabled = true;
                    PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = true;
                    isFirstPerson = false;
                    //Disabling RTS Camera
                    PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                }
                //Is Third Person enabled and RTS disabled
                else if (isFirstPerson ==  false && isRTSCameraEnabled == false)
                {
                    //Setting up First Person Mode
                    PlayerCamera.transform.localPosition = localFirstPersonPos;
                    PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                    PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = false;
                    isFirstPerson = true;
                    //Disabling RTS Camera
                    PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                } 
            }
        }
        else
        {
            //Checking if Button Down
            if (Input.GetButtonDown(inputName))
            {
                //Is First Person enabled and RTS disabled
                if (isFirstPerson == true && isRTSCameraEnabled == false)
                {
                    //Setting up Third Person Mode
                    PlayerCamera.transform.localPosition = localThirdPersonPos;
                    PlayerCamera.GetComponent<CameraCollision>().enabled = true;
                    PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = true;
                    isFirstPerson = false;
                    //Disabling RTS Camera
                    PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                }
                //Is Third Person enabled and RTS disabled
                else if (isFirstPerson == false && isRTSCameraEnabled == false)
                {
                    //Setting up First Person Mode
                    PlayerCamera.transform.localPosition = localFirstPersonPos;
                    PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                    PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = false;
                    isFirstPerson = true;
                    //Disabling RTS Camera
                    PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                }
            }
        }
        
        //Checking if using RTS Camera
        if (useRTSCamera == true)
        {
            //Using KeyCode
            if (RTSIsKeyCode)
            {
                //Checking if KeyCode Down
                if (Input.GetKeyDown(RTSKey))
                {
                    //Swapping from RTS to FPS or 3rd Person
                    if (isRTSCameraEnabled == true)
                    {
                        //Enabling Character Controller
                        gameObject.GetComponent<CharacterController>().enabled = true;
                        gameObject.GetComponent<FirstPersonController>().enabled = true;
                        //First Person Enabling
                        if (isFirstPerson == true)
                        {
                            PlayerCamera.transform.localPosition = localFirstPersonPos;
                            PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                            PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                            PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = false;
                        }
                        //Third Person Enabling
                        else
                        {
                            PlayerCamera.transform.localPosition = localThirdPersonPos;
                            PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                            PlayerCamera.GetComponent<CameraCollision>().enabled = true;
                            PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = true;
                        }
                        isRTSCameraEnabled = false;
                    }
                    else
                    {
                        //Disbaling Character Controls
                        gameObject.GetComponent<CharacterController>().enabled = false;
                        gameObject.GetComponent<FirstPersonController>().enabled = false;
                        //Setting up RTS Mode
                        PlayerCamera.transform.localPosition = localRTSCameraPos;
                        PlayerCamera.GetComponent<RTSCamera>().enabled = true;
                        PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                        PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = false;
                        isRTSCameraEnabled = true;
                    }
                }
            }
            else
            {
                //Checking if Button Down
                if (Input.GetButtonDown(RTSInputName))
                {
                    //Swapping from RTS to FPS or 3rd Person
                    if (isRTSCameraEnabled == true)
                    {
                        //Enabling Character Controller
                        PlayerCamera.GetComponent<CharacterController>().enabled = true;
                        PlayerCamera.GetComponent<FirstPersonController>().enabled = true;
                        //First Person Enabling
                        if (isFirstPerson == true)
                        {
                            PlayerCamera.transform.localPosition = localFirstPersonPos;
                            PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                            PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                            PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = false;
                        }
                        //Third Person Enabling
                        else
                        {
                            PlayerCamera.transform.localPosition = localThirdPersonPos;
                            PlayerCamera.GetComponent<RTSCamera>().enabled = false;
                            PlayerCamera.GetComponent<CameraCollision>().enabled = true;
                            PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = true;
                        }
                        isRTSCameraEnabled = false;
                    }
                    else
                    {
                        //Disbaling Character Controls
                        PlayerCamera.GetComponent<CharacterController>().enabled = false;
                        PlayerCamera.GetComponent<FirstPersonController>().enabled = false;
                        //Setting up RTS Mode
                        PlayerCamera.transform.localPosition = localRTSCameraPos;
                        PlayerCamera.GetComponent<RTSCamera>().enabled = true;
                        PlayerCamera.GetComponent<CameraCollision>().enabled = false;
                        PlayerCamera.GetComponent<Smooth3rdPersonFollow>().enabled = false;
                        isRTSCameraEnabled = true;
                    }
                }
            }
        }
	}
}
