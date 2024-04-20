using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public int dirtLayerHeight = 5;
    public Sprite dirt;
    public Sprite grass;
    public Sprite stone;

    public int chunkSize = 20;
    public bool generateCaves = true;
    public float surfaceValue = 0.25f;
    public int worldSize = 100;
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float heightMultiplier = 4f;
    public int heightAddition = 25;
    public float seed;

    public Texture2D noiseTexture;
    private GameObject[] worldChunks;

    public bool isSolid = true;

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture();
        CreateChunks();
        GenerateTerrain();
    }
    public void CreateChunks()
    {
        int numChuncks = worldSize / chunkSize;
        worldChunks = new GameObject[numChuncks];
        for (int i = 0; i < numChuncks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }
    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if (y < height - dirtLayerHeight) 
                {
                    tileSprite = stone;
                }
                else if (y < height - 1)
                {
                    tileSprite = dirt;
                }
                else
                {
                    tileSprite = grass;   
                }
                if (generateCaves)
                {
                    if (noiseTexture.GetPixel(x, y).r > surfaceValue)
                    {
                        PlaceTile(tileSprite, x , y);
                    }
                }
                else 
                {
                    PlaceTile(tileSprite, x, y);
                }
            }
        }
    }
    public void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);
        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * caveFreq,(y + seed)* caveFreq);
                noiseTexture.SetPixel(x, y, new Color(v, v, v));
            }
        }
        noiseTexture.Apply();
    }

    public void PlaceTile (Sprite tileSprite, int x, int y)
    {
        GameObject newTile = new GameObject();


        int chunkCoord = Mathf.RoundToInt(x / chunkSize) * chunkSize;
        chunkCoord /= chunkSize;
        newTile.transform.parent = worldChunks[(int) chunkCoord].transform;
        
        
        newTile.AddComponent<SpriteRenderer>();

        newTile.AddComponent<BoxCollider2D>();
        newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
        newTile.tag = "Ground";
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y+0.5f);

        

    }
}
