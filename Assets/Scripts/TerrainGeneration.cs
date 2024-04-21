using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    
    public TileAtlas tileAtlas;
    
    public int dirtLayerHeight = 5;
    public int chunkSize = 20;
    public bool generateCaves = true;
    public float surfaceValue = 0.25f;
    public int worldSize = 100;
    public float heightMultiplier = 4f;
    public int heightAddition = 25;
    

    public float terrainFreq = 0.05f;
    public float caveFreq = 0.05f;
    public float seed;

    public float coalRarity;
    public float coalSize;
    public float ironRarity;
    public float ironSize;
    public float iceRarity;
    public float iceSize;
    public float uraniumRarity;
    public float uraniumSize;
    public float alfacrystalRarity;
    public float alfacrystalSize;
    public float tungstenRarity;
    public float tungstenSize;
    public Texture2D ironSpread;
    public Texture2D coalSpread;
    public Texture2D iceSpread;
    public Texture2D uraniumSpread;
    public Texture2D alfacrystalSpread;
    public Texture2D tungstenSpread;




    public Texture2D caveNoiseTexture;
    private GameObject[] worldChunks;

    public bool isSolid = true;

    private void OnValidate()
    {
        if (caveNoiseTexture == null)
        {
            caveNoiseTexture = new Texture2D(worldSize, worldSize);
            ironSpread = new Texture2D(worldSize, worldSize);
            iceSpread = new Texture2D(worldSize, worldSize);
            uraniumSpread = new Texture2D(worldSize, worldSize);
            alfacrystalSpread = new Texture2D(worldSize, worldSize);
            tungstenSpread = new Texture2D(worldSize, worldSize);
            coalSpread = new Texture2D(worldSize, worldSize);
        }
            GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);

            //ores
            GenerateNoiseTexture(ironRarity, ironSize, ironSpread);
            GenerateNoiseTexture(iceRarity, iceSize, iceSpread);
            GenerateNoiseTexture(uraniumRarity, uraniumSize, uraniumSpread);
            GenerateNoiseTexture(alfacrystalRarity, alfacrystalSize, alfacrystalSpread);
            GenerateNoiseTexture(tungstenRarity, tungstenSize, tungstenSpread);
            GenerateNoiseTexture(coalRarity, coalSize, coalSpread);
        
    }

    private void Start()
    {
        if (caveNoiseTexture == null)
        {
            caveNoiseTexture = new Texture2D(worldSize, worldSize);
            ironSpread = new Texture2D(worldSize, worldSize);
            iceSpread = new Texture2D(worldSize, worldSize);
            uraniumSpread = new Texture2D(worldSize, worldSize);
            alfacrystalSpread = new Texture2D(worldSize, worldSize);
            tungstenSpread = new Texture2D(worldSize, worldSize);
            coalSpread = new Texture2D(worldSize, worldSize);
        }

        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture(caveFreq,surfaceValue,caveNoiseTexture);

        //ores
        GenerateNoiseTexture(ironRarity,ironSize, ironSpread);
        GenerateNoiseTexture(iceRarity,iceSize, iceSpread);
        GenerateNoiseTexture(uraniumRarity,uraniumSize, uraniumSpread);
        GenerateNoiseTexture(alfacrystalRarity,alfacrystalSize, alfacrystalSpread);
        GenerateNoiseTexture(tungstenRarity,tungstenSize, tungstenSpread);
        GenerateNoiseTexture(coalRarity,coalSize, coalSpread);
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
                    if (coalSpread.GetPixel(x, y).r > 0.5f)
                        tileSprite = tileAtlas.coal.tileSprite;






                    else
                        tileSprite = tileAtlas.stone.tileSprite;
                }
                else if (y < height - 1)
                {
                    tileSprite = tileAtlas.dirt.tileSprite;
                }
                else
                {
                    tileSprite = tileAtlas.grass.tileSprite;   
                }
                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
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
    public void GenerateNoiseTexture(float frequency,float limit,Texture2D noiseTexture )
    {
        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed)* frequency);
                if (v > limit)
                noiseTexture.SetPixel(x, y, Color.white);
                else
                    noiseTexture.SetPixel(x, y, Color.black);
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
