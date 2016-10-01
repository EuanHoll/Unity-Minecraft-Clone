using UnityEngine;
using System.Collections;

public class Stone : Block{

    TextureList tl;
    Texture2D stone;

	// Use this for initialization
	void Start () {
        tl = GameObject.Find("TextureList").GetComponent<TextureList>();
        setTexture(tl.Stone2D);
        stone = tl.Stone2D;
	}
	
	// Update is called once per frame
	void Update () {
    }
}
