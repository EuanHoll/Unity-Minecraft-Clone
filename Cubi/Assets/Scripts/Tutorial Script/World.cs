using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {

    public Biome[] biomes;

    public static World cWorld;

    public int chunkWidth = 20, chunkHeight = 20, seed = 0;
    public float viewRange = 30;

    public Chunk chunkPrefab;
    public GameObject player;
    public GameObject worldMap;

	// Use this for initialization
	void Awake () {
        cWorld = this;
	    if(seed == 0)
        {
            seed = Random.Range(0, int.MaxValue);
        }
        worldMap = new GameObject();
        worldMap.name = "WorldMap";
	}
	
	// Update is called once per frame
	void Update () {
        for (float x = player.transform.position.x - viewRange; x < player.transform.position.x + viewRange; x += chunkWidth)
        {
            for (float z = player.transform.position.z - viewRange; z < player.transform.position.z + viewRange; z += chunkWidth)
            {

                Vector3 pos = new Vector3(x, 0, z);
                pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;

                Chunk chunk = Chunk.findChunk(pos);
                if (chunk != null) continue;

                chunk = (Chunk)Instantiate(chunkPrefab, pos, Quaternion.identity);
                chunk.transform.SetParent(worldMap.transform);


            }
        }
    }
}
