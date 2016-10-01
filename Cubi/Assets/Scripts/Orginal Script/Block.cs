using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public Vector3 getLocation()
    {
        Vector3 loc = gameObject.transform.position;
        return loc;
    }

    public Quaternion getRotation()
    {
        Quaternion rot = gameObject.transform.rotation;
        return rot;
    }

    public void setLocation(Vector3 location)
    {
        gameObject.transform.position.Set(location.x, location.y, location.z);
    }

    public void setRotation(Quaternion rotation)
    {
        gameObject.transform.rotation.Set(rotation.x, rotation.y, rotation.z, rotation.w);
    }

    public void setTexture(Texture2D texture)
    {
        gameObject.transform.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;

    }

    public Texture2D getTexture2D()
    {
        return (Texture2D)gameObject.transform.GetComponent<MeshRenderer>().material.mainTexture;
    }

    public Texture getTexture()
    {
        return gameObject.transform.GetComponent<MeshRenderer>().material.mainTexture;
    }

}
