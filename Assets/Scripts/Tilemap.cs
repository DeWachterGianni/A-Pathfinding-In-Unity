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

        //Left tile
        if(tile.X - 1 >= 0 && tile.X - 1 < Width)
        {
            if(Tiles[tile.X - 1,tile.Y].Type == Tile.TileType.Walkable || Tiles[tile.X - 1, tile.Y].Type == Tile.TileType.End)
            {
                adjacent.Add(new TileInfo() {Tile = Tiles[tile.X - 1, tile.Y] , Diagonal = false });
            }
        }
        //Right tile
        if (tile.X + 1 >= 0 && tile.X + 1 < Width)
        {
            if (Tiles[tile.X + 1, tile.Y].Type == Tile.TileType.Walkable || Tiles[tile.X + 1, tile.Y].Type == Tile.TileType.End)
            {
                adjacent.Add(new TileInfo() { Tile = Tiles[tile.X + 1, tile.Y], Diagonal = false });
            }
        }
        //Up tile
        if (tile.Y - 1 >= 0 && tile.Y - 1 < Height)
        {
            if (Tiles[tile.X, tile.Y - 1].Type == Tile.TileType.Walkable || Tiles[tile.X, tile.Y - 1].Type == Tile.TileType.End)
            {
                adjacent.Add(new TileInfo() { Tile = Tiles[tile.X, tile.Y - 1], Diagonal = false });
            }
        }
        //Down tile
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
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X - 1, tile.Y + 1], Diagonal = false });
                }
            }
            //Right down tile x++ y++
            if (tile.X + 1 < Width && tile.Y + 1 < Height)
            {
                if (Tiles[tile.X + 1, tile.Y + 1].Type == Tile.TileType.Walkable || Tiles[tile.X + 1, tile.Y + 1].Type == Tile.TileType.End)
                {
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X + 1, tile.Y + 1], Diagonal = false });
                }
            }
            //Left Up tile x-- y--
            if (tile.X - 1 >= 0 && tile.Y - 1 >= 0)
            {
                if (Tiles[tile.X - 1, tile.Y - 1].Type == Tile.TileType.Walkable || Tiles[tile.X - 1, tile.Y - 1].Type == Tile.TileType.End)
                {
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X - 1, tile.Y - 1], Diagonal = false });
                }
            }
            //Right Up tile x++ y--
            if (tile.X + 1 < Width && tile.Y - 1 >= 0)
            {
                if (Tiles[tile.X + 1, tile.Y - 1].Type == Tile.TileType.Walkable || Tiles[tile.X + 1, tile.Y - 1].Type == Tile.TileType.End)
                {
                    adjacent.Add(new TileInfo() { Tile = Tiles[tile.X + 1, tile.Y - 1], Diagonal = false });
                }
            }
        }

        return adjacent;
    }

	void Update () {
        //Fast edit
        if(Input.GetKey(KeyCode.A) && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit != false && hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    int x = int.Parse(hit.collider.name.Substring(hit.collider.name.IndexOf(",") + 1));
                    int y = int.Parse(hit.collider.name.Remove(hit.collider.name.IndexOf(",")));

                    Tile tile = Tiles[y, x];
                    changeClickedTile(tile, Tile.TileType.Wall);
                }
            }
            return;
        }
        else if (Input.GetKey(KeyCode.Z) && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit != false && hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    int x = int.Parse(hit.collider.name.Substring(hit.collider.name.IndexOf(",") + 1));
                    int y = int.Parse(hit.collider.name.Remove(hit.collider.name.IndexOf(",")));

                    Tile tile = Tiles[y, x];
                    changeClickedTile(tile, Tile.TileType.Walkable);
                }
            }
            return;
        }

        //Clicking
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit != false && hit.collider != null)
            {
                if(hit.collider.gameObject.tag == "Tile")
                {
                    int x = int.Parse(hit.collider.name.Substring(hit.collider.name.IndexOf(",") + 1));
                    int y = int.Parse(hit.collider.name.Remove(hit.collider.name.IndexOf(",")));

                    // X and Y are here reversed because else it didn't work
                    // Why you ask me?
                    // Well I don't know...
                    // Let me know
                    leftClickedOnTile(Tiles[y,x]);                    
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit != false && hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Tile")
                {
                    int x = int.Parse(hit.collider.name.Substring(hit.collider.name.IndexOf(",") + 1));
                    int y = int.Parse(hit.collider.name.Remove(hit.collider.name.IndexOf(",")));

                    // X and Y are here reversed because else it didn't work
                    // Why you ask me?
                    // Well I don't know...
                    // Let me know
                    rightClickedOnTile(Tiles[y, x]);
                }
            }
        }
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
                    Id = i
                };
                go.name = x + "," + y;
                go.transform.position = new Vector3(x * 1 - xOffset, -y * 1 + yOffset, 0);
                Tiles[x,y].UpdateTileColor();
                i++;
            }
        }
    }

    void leftClickedOnTile(Tile clickedTile)
    {
        //May I change tile?
        if (clickedTile.Type == Tile.TileType.Border)
        {
            Log.Instance.AddToQueue("You can't edit the border tiles! :'(");
            return;
        }

        //Handling tile setting
        if (EndTile == clickedTile)
            EndTile = null;

        if (StartTile == clickedTile)
            StartTile = null;

        //Start and End tile control
        if (Control.Instance.CanSetEndTile)
        {
            if(EndTile != null)
            {
                EndTile.ChangeType(Tile.TileType.Walkable);
                EndTile = null;
            }

            EndTile = clickedTile;
            clickedTile.ChangeType(Tile.TileType.End);
            Control.Instance.CanSetEndTile = false;
            Log.Instance.AddToQueue("End tile chosen!");
            return;
        }
        else if (Control.Instance.CanSetStartTile)
        {
            if (StartTile != null)
            {
                StartTile.ChangeType(Tile.TileType.Walkable);
                StartTile = null;
            }

            StartTile = clickedTile;
            clickedTile.ChangeType(Tile.TileType.Start);
            Control.Instance.CanSetStartTile = false;
            Log.Instance.AddToQueue("Start tile chosen!");
            return;
        }

        //Wall / Normal
        if (clickedTile.Type == Tile.TileType.Walkable)
            clickedTile.ChangeType(Tile.TileType.Wall);
        else
            clickedTile.ChangeType(Tile.TileType.Walkable);
    }

    void changeClickedTile(Tile clickedTile, Tile.TileType type)
    {
        //May I change tile?
        if (clickedTile.Type == Tile.TileType.Border)
        {
            Log.Instance.AddToQueue("You can't edit the border tiles! :'(");
            return;
        }

        //Handling tile setting
        if (EndTile == clickedTile)
            EndTile = null;

        if (StartTile == clickedTile)
            StartTile = null;

        //Start and End tile control not allowed here
        if (Control.Instance.CanSetEndTile || Control.Instance.CanSetStartTile)
        {          
            Control.Instance.CanSetEndTile = false;
            Control.Instance.CanSetStartTile = false;
        }

        //Change
        clickedTile.ChangeType(type);
    }

    void rightClickedOnTile(Tile clickedTile)
    {

        Debug.Log("Tile: " + clickedTile.GameObject.name + "\n" + 
            "H Score: " + clickedTile.HScore + " | " +
            "G Score: " + clickedTile.GScore +" | " +
            "F Score: " + clickedTile.FScore);


    }

    public void UpdateTileScore(Tile tile)
    {
        int x = int.Parse(tile.GameObject.name.Substring(tile.GameObject.name.IndexOf(",") + 1));
        int y = int.Parse(tile.GameObject.name.Remove(tile.GameObject.name.IndexOf(",")));
        Tiles[x, y].FScore = tile.FScore;
        Tiles[x, y].GScore = tile.GScore;
        Tiles[x, y].HScore = tile.HScore;
    }

    public class TileInfo
    {
        public Tile Tile;
        public bool Diagonal;
    }

}
