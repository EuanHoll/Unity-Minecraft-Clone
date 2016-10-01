using UnityEngine;
using System.Collections;

public class PlayerIO : MonoBehaviour {

    public static PlayerIO cPlayerIO;
    public Camera camera;
    public float maxHit;
    public byte cInventory = 0;

	// Use this for initialization
	void Start () {
        cPlayerIO = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxHit))
        {
            Chunk chunk = hit.transform.GetComponent<Chunk>();
            if (chunk == null)
            {
                Debug.Log("Clicked on " + hit.transform.name + " and it's not a chunk.");

                return;
            }

            Debug.Log("Clicked on the chunk at " + chunk.transform.position);
            Vector3 pos = hit.point;
            if (cInventory == 0)
            {
                pos -= hit.normal / 4;
                Debug.Log(pos + " : " + chunk.getByte(pos));
                Debug.Log(pos + " : " + (Blocks)chunk.getByte(pos));
                if (chunk.getByte(pos) != 5)
                {
                    cInventory = chunk.getByte(pos);
                    chunk.setBlock(0, pos);
                }
            }
            else
            {
                pos += hit.normal / 4;
                chunk.setBlock(cInventory, pos);
                cInventory = 0;
            }


        }
        else
        {
            Debug.Log("Clicked, but nothing was there!");
        }
	}
}
