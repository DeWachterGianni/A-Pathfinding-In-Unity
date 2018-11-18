using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour {

    void Update()
    {
        if (Pathfinding.Instance.Running)
            return;

        Tile rayTile = raycastCurrentTile();
        if (rayTile == null)
            return;

        //Fast edit while dragging
        if (Input.GetKey(KeyCode.A) && Input.GetMouseButton(0))
        {
            Tilemap.Instance.TryChangeTile(rayTile, Tile.TileType.Wall);
            return;
        }
        else if (Input.GetKey(KeyCode.Z) && Input.GetMouseButton(0))
        {
            Tilemap.Instance.TryChangeTile(rayTile, Tile.TileType.Walkable);
            return;
        }

        //Setting Start and End tile
        if (Input.GetKey(KeyCode.S) && Input.GetMouseButton(0))
        {
            Tilemap.Instance.TryChangeTile(rayTile, Tile.TileType.Start);
        }
        else if (Input.GetKey(KeyCode.E) && Input.GetMouseButton(0))
        {
            Tilemap.Instance.TryChangeTile(rayTile, Tile.TileType.End);
        }

        //Clicking left and right
        if (Input.GetMouseButtonDown(0)) //Left click
        {         
            Tilemap.Instance.TileLeftClick(rayTile);
            return;
        }
        if (Input.GetMouseButtonDown(1)) //Right click
        {
            Tilemap.Instance.TileLeftClick(rayTile);
            return;
        }

    }

    Tile raycastCurrentTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit != false && hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Tile")
            {
                int x = int.Parse(hit.collider.name.Substring(hit.collider.name.IndexOf(",") + 1));
                int y = int.Parse(hit.collider.name.Remove(hit.collider.name.IndexOf(",")));

                return Tilemap.Instance.Tiles[y, x];
            }
        }
        return null;
    }

}
