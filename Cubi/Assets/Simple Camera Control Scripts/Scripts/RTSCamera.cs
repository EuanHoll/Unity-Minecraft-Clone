using UnityEngine;
using System.Collections.Generic;

public class RTSCamera : MonoBehaviour {

    //Private Vars
    //Is the Rotation Key Down
    bool isRotating = false;

    //Camera's Starting Position and Rotation
    Vector3 mouseStartPos;
    Vector3 mouseStartRot;

    //Camera's Transform
    Transform trans;

    //Camera's Rotation
    float verticalRotation = 0;
    float horizontalRotation = 0;
    //Invert Int
    int invert = -1;

    //public Vars

    //Using KeyCode?
    public bool useKeyCode = true;

    //KeyCode
    public KeyCode KeyCode;

    //Input Code
    public string inputCode;

    //Is Rotation inverted?
    public bool isRotationInverted;

    //Sensitivities
    public float rotSensitivity = 3f;
    public float transSensitivity = 1f;
    public float zoomSensitivity = 20;

    //Setting Max Height
    public float maxHeight = 280;
    //Set Up Down Range
    public float upDownRange = 59.0f;

    //Called on Start
    void start()
    {
        //Setting Transform
        trans = gameObject.transform.GetChild(0).transform;

        //Checking for Inverted Rotation
        if(isRotationInverted == true)
        {
            invert = 1;
        }
        else
        {
            invert = -1;
        }
    }

	// Update is called once per frame
	void Update () {
        //Checking for Inverted Rotation
        if (isRotationInverted == true)
        {
            invert = 1;
        }
        else
        {
            invert = -1;
        }
        //Using KeyCode
        if (useKeyCode == true)
        {
            if (Input.GetKey(KeyCode))
            {
                //Sets Horizontal Rotation
                horizontalRotation -= invert * Input.GetAxis("Mouse X") * rotSensitivity;
                //Set Vertical Rotation
                verticalRotation -= Input.GetAxis("Mouse Y") * rotSensitivity;
                verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
                //Sets Transform's Rotation
                transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
            }
            else
            {
                isRotating = false;
            }
        }
        else
        {
            if (Input.GetButton(inputCode))
            {
                //Sets Horizontal Rotation
                horizontalRotation -= invert * Input.GetAxis("Mouse X") * rotSensitivity;
                //Sets Vertical Rotation
                verticalRotation -= Input.GetAxis("Mouse Y") * rotSensitivity;
                verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
                //Sets Transform's Rotation
                transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
            }
            else
            {
                isRotating = false;
            }
        }

        //Checking if Colliding with active Terrain
        float curTerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        if (transform.position.y > (curTerrainHeight))
        {
            transform.Translate(Input.GetAxis("Horizontal"), transSensitivity * Input.GetAxis("Vertical") * Mathf.Sin(transform.rotation.eulerAngles.x * (3.14f / 180)), transSensitivity * Input.GetAxis("Vertical") * Mathf.Cos(transform.rotation.eulerAngles.x * (3.14f / 180)) + (Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity), Space.Self);
        }
        else
        {
            transform.Translate(Input.GetAxis("Horizontal"), curTerrainHeight, transSensitivity * Input.GetAxis("Vertical") * Mathf.Cos(transform.rotation.eulerAngles.x * (3.14f / 180)) + (Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity), Space.Self);
        }

        //Checking if Under Max Height
        if (gameObject.transform.position.y > maxHeight)
        {  
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, maxHeight, gameObject.transform.position.z);
        }

	}
}
