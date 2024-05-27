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
    
    public GameObject MushroomPrefab;
    public GameObject BatPrefab;
    



    public float terrainFreq = 0.05f;
    public float caveFreq = 0.05f;
    public float seed;
    public GameObject redBlockPrefab; 
    public OreClass[] ores;




    public Texture2D caveNoiseTexture;
    private GameObject[] worldChunks;

    public bool isSolid = true;

    private void OnValidate()
    {
        caveNoiseTexture = new Texture2D(worldSize, worldSize);
        ores[0].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[1].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[2].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[3].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[4].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[5].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[6].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[7].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[8].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[9].spreadTexture = new Texture2D(worldSize, worldSize);

        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);

        //ores

        GenerateNoiseTexture(ores[0].rarity, ores[0].size, ores[0].spreadTexture);
        GenerateNoiseTexture(ores[1].rarity, ores[1].size, ores[1].spreadTexture);
        GenerateNoiseTexture(ores[2].rarity, ores[2].size, ores[2].spreadTexture);
        GenerateNoiseTexture(ores[3].rarity, ores[3].size, ores[3].spreadTexture);
        GenerateNoiseTexture(ores[4].rarity, ores[4].size, ores[4].spreadTexture);
        GenerateNoiseTexture(ores[5].rarity, ores[5].size, ores[5].spreadTexture);
        GenerateNoiseTexture(ores[6].rarity, ores[6].size, ores[6].spreadTexture);
        GenerateNoiseTexture(ores[7].rarity, ores[7].size, ores[7].spreadTexture);
        GenerateNoiseTexture(ores[8].rarity, ores[8].size, ores[8].spreadTexture);
        GenerateNoiseTexture(ores[9].rarity, ores[9].size, ores[9].spreadTexture);

    }

    private void Start()
    {

        caveNoiseTexture = new Texture2D(worldSize, worldSize);
        ores[0].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[1].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[2].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[3].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[4].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[5].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[6].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[7].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[8].spreadTexture = new Texture2D(worldSize, worldSize);
        ores[9].spreadTexture = new Texture2D(worldSize, worldSize);

        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture(caveFreq,surfaceValue,caveNoiseTexture);

        //ores
        GenerateNoiseTexture(ores[0].rarity, ores[0].size, ores[0].spreadTexture);
        GenerateNoiseTexture(ores[1].rarity, ores[1].size, ores[1].spreadTexture);
        GenerateNoiseTexture(ores[2].rarity, ores[2].size, ores[2].spreadTexture);
        GenerateNoiseTexture(ores[3].rarity, ores[3].size, ores[3].spreadTexture);
        GenerateNoiseTexture(ores[4].rarity, ores[4].size, ores[4].spreadTexture);
        GenerateNoiseTexture(ores[5].rarity, ores[5].size, ores[5].spreadTexture);
        GenerateNoiseTexture(ores[6].rarity, ores[6].size, ores[6].spreadTexture);
        GenerateNoiseTexture(ores[7].rarity, ores[7].size, ores[7].spreadTexture);
        GenerateNoiseTexture(ores[8].rarity, ores[8].size, ores[8].spreadTexture);
        GenerateNoiseTexture(ores[9].rarity, ores[9].size, ores[9].spreadTexture);
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
                    tileSprite = tileAtlas.stone.tileSprite;

                    if (ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[0].maxSpawnHeight )
                        tileSprite = tileAtlas.coal.tileSprite;
                    if (ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[1].maxSpawnHeight)
                        tileSprite = tileAtlas.iron.tileSprite;
                    if (ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[2].maxSpawnHeight)
                        tileSprite = tileAtlas.ice.tileSprite;
                    if (ores[3].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].maxSpawnHeight)
                        tileSprite = tileAtlas.alfa_crystal.tileSprite;
                    if (ores[4].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[4].maxSpawnHeight)
                        tileSprite = tileAtlas.cobalto.tileSprite;
                    if (ores[5].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[5].maxSpawnHeight)
                        tileSprite = tileAtlas.tungsten.tileSprite;
                    if (ores[6].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[6].maxSpawnHeight)
                        tileSprite = tileAtlas.uranio.tileSprite;
                    if (ores[7].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[7].maxSpawnHeight)
                        tileSprite = tileAtlas.platino.tileSprite;
                    if (ores[8].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[8].maxSpawnHeight)
                        tileSprite = tileAtlas.titanio.tileSprite;
                    if (ores[9].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[9].maxSpawnHeight)
                        tileSprite = tileAtlas.mugufin.tileSprite;



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
                    else
                    {  
                        // Aca cambiar el bloque por un enemigo de cueva
                        int random = RandomNumber();
                        if (random < 1)
                        {
                            Vector3 position = new Vector3(x, y, 0);

                            random = Random.Range(0, 2);
                            if (random == 0)
                            {
                                Instantiate(BatPrefab, position, Quaternion.identity);
                            }
                            else
                            {
                                Instantiate(MushroomPrefab, position, Quaternion.identity);
                            }
                        }
                    }
                }
                else 
                {
                    PlaceTile(tileSprite, x, y);
                }
            }
        }
    }

    private int RandomNumber()
    {
        return Random.Range(0, 100);
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
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;

        newTile.AddComponent<BoxCollider2D>();
        newTile.GetComponent<BoxCollider2D>().size = Vector2.one;

        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

        // Asigna el tag "Ore" si el sprite es el de carbón
        if (tileSprite == tileAtlas.coal.tileSprite || tileSprite == tileAtlas.iron.tileSprite || tileSprite == tileAtlas.ice.tileSprite || tileSprite == tileAtlas.alfa_crystal.tileSprite || tileSprite == tileAtlas.tungsten.tileSprite || tileSprite == tileAtlas.uranio.tileSprite || tileSprite == tileAtlas.platino.tileSprite || tileSprite == tileAtlas.titanio.tileSprite || tileSprite == tileAtlas.mugufin.tileSprite)
        {
            newTile.tag = "Ore";
        }
        else
        {
            newTile.tag = "Ground";  // O cualquier otro tag apropiado para otros tipos de tiles
        }

        newTile.name = tileSprite.name;  // Este es opcional, pero ayuda en la organización y debugging
    }
}