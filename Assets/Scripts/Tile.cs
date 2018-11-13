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
    public Sprite ArrowSprite;

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

    public void ChangeFScore(int FScore)
    {
        if (Type == TileType.Border || Type == TileType.Wall || FScore == 0)
        {
            GameObject.GetComponentInChildren<TextMesh>().text = "";
            return;
        }
        this.FScore = FScore;
        GameObject.GetComponentInChildren<TextMesh>().text = this.FScore.ToString();
    }

    public void ChangeGoToTile(Tile GotoTile)
    {
        this.GotoTile = GotoTile;

        if (GotoTile == null)
        {
            GameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        GameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = ArrowSprite;

        Vector3 targ = GotoTile.GameObject.transform.position;
        targ.z = 0f;

        Vector3 objectPos = GameObject.transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        GameObject.transform.GetChild(1).gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));     
    }

}

