using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTileGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile[] tiles;

    public int mapWidth = 10;
    public int maxMapHeight = 10;
    public int startY = -10;
    public float delay = 0.01f;
    public float fillChance = 0.75f;
    public int middleDepthReduction = 0;
    public float regenerationInterval = 5f;

    public int xSpacing = 0;
    public int ySpacing = 0;

    private float timer;
    public float startClearTimer = 1f;

    public void Start()
    {
        Invoke("ClearAndGenerateTilemap", startClearTimer);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= regenerationInterval)
        {
            timer = 0f;
            ClearAndGenerateTilemap();
        }
    }

    public void ClearAndGenerateTilemap()
    {
        StartCoroutine(ClearTilemap());
    }

    IEnumerator ClearTilemap()
    {
        for (int x = -mapWidth; x <= mapWidth; x++)
        {
            int reduction = Mathf.Max(0, middleDepthReduction - Mathf.Abs(x));
            int columnHeight = maxMapHeight - reduction;
            for (int y = startY; y < startY + columnHeight; y += (1 + ySpacing))
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(position))
                {
                    tilemap.SetTile(position, null);
                    if (x != -mapWidth)
                    {
                        Vector3Int mirrorPosition = new Vector3Int(-x - 1, y, 0);
                        tilemap.SetTile(mirrorPosition, null);
                    }
                    yield return new WaitForSeconds(delay);
                }
            }
        }
        GenerateNewTilemap();
    }

    public void GenerateNewTilemap()
    {
        tilemap.ClearAllTiles();
        StartCoroutine(GenerateTilemap());
    }

    IEnumerator GenerateTilemap()
    {
        for (int x = -mapWidth; x <= mapWidth; x += (1 + xSpacing))
        {
            int reduction = Mathf.Max(0, middleDepthReduction - Mathf.Abs(x));
            int columnHeight = Random.Range(0, maxMapHeight - reduction);
            for (int y = startY; y < startY + columnHeight; y += (1 + ySpacing))
            {
                if (Random.value < fillChance)
                {
                    Tile tile = GetRandomTile();
                    Vector3Int position = new Vector3Int(x, y, 0);
                    tilemap.SetTile(position, tile);
                    tilemap.SetColor(position, GetRandomColor());
                    if (x != -mapWidth)
                    {
                        Vector3Int mirrorPosition = new Vector3Int(-x - 1 - xSpacing, y, 0);
                        tilemap.SetTile(mirrorPosition, tile);
                        tilemap.SetColor(mirrorPosition, GetRandomColor());
                    }
                }
                yield return new WaitForSeconds(delay);
            }
        }
    }

    Tile GetRandomTile()
    {
        if (tiles.Length == 0)
        {
            Debug.LogError("No tiles assigned.");
            return null;
        }
        int randomIndex = Random.Range(0, tiles.Length);
        return tiles[randomIndex];
    }

    Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
