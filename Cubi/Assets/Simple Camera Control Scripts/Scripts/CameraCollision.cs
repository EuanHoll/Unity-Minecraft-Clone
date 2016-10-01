using UnityEngine;
using System.Collections;

public class CameraCollision : MonoBehaviour
{
    //Minimum Distance from Player
    public float minDistance = 1.0f;

    //Maximum Distance from Player
    float maxDistance;

    //Smoothness of Camera collision
    public float smooth = 5.0f;

    //Extra Hight to avoid collision with ground (if unsure leave as "2")
    public float addOnHeight = 2;
    Vector3 dollyDir;

    //Current Distance
    float distance;

    //Location of player with Addon Height
    Vector3 posAddon;

    void Start()
    {
        //Set Camera Switcher variable
        CameraSwitcher cs = gameObject.transform.parent.GetComponent<CameraSwitcher>();
        //Gets Character Collision Position(the point at which the camera is detecting any objects inbetween) 
        Vector3 pos1 = new Vector3(transform.parent.GetComponent<Renderer>().bounds.center.x,
            transform.parent.GetComponent<Renderer>().bounds.center.y + addOnHeight,
            transform.parent.GetComponent<Renderer>().bounds.center.z);
        //Sets Max Distance to the distance between the Third Person Camera Position and the Character.
        maxDistance = Vector3.Distance(pos1, new Vector3(pos1.x + cs.localThirdPersonPos.x, 
                                                         pos1.y + cs.localThirdPersonPos.y,
                                                         pos1.z + cs.localThirdPersonPos.z));
    }

    void Awake()
    {
        dollyDir = transform.localPosition.normalized;

        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        //Setting posAddon
        posAddon = new Vector3(transform.parent.GetComponent<Renderer>().bounds.center.x,
            transform.parent.GetComponent<Renderer>().bounds.center.y + addOnHeight,
            transform.parent.GetComponent<Renderer>().bounds.center.z);
        //Getting Player's Rotation
        Quaternion q = transform.rotation;
        //Getting the desired Camera Position
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);

        RaycastHit hit;

        //Drawing Linecast from the desiredCamera Position to the centre of the object with the addon height
        if (Physics.Linecast(posAddon, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        //Setting Camera Position
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);

    }

}