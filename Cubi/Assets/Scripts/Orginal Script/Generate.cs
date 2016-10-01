using UnityEngine;
using System.Collections;

public class Generate : MonoBehaviour {

    public GameObject Cube;

	// Use this for initialization
	void Start () {
        int r = Random.Range(0, 4000000);
        GameObject stoneFloor = new GameObject();
        stoneFloor.name = "Stone Floor";
        for (float x = 0; x <= 50; x++)
        {
            for (float z = 0; z <= 50; z++)
            {
                GameObject stone = (GameObject)Instantiate(Cube, new Vector3(x, Mathf.Round(Mathf.PerlinNoise(x / 25, z / 25 )), z), new Quaternion(0, 0, 0, 0));
                stone.AddComponent<Stone>();
                stone.name = "StoneBlock [ X: " + stone.transform.position.x + " , Y: " + stone.transform.position.y + " , Z: " + stone.transform.position.z + " ]";
                stone.transform.SetParent(stoneFloor.transform);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void stoneWall()
    {
        GameObject stoneWall = new GameObject();
        stoneWall.name = "Stone Wall";
        for (int x = 0; x <= 50; x += 2)
        {
            for (int y = 0; y <= 50; y += 2)
            {
                GameObject stone = (GameObject)Instantiate(Cube, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
                stone.AddComponent<Stone>();
                stone.name = "StoneBlock [ X: " + stone.transform.position.x + " , Y: " + stone.transform.position.y + " , Z: " + stone.transform.position.z + " ]";
                stone.transform.SetParent(stoneWall.transform);
            }
        }
    }
}
