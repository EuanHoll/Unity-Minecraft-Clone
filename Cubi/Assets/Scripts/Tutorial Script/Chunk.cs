using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;

public enum Blocks
{

    Grass,
    Stone,
    Dirt,
    Ore,
    Sand,
    Bedrock
}


[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{

    public static List<Chunk> chunks = new List<Chunk>();

    public static float chunkWidth
    {
        get { return World.cWorld.chunkWidth; }
    }
    public static float chunkHeight
    {
        get { return World.cWorld.chunkHeight; }
    }

    public byte[,,] map;
    public Mesh visualMesh;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;

    // Use this for initialization
    void Start()
    {
        
        chunks.Add(this);

        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        calculateWorldMap();

        StartCoroutine(createVisualMesh());

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static byte getPossibleByte(Vector3 pos)
    {
        Random.seed = World.cWorld.seed;
        Vector3 grain0Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        Vector3 grain1Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        Vector3 grain2Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);

        return calculateByte(pos, grain0Offset, grain1Offset, grain2Offset);
    }

    public static byte calculateByte(Vector3 pos, Vector3 offset1, Vector3 offset2, Vector3 offset3)
    {
        float clusterValue = calculateNoise(pos, offset2, 0.02f);
        int biomeIndex = Mathf.FloorToInt(clusterValue * World.cWorld.biomes.Length);

        Biome biome = World.cWorld.biomes[biomeIndex];

        float heightBase = biome.minHeight;
        float maxHeight = biome.maxHeight;
        float heightSwing = maxHeight - heightBase;

        float blobValue = calculateNoise(pos, offset2, 0.05f);
        float mountainValue = calculateNoise(pos, offset1, 0.009f);

        mountainValue += biome.mountainPowerBonus;
        //if (mountainValue < 0) mountainValue = 0;

        mountainValue = Mathf.Sqrt(mountainValue);

        byte block = biome.getBlock(Mathf.FloorToInt(pos.y), mountainValue, blobValue);

        mountainValue *= heightSwing;
        mountainValue += heightBase;

        mountainValue += (blobValue * 10) - 5f;


        if (mountainValue >= pos.y)
            return block;
        return 0;
    }
    
    public virtual void calculateWorldMap()
    {
        map = new byte[(byte)chunkWidth, (byte)chunkHeight, (byte)chunkWidth];

        Random.seed = World.cWorld.seed;
        Vector3 grain0Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        Vector3 grain1Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        Vector3 grain2Offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);



        for (int x = 0; x < World.cWorld.chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                for (int z = 0; z < chunkWidth; z++)
                {
                    if (y == 0)
                    {
                        map[x, y, z] = 5;
                    }
                    else
                    {
                        map[x, y, z] = calculateByte(new Vector3(x, y, z) + transform.position, grain0Offset, grain1Offset, grain2Offset);
                    }
                }
            }
        }
    }

    public static float calculateNoise(Vector3 pos, Vector3 offset, float scale)
    {
        float noiseX = Mathf.Abs((pos.x + offset.x) * scale);
        float noiseY = Mathf.Abs((pos.y + offset.y) * scale);
        float noiseZ = Mathf.Abs((pos.z + offset.z) * scale);

        return Mathf.Max(0, Noise.Generate(noiseX, noiseY, noiseZ));
    }

    public virtual IEnumerator createVisualMesh()
    {
        visualMesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (byte x = 0; x < chunkWidth; x++)
        {
            for (byte y = 0; y < chunkHeight; y++)
            {
                for (byte z = 0; z < chunkWidth; z++)
                {
                    if (map[x, y, z] == 0) continue;

                    byte block = map[x, y, z];
                    //Left Right
                    if (isTransparent(x - 1, y, z))
                        buildFace(block, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                    if (isTransparent(x + 1, y, z))
                        buildFace(block, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
                    //Bottom Top
                    if (isTransparent(x, y - 1, z))
                        buildFace(block, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
                    if (isTransparent(x, y + 1, z))
                        buildFace(block, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);
                    //Back Forward
                    if (isTransparent(x, y, z - 1))
                        buildFace(block, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
                    if (isTransparent(x, y, z + 1))
                        buildFace(block, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
                }
            }
        }

        visualMesh.vertices = verts.ToArray();
        visualMesh.uv = uvs.ToArray();
        visualMesh.triangles = tris.ToArray();
        visualMesh.RecalculateBounds();
        visualMesh.RecalculateNormals();

        meshFilter.mesh = visualMesh;
        meshCollider.sharedMesh = visualMesh;

        yield return 0;
    }

    public virtual void buildFace(byte block, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        Vector2 uvCorner = getTexturePos(block);

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        Vector2 uvWidth = new Vector2(0.25f, 0.25f);

        uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));

        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        }
        else
        {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }
    }

    public virtual bool isTransparent(int x, int y, int z)
    {
        if (y < 0) return false;

        byte block = getByte(x, y, z);

        switch (block)
        {
            case 0:
                return true;
            default:
                return false;
        }
    }

    public virtual byte getByte(int x, int y, int z)
    {
        if ((y < 0) || (y >= chunkHeight))
        {
            return 0;
        }

        if ((x < 0) || (y < 0) || (z < 0) || (y >= chunkHeight) || x >= chunkWidth || z >= chunkWidth)
        {

            Vector3 worldPos = new Vector3(x, y, z) + transform.position;
            Chunk chunk = findChunk(worldPos);
            if (chunk == this) { return 0; }
            if (chunk == null) { return getPossibleByte(worldPos); }

            return chunk.getByte(worldPos);
        }
        return map[x, y, z];
    }

    public virtual byte getByte(Vector3 worldPos)
    {
        worldPos -= transform.position;
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);
        int z = Mathf.FloorToInt(worldPos.z);
        return getByte(x, y, z);
    }

    public static Chunk findChunk(Vector3 pos)
    {
        for (int a = 0; a < chunks.Count; a++)
        {
            Vector3 cpos = chunks[a].transform.position;

            if ((pos.x < cpos.x) || (pos.z < cpos.z) || (pos.x >= cpos.x + chunkWidth) || (pos.z >= cpos.z + chunkWidth)) continue;
            return chunks[a];

        }
        return null;
    }

    public bool setBlock(byte block, Vector3 worldPos)
    {
        worldPos -= transform.position;
        return setBlock(block, Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z));
    }

    public bool setBlock(byte block, int x, int y, int z)
    {
        if ((x < 0) || (y < 0) || (z < 0) || (x >= chunkWidth) || (y >= chunkHeight) || (z >= chunkWidth))
        {
            return false;
        }

        if (map[x, y, z] == block) return false;
        map[x, y, z] = block;
        StartCoroutine(createVisualMesh());

        if (x == 0)
        {
            Chunk chunk = findChunk(new Vector3(x - 2, y, z) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.createVisualMesh());
        }
        if (x == chunkWidth - 1)
        {
            Chunk chunk = findChunk(new Vector3(x + 2, y, z) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.createVisualMesh());
        }
        if (z == 0)
        {
            Chunk chunk = findChunk(new Vector3(x, y, z - 2) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.createVisualMesh());
        }
        if (z == chunkWidth - 1)
        {
            Chunk chunk = findChunk(new Vector3(x, y, z + 2) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.createVisualMesh());
        }

        return true;
    }

    public Vector2 getTexturePos(byte block)
    {
        byte b1 = block;
        float lenght = 0.00f;
        float height = 0.75f;

        while(b1 > 1)
        {
            if(b1 < 5)
            {
                lenght += 0.25f;
                b1 -= 1;
            }
            else
            {
                height -= 0.25f;
                b1 -=4;
            }
        }
        return new Vector2(lenght, height);
    }
}
