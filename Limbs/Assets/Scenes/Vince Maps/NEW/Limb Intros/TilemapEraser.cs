using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapEraser : MonoBehaviour
{
    public Tilemap tilemap;
    public int mapWidth = 10;
    public int maxMapHeight = 10;
    public int startY = -10;
    public float delay = 0.01f;
    public float eraseInterval = 5f;
    public bool eraseFromTopToBottom = true;
    public bool eraseInsideOut = true;

    private float timer;

    void Start()
    {
        Invoke("StartErasingTilemap", eraseInterval);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= eraseInterval)
        {
            timer = 0f;
            StartErasingTilemap();
        }
    }

    public void StartErasingTilemap()
    {
        StartCoroutine(EraseTilemap());
    }

    IEnumerator EraseTilemap()
    {
        if (eraseFromTopToBottom)
        {
            for (int y = startY + maxMapHeight; y >= startY; y--)
            {
                StartCoroutine(EraseRow(y));
                yield return new WaitForSeconds(delay);
            }
        }
        else
        {
            for (int y = startY; y <= startY + maxMapHeight; y++)
            {
                StartCoroutine(EraseRow(y));
                yield return new WaitForSeconds(delay);
            }
        }
    }

    IEnumerator EraseRow(int y)
    {
        int middleX = 0;
        if (eraseInsideOut)
        {
            for (int i = 0; i <= mapWidth; i++)
            {
                Vector3Int leftPosition = new Vector3Int(middleX - i, y, 0);
                Vector3Int rightPosition = new Vector3Int(middleX + i, y, 0);
                EraseTileAtPosition(leftPosition);
                EraseTileAtPosition(rightPosition);
                yield return new WaitForSeconds(delay);
            }
        }
        else
        {
            for (int i = mapWidth; i >= 0; i--)
            {
                Vector3Int leftPosition = new Vector3Int(middleX - i, y, 0);
                Vector3Int rightPosition = new Vector3Int(middleX + i, y, 0);
                EraseTileAtPosition(leftPosition);
                EraseTileAtPosition(rightPosition);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private void EraseTileAtPosition(Vector3Int position)
    {
        if (tilemap.HasTile(position))
        {
            tilemap.SetTile(position, null);
        }
    }
}
