using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct Biome
{
    public string Name;
    public Tile Tile;

    public float minTemp;
    public float maxTemp;
    public float minHum;
    public float maxHum;
}

[System.Serializable]

public struct MapData
{
    public string name;
    public float[,] data;

    public float scale;
    [Range(1, 16)]
    public int octaves;
    [Range(0f, 1f)]
    public float persistance;
    [Range(0f, 4f)]
    public float lacunarity;
}

public class WorldGen : MonoBehaviour
{
    public int mapSize;
    public int seed;
    public float circleSizePercentageOfMapSize;

    public MapData[] mapData;
    public Biome[] biomes;

    public Tilemap tilemap;

    private void Start()
    {
        GenrateMap();
    }

    public void GenrateMap()
    {
        mapData[0].data = GenrateNoiseMap(mapSize, seed, mapData[0].scale, mapData[0].octaves, mapData[0].persistance, mapData[0].lacunarity);
        mapData[1].data = GenrateNoiseMap(mapSize, seed, mapData[1].scale, mapData[1].octaves, mapData[1].persistance, mapData[1].lacunarity);

        int halfSize = (mapSize / 2);

       for(int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float dx = x - halfSize;
                float dy = y - halfSize;

                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                float maxDistance = Mathf.Sqrt(2f) * halfSize * 0.7f;

                float circleValue = Mathf.Clamp01(distance / maxDistance);
                
                float temp = mapData[0].data[x, y];
                float hum = Mathf.Clamp01(mapData[1].data[x, y] + circleValue);

                foreach(Biome biome in biomes)
                {
                    if (temp >= biome.minTemp && temp <= biome.maxTemp && hum >= biome.minHum && hum <= biome.maxHum)
                    {
                        tilemap.SetTile(new Vector3Int(x - halfSize, y - halfSize, 0), biome.Tile);
                        break;
                    }
                }
            }
        }
    }

    public static float[,] GenrateNoiseMap(int mapSize, int seed, float scale, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[mapSize, mapSize];
        
        System.Random prng = new System.Random();
        Vector2[] octaveOffset = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for(int i = 0; i < octaves; i++)
        {
            float offSetX = prng.Next(-100000, 100000);
            float offSetY = prng.Next(-100000, 100000);
            octaveOffset[i] = new Vector2(offSetX, offSetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (scale <= 0)
            scale = 0.0001f;
        float halfSize = mapSize / 2f;

        for(int y = 0; y < mapSize; y++)
        {
            for(int x = 0; x < mapSize; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for(int i = 0;i < octaves; i++)
                {
                    float sampleX = (x - halfSize + octaveOffset[i].x) / scale * frequency;
                    float sampleY = (y - halfSize + octaveOffset[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                float normalizedHeight = (noiseHeight + maxPossibleHeight) / (2f * maxPossibleHeight);

                noiseMap[y, x] = Mathf.Clamp01(normalizedHeight);
            }
        }

        return noiseMap;
    }
}
