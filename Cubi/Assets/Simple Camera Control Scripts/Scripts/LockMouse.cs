using UnityEngine;
using System.Collections;

public class LockMouse : MonoBehaviour {

    //Cursor Bool Lock
    public bool cursorShouldBeLocked = false;

    //If true uses keycode to hide cursor, if false uses input
    public bool useCursorHideKeycode = true;
    //Cursor Hide Input Button/KeyCode 
    public KeyCode cursorHideKeycode;
    public string cursorHideInput;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Check Menu Button Down
        EscTest();

        //Set Cursor Visibility
        if (cursorShouldBeLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


    //Escape Tester
    void EscTest()
    {
        if (useCursorHideKeycode)
        {
            //Uses Keycode cursorHideKeycode To Check if cursor should be locked.
            if (Input.GetKeyDown(cursorHideKeycode))
            {
                cursorShouldBeLocked = !cursorShouldBeLocked;
            }
        }
        else {
            //Uses Input "Menu" To Check if cursor should be locked.
            if (Input.GetButtonDown(cursorHideInput))
            {
                cursorShouldBeLocked = !cursorShouldBeLocked;
            }
        }
    }
}
