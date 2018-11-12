using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    public int Id;
    public GameObject GameObject;
    public int X, Y;
    public TileType Type;
    public int GScore, HScore, FScore;
    public Tile GotoTile;

    public enum TileType { Border, Wall, Walkable, Start, End };

    public static TileType CalculateBorder(int x, int y)
    {
        if (x == 0 || y == 0 || x == Tilemap.Instance.Width - 1 || y == Tilemap.Instance.Height - 1)
            return TileType.Border;     
        return TileType.Walkable;
    }

    public void ChangeType(TileType Type)
    {
        this.Type = Type;
        UpdateTileColor();
    }

    public void UpdateTileColor()
    {
        //Tile Color
        switch (Type)
        {
            case TileType.Border:
                ChangeColor(Color.black);
                break;
            case TileType.End:
                ChangeColor(Color.red);
                break;
            case TileType.Walkable:
                ChangeColor(Color.white);
                break;
            case TileType.Start:
                ChangeColor(Color.green);
                break;
            case TileType.Wall:
                ChangeColor(Color.grey);
                break;
        }
    }

    public void ChangeColor(Color color)
    {
        GameObject.GetComponent<SpriteRenderer>().color = color;
    }

}

