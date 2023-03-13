using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Ground : MonoBehaviour
{
    public Tilemap tilemap;

    //[SerializeField] Sprite Breakable;
    [SerializeField] Tile UnBreakable;
    [SerializeField] bool UnBreakableOnOff = false;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void Digged(Vector3 pos)
    {
        
        Vector3Int cellPosition = tilemap.WorldToCell(pos);
        TileData t = new TileData();
        
        TileBase tb = tilemap.GetTile(cellPosition);
        if (tb)
            tb.GetTileData(cellPosition, tilemap, ref t);
        else
            return;
        Debug.Log(t.sprite);
        if(UnBreakable && UnBreakableOnOff)
        if (t.sprite == UnBreakable.sprite)
            return;

        tilemap.SetTile(cellPosition, null);
        Debug.Log("Fill null:" + cellPosition);
    }
}
