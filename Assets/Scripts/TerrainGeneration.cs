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
    public GameObject TentaclePrefab;
    // public GameObject ButterflyPrefab;

    public GameObject BenchPrefab;
    public GameObject SpaceShipPrefab;
    private bool spaceShipPlaced = false;




    public float terrainFreq = 0.05f;
    public float caveFreq = 0.05f;
    public float seed;
    public GameObject redBlockPrefab;
    public OreClass[] ores;

    public Texture2D caveNoiseTexture;
    private GameObject[] worldChunks;

    public bool isSolid = true;
    private bool[] benchPlacedInLevel = new bool[3];
    private List<Vector2Int> potentialBenchPositions = new List<Vector2Int>();
    public int minBenchesPerLevel = 3; // Número mínimo de benches por nivel
    public int maxBenchesPerLevel = 4;
    public float minBenchDistance = 20f;
    private List<Vector2Int> placedBenches = new List<Vector2Int>();


    private void OnValidate()
    {
        caveNoiseTexture = new Texture2D(worldSize, worldSize);

        // Inicializar las texturas de spread para todos los ores
        for (int i = 0; i < ores.Length; i++)
        {
            ores[i].spreadTexture = new Texture2D(worldSize, worldSize);
        }

        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);

        // Generar texturas de ruido para todos los ores
        for (int i = 0; i < ores.Length; i++)
        {
            GenerateNoiseTexture(ores[i].rarity, ores[i].size, ores[i].spreadTexture);
        }
    }

    private void Start()
    {
        caveNoiseTexture = new Texture2D(worldSize, worldSize);

        // Inicializar las texturas de spread para todos los ores
        for (int i = 0; i < ores.Length; i++)
        {
            ores[i].spreadTexture = new Texture2D(worldSize, worldSize);
        }

        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture(caveFreq, surfaceValue, caveNoiseTexture);

        // Generar texturas de ruido para todos los ores
        for (int i = 0; i < ores.Length; i++)
        {
            GenerateNoiseTexture(ores[i].rarity, ores[i].size, ores[i].spreadTexture);
        }

        CreateChunks();
        GenerateTerrain();
        Time.timeScale = 1;
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
        potentialBenchPositions.Clear();
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;

                // Condición para las posiciones laterales y la fila inferior del terreno
                if (x == 0 || x == worldSize - 1 || y == 0)
                {
                    tileSprite = tileAtlas.bedrock.tileSprite;
                }
                else if (y < height - dirtLayerHeight)
                {
                    tileSprite = tileAtlas.stone.tileSprite;

                    if (height - y > ores[1].profundidadMaxima)
                    {
                        tileSprite = tileAtlas.stone2.tileSprite;
                        benchPlacedInLevel[1] = false; // Restablecer el indicador de colocación para este nivel
                    }
                    if (height - y > ores[4].profundidadMaxima)
                    {
                        tileSprite = tileAtlas.stone3.tileSprite;
                        benchPlacedInLevel[2] = false; // Restablecer el indicador de colocación para este nivel
                    }

                    if (ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[0].profundidadMinima && height - y < ores[0].profundidadMaxima))
                        tileSprite = tileAtlas.coal.tileSprite;
                    if (ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[1].profundidadMinima && height - y < ores[1].profundidadMaxima))
                        tileSprite = tileAtlas.iron.tileSprite;
                    if (ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[2].profundidadMinima && height - y < ores[2].profundidadMaxima))
                        tileSprite = tileAtlas.ice.tileSprite;
                    if (ores[3].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].profundidadMinima)
                        tileSprite = tileAtlas.alfa_crystal.tileSprite;
                    if (ores[4].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[4].profundidadMinima && height - y < ores[4].profundidadMaxima))
                        tileSprite = tileAtlas.cobalto.tileSprite;
                    if (ores[5].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[5].profundidadMinima && height - y < ores[5].profundidadMaxima))
                        tileSprite = tileAtlas.tungsten.tileSprite;
                    if (ores[6].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[6].profundidadMinima && height - y < ores[6].profundidadMaxima))
                        tileSprite = tileAtlas.uranio.tileSprite;
                    if (ores[7].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[7].profundidadMinima && height - y < ores[7].profundidadMaxima))
                        tileSprite = tileAtlas.platino.tileSprite;
                    if (ores[8].spreadTexture.GetPixel(x, y).r > 0.5f && (height - y > ores[8].profundidadMinima && height - y < ores[8].profundidadMaxima))
                        tileSprite = tileAtlas.titanio.tileSprite;
                    if (ores[9].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[9].profundidadMinima)
                        tileSprite = tileAtlas.mugufin.tileSprite;
                }
                else if (y < height - 1)
                {
                    tileSprite = tileAtlas.dirt.tileSprite;
                }
                else
                {
                    tileSprite = tileAtlas.grass.tileSprite;
                    if (y == Mathf.FloorToInt(height) - 1)
                    {
                        tileSprite = tileAtlas.grass.tileSprite;

                        // Intentar colocar la nave espacial flotando sobre la superficie
                        if (!spaceShipPlaced && Random.value < 0.01f) // 1% de probabilidad por cada bloque de superficie
                        {
                            float floatHeight = Random.Range(3f, 5f); // La nave flotará entre 3 y 5 unidades sobre el suelo
                            Vector3 spaceShipPosition = new Vector3(x, y + 1 + floatHeight, 0);
                            Instantiate(SpaceShipPrefab, spaceShipPosition, Quaternion.identity);
                            spaceShipPlaced = true;
                        }
                    }

                }

                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                    {
                        PlaceTile(tileSprite, x, y);
                    }
                    else
                    {
                        if (x == 0 || x == worldSize - 1 || y == 0)
                        {
                            PlaceTile(tileAtlas.bedrock.tileSprite, x, y);
                        }
                        else
                        {
                            // Almacenar posiciones potenciales para BenchPrefabs
                            if (IsSuitableForBench(x, y))
                            {
                                potentialBenchPositions.Add(new Vector2Int(x, y));
                            }

                            // Generar otros enemigos de cueva como antes
                            int random = RandomNumber();
                            if (random < 1)
                            {
                                Vector3 position = new Vector3(x, y, 0);
                                random = Random.Range(0, 3);
                                if (random == 0)
                                    Instantiate(BatPrefab, position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                                else if (random == 1)
                                    Instantiate(MushroomPrefab, position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                                else
                                    Instantiate(TentaclePrefab, position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
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
        if (!spaceShipPlaced)
        {
            int randomX = Random.Range(0, worldSize);
            float surfaceHeight = Mathf.PerlinNoise((randomX + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;
            float floatHeight = Random.Range(3f, 5f); // La nave flotará entre 3 y 5 unidades sobre el suelo
            Vector3 spaceShipPosition = new Vector3(randomX, Mathf.FloorToInt(surfaceHeight) + floatHeight, 0);
            Instantiate(SpaceShipPrefab, spaceShipPosition, Quaternion.identity);
        }

        Debug.Log($"Posiciones potenciales para benches: {potentialBenchPositions.Count}");
        PlaceBenchPrefabs();
    }
    private bool IsSuitableForBench(int x, int y)
    {
        // Verificar si la posición es adecuada para un bench (en el suelo de una cueva)
        bool suitable = caveNoiseTexture.GetPixel(x, y).r <= 0.5f &&
                        caveNoiseTexture.GetPixel(x, y - 1).r > 0.5f;

        if (suitable)
        {
            Debug.Log($"Posición adecuada para bench encontrada en ({x}, {y})");
        }

        return suitable;
    }
    private void PlaceBenchPrefabs()
    {
        Debug.Log("Iniciando colocación de benches");
        placedBenches.Clear();

        // Colocar benches en cada nivel
        for (int level = 0; level < 3; level++)
        {
            int benchesToPlace = Random.Range(minBenchesPerLevel, maxBenchesPerLevel + 1);
            PlaceBenchesInLevel(level, benchesToPlace);
        }

        Debug.Log($"Total de benches colocados: {placedBenches.Count}");
    }
    private void PlaceBenchInLevel(int level)
    {
        int minY = level == 0 ? 0 : (level == 1 ? ores[1].profundidadMinima : ores[4].profundidadMinima);
        int maxY = level == 0 ? ores[1].profundidadMaxima : (level == 1 ? ores[4].profundidadMaxima : worldSize);

        List<Vector2Int> levelPositions = potentialBenchPositions.FindAll(pos => pos.y >= minY && pos.y < maxY);

        Debug.Log($"Intentando colocar bench en nivel {level}. Posiciones disponibles: {levelPositions.Count}");

        if (levelPositions.Count > 0)
        {
            Vector2Int benchPos = levelPositions[Random.Range(0, levelPositions.Count)];
            Vector3 position = new Vector3(benchPos.x, benchPos.y + 1, 0);
            Instantiate(BenchPrefab, position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
            potentialBenchPositions.Remove(benchPos);
            Debug.Log($"Bench colocado en nivel {level} en posición {position}");
        }
        else
        {
            Debug.Log($"No se pudo colocar bench en nivel {level}. No hay posiciones disponibles.");
        }
    }
    private void PlaceBenchesInLevel(int level, int count)
    {
        int minY = level == 0 ? 0 : (level == 1 ? ores[1].profundidadMinima : ores[4].profundidadMinima);
        int maxY = level == 0 ? ores[1].profundidadMaxima : (level == 1 ? ores[4].profundidadMaxima : worldSize);

        List<Vector2Int> levelPositions = potentialBenchPositions.FindAll(pos => pos.y >= minY && pos.y < maxY);

        Debug.Log($"Intentando colocar {count} benches en nivel {level}. Posiciones disponibles: {levelPositions.Count}");

        int benchesPlaced = 0;
        int attempts = 0;
        int maxAttempts = levelPositions.Count * 2; // Limitamos los intentos para evitar bucles infinitos

        while (benchesPlaced < count && attempts < maxAttempts && levelPositions.Count > 0)
        {
            Vector2Int benchPos = levelPositions[Random.Range(0, levelPositions.Count)];

            if (IsFarEnoughFromOtherBenches(benchPos))
            {
                Vector3 position = new Vector3(benchPos.x, benchPos.y + 1, 0);
                Instantiate(BenchPrefab, position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                placedBenches.Add(benchPos);
                potentialBenchPositions.Remove(benchPos);
                levelPositions.Remove(benchPos);
                benchesPlaced++;
                Debug.Log($"Bench colocado en nivel {level} en posición {position}");
            }

            attempts++;
        }

        Debug.Log($"Se colocaron {benchesPlaced} benches en el nivel {level} después de {attempts} intentos");
    }
    private bool IsFarEnoughFromOtherBenches(Vector2Int newPos)
    {
        foreach (Vector2Int placedBench in placedBenches)
        {
            if (Vector2Int.Distance(newPos, placedBench) < minBenchDistance)
            {
                return false;
            }
        }
        return true;
    }


    private int RandomNumber()
    {
        return Random.Range(0, 100);
    }

    public void GenerateNoiseTexture(float frequency, float limit, Texture2D noiseTexture)
    {
        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);
                if (v > limit)
                    noiseTexture.SetPixel(x, y, Color.white);
                else
                    noiseTexture.SetPixel(x, y, Color.black);
            }
        }
        noiseTexture.Apply();
    }

    public void PlaceTile(Sprite tileSprite, int x, int y)
    {
        GameObject newTile = new GameObject();

        int chunkCoord = Mathf.RoundToInt(x / chunkSize) * chunkSize;
        chunkCoord /= chunkSize;
        newTile.transform.parent = worldChunks[(int)chunkCoord].transform;

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;

        newTile.AddComponent<BoxCollider2D>();
        newTile.GetComponent<BoxCollider2D>().size = Vector2.one;

        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

        // Asigna el tag "Ore" si el sprite es el de carbón
        if (tileSprite == tileAtlas.coal.tileSprite || tileSprite == tileAtlas.iron.tileSprite)
        {
            newTile.tag = "Ore basic";
        }
        else if (tileSprite == tileAtlas.ice.tileSprite || tileSprite == tileAtlas.tungsten.tileSprite || tileSprite == tileAtlas.cobalto.tileSprite)
        {
            newTile.tag = "Ore medium";
        }
        else if (tileSprite == tileAtlas.alfa_crystal.tileSprite || tileSprite == tileAtlas.uranio.tileSprite || tileSprite == tileAtlas.platino.tileSprite || tileSprite == tileAtlas.titanio.tileSprite || tileSprite == tileAtlas.mugufin.tileSprite)
        {
            newTile.tag = "Ore rare";
        }
        else if (tileSprite == tileAtlas.bedrock.tileSprite)
        {
            newTile.tag = "Bedrock";
        }
        else
        {
            newTile.tag = "Ground";  // O cualquier otro tag apropiado para otros tipos de tiles
        }

        newTile.name = tileSprite.name;  // Este es opcional, pero ayuda en la organización y debugging
    }
}
