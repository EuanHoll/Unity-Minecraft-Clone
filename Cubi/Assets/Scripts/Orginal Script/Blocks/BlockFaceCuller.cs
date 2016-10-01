using UnityEngine;
using System.Collections;

public class BlockFaceCuller : MonoBehaviour {
    Texture2D stone;

	// Use this for initialization
	void Start () {
        TextureList tl = GameObject.Find("TextureList").GetComponent<TextureList>();
        stone = tl.Stone2D;
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnCollisionEnter(Collision co)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void OnCollisionExit(Collision co)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
