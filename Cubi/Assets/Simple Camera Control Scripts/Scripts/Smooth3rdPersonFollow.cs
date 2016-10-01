using UnityEngine;
using System.Collections;

public class Smooth3rdPersonFollow : MonoBehaviour {

    Vector2 abMouse;
    Vector2 mouseSmooth;

    //X Y 3rd Person Clamps
    public Vector2 degreesClamp = new Vector2(360, 50);

    //X Y Movement Sensitivity
    public Vector2 sensitivity = new Vector2(2, 2);

    //X Y Movement Smoothing
    public Vector2 smoothing = new Vector2(3, 3);

    public Vector2 targetDir;
    public Vector2 targetCharDir;

    //Recalls Start Method
    public void Restart()
    {
        Start();
    }

    void Start()
    {

        // Set target direction to the camera's initial orientation.
        targetDir = transform.localRotation.eulerAngles;


    }

    void Update()
    {

        // Allow the script to clamp based on a desired target value.
        Quaternion targetFacing = Quaternion.Euler(targetDir);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        md = Vector2.Scale(md, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        mouseSmooth.x = Mathf.Lerp(mouseSmooth.x, md.x, 1f / smoothing.x);
        mouseSmooth.y = Mathf.Lerp(mouseSmooth.y, md.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero.
        abMouse += mouseSmooth;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (degreesClamp.x < 360)
        {
            abMouse.x = Mathf.Clamp(abMouse.x, -degreesClamp.x * 0.5f, degreesClamp.x * 0.5f);
        }

        Quaternion xRot = Quaternion.AngleAxis(-abMouse.y, targetFacing * Vector3.right);
        transform.localRotation = xRot;

        // Then clamp and apply the global y value.
        if (degreesClamp.y < 360)
        {
            abMouse.y = Mathf.Clamp(abMouse.y, -degreesClamp.y * 0.5f, degreesClamp.y * 0.5f);
        }

        transform.localRotation *= targetFacing;

        
    }

}