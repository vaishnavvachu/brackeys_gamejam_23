using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        var colGameObject = col.gameObject;

        if (!colGameObject.CompareTag("Bomb")) { return; }
        
        // Destroy Tiles
        DestroyTiles();
    }

    void DestroyTiles()
    {
        var grid = FindObjectOfType<Grid>();
        var tilemap = grid.GetComponentInChildren<Tilemap>();
        // HACK: Set proper positions.
        for (int i = -8; i < -3; i++)
        {
            for (int j = -12; j < -10; j++)
            {
                tilemap.SetTile(new Vector3Int(), null);
            }
        }
    }
}
