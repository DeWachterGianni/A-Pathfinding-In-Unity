using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour {

    public static Tilemap Instance;

    public int Width, Height;
    public Tile StartTile, EndTile;
    public Tile[,] Tiles;

    [SerializeField]
    GameObject tilePrefab;
    [SerializeField]
    Sprite arrowSprite;

	void Start () {
        Instance = this;

        StartTile = null;
        EndTile = null;

        createTilegrid();
	}
	
    public List<TileInfo> GetAdjacentWalkableTiles(Tile tile, bool diagonal)
    {
        List<TileInfo> adjacent = new List<TileInfo>();

        if(tile == null)
        {
            Debug.Log("Tile is null when getting adjacents");
            return adjacent;
        }

        //Left tile x--
        if(tile.X - 1 >= 0 && tile.X - 1 < Width)
        {
            if(Tiles[tile.X - 1,tile.Y].Type == Tile.TileType.Walkable || Tiles[tile.X - 1, tile.Y].Type == Tile.TileType.End)
            {
                adjacent.Add(new TileInfo() {Tile = Tiles[tile.X - 1, tile.Y] , Diagonal = false });
            }
        }
        //Right tile x++
        if (tile.X + 1 >= 0 && tile.X + 1 < Width)
        {
            if (Tiles[tile.X + 1, tile.Y].Type == Tile.TileType.Walkable || Tiles[tile.X + 1, tile.Y].Type == Tile.TileType.End)
            {
                adjacent.Add(new TileInfo() { Tile = Tiles[tile.X + 1, tile.Y], Diagonal = false });
            }
        }
        //Up tile y--
        if (tile.Y - 1 >= 0 && tile.Y - 1 < Height)
        {
            if (Tiles[tile.X, tile.Y - 1].Type == Tile.TileType.Walkable || Tiles[tile.X, tile.Y - 1].Type == Tile.TileType.End)
            {
                adjacent.Add(new TileInfo() { Tile = Tiles[tile.X, tile.Y - 1], Diagonal = false });
            }
        }
        //Down tile y++
        if (tile.Y + 1 >= 0 && tile.Y + 1 < Height)
        {
            if (Tiles[tile.X, tile.Y + 1].Type == Tile.TileType.Walkable || Tiles[tile.X, tile.Y + 1].Type == Tile.TileType.End)
            {
                adjacent.Add(new TileInfo() { Tile = Tiles[tile.X, tile.Y + 1], Diagonal = false });
            }
        }

        //Diagonal -> may be broken :'(
        if (diagonal)
        {
            //Left down tile // x-- y++
            if (tile.X - 1 >= 0 && tile.Y + 1 < Height)
            {
                if (Tiles[tile.X - 1, tile.Y + 1].Type == Tile.TileType.Walkable || Tiles[tile.X - 1, tile.Y + 1].Type == Tile.TileType.End)
                {
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X - 1, tile.Y + 1], Diagonal = true });
                }
            }
            //Right down tile x++ y++
            if (tile.X + 1 < Width && tile.Y + 1 < Height)
            {
                if (Tiles[tile.X + 1, tile.Y + 1].Type == Tile.TileType.Walkable || Tiles[tile.X + 1, tile.Y + 1].Type == Tile.TileType.End)
                {
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X + 1, tile.Y + 1], Diagonal = true });
                }
            }
            //Left Up tile x-- y--
            if (tile.X - 1 >= 0 && tile.Y - 1 >= 0)
            {
                if (Tiles[tile.X - 1, tile.Y - 1].Type == Tile.TileType.Walkable || Tiles[tile.X - 1, tile.Y - 1].Type == Tile.TileType.End)
                {
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X - 1, tile.Y - 1], Diagonal = true });
                }
            }
            //Right Up tile x++ y--
            if (tile.X + 1 < Width && tile.Y - 1 >= 0)
            {
                if (Tiles[tile.X + 1, tile.Y - 1].Type == Tile.TileType.Walkable || Tiles[tile.X + 1, tile.Y - 1].Type == Tile.TileType.End)
                {
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X + 1, tile.Y - 1], Diagonal = true });
                }
            }
        }

        return adjacent;
    }

    void createTilegrid()
    {
        Tiles = new Tile[Width, Height];

        float xOffset = Width / 2;
        float yOffset = Height / 2;
        int i = 0;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                GameObject go = Instantiate(tilePrefab, transform);
                Tiles[x, y] = new Tile()
                {
                    GameObject = go,
                    X = x,
                    Y = y,
                    Type = Tile.CalculateBorder(x, y),
                    GotoTile = null,
                    Id = i,
                    ArrowSprite = arrowSprite,                   
                };
                go.name = x + "," + y;
                go.transform.position = new Vector3(x * 1 - xOffset, -y * 1 + yOffset, 0);
                Tiles[x,y].UpdateTileColor();
                i++;
            }
        }
    }

    public void TryChangeTile(Tile Tile, Tile.TileType Type)
    {
        //May I change tile?
        if (Tile.Type == Tile.TileType.Border)
        {
            Log.Instance.AddToQueue("You can't edit the border tiles! :'(");
            return;
        }

        //Is the changing type end or start
        if(Type == Tile.TileType.Start)
        {
            if (StartTile != null)
                StartTile.ChangeType(Tile.TileType.Walkable);

            StartTile = Tile;
            StartTile.ChangeType(Tile.TileType.Start);
            return;
        }
        else if(Type == Tile.TileType.End)
        {
            if (EndTile != null)
                EndTile.ChangeType(Tile.TileType.Walkable);

            EndTile = Tile;
            EndTile.ChangeType(Tile.TileType.End);
            return;
        }

        //Is the tile the end or start
        if (Tile.Type == Tile.TileType.Start)
            StartTile = null;
        else if (Tile.Type == Tile.TileType.End)
            EndTile = null;

        //Change
        Tile.ChangeType(Type);
    }

    public void TileRightClick(Tile Tile)
    {
        Debug.Log("Tile " + Tile.Id + ": " + Tile.GameObject.name + "\n" + 
            "H Score: " + Tile.HScore + " | " +
            "G Score: " + Tile.GScore +" | " +
            "F Score: " + Tile.FScore + " | " +
            "Goto tile id: " + (Tile.GotoTile == null ? "NULL" : Tile.GotoTile.Id.ToString()));
    }

    public void TileLeftClick(Tile Tile)
    {
        if(Tile.Type == Tile.TileType.Walkable)
            TryChangeTile(Tile, Tile.TileType.Wall);
        else
            TryChangeTile(Tile, Tile.TileType.Walkable);
    }

    public void UpdateTileScore(Tile Tile)
    {
        int x = int.Parse(Tile.GameObject.name.Substring(Tile.GameObject.name.IndexOf(",") + 1));
        int y = int.Parse(Tile.GameObject.name.Remove(Tile.GameObject.name.IndexOf(",")));
        Tiles[x, y].FScore = Tile.FScore;
        Tiles[x, y].GScore = Tile.GScore;
        Tiles[x, y].HScore = Tile.HScore;
    }

    public class TileInfo
    {
        public Tile Tile;
        public bool Diagonal;
    }

}
