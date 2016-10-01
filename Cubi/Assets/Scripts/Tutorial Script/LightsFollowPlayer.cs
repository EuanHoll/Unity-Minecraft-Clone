using UnityEngine;
using System.Collections;

public class LightsFollowPlayer : MonoBehaviour {

    public float yPos = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(GameObject.Find("Player Capsule").gameObject.transform.position.x
            , yPos,
            GameObject.Find("Player Capsule").gameObject.transform.position.z);
	}
}
