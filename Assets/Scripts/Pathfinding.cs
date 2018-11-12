﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding Instance;
    public float WaitTime = 0f;
    public bool Found = false;
    public bool Running = false;

    List<int> openList, closedList;

    int steps = 1;
    bool diagonal;

    void Start () {
        Instance = this;

        openList = new List<int>();
        closedList = new List<int>();
    }
    
    public void Begin()
    {
        if (Running)
            return;

        Running = true;
        steps = 1;
        Found = false;
        diagonal = Control.Instance.DoDiagonal;

        foreach (Tile tile in Tilemap.Instance.Tiles)
        {
            if(tile.Type == Tile.TileType.Walkable)
            {
                tile.UpdateTileColor();
            }
        }

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        int checks = 0;
        openList.Clear();
        closedList.Clear();

        //Calculate all H scores
        foreach (Tile tile in Tilemap.Instance.Tiles)
        {
            //checks++; should I let it count here as well???
            if(tile.Type == Tile.TileType.Walkable || tile.Type == Tile.TileType.Start)
            {
                tile.HScore = Mathf.Abs(Tilemap.Instance.EndTile.X - tile.X) + Mathf.Abs(Tilemap.Instance.EndTile.Y - tile.Y);
            }
            Log.Instance.SetCheckAmount(checks);
        }

        //Add start tile to open list
        openList.Add(Tilemap.Instance.StartTile.Id);
        bool found = false;
        do
        {
            Tile center = smallestFScoreInOpenList();

            if(center == null)
            {
                Log.Instance.AddToQueue("No solution found :'(");
                break;
            }

            foreach (Tilemap.TileInfo tileInfo in Tilemap.Instance.GetAdjacentWalkableTiles(center, diagonal))
            {
                Tile neighbourTile = tileInfo.Tile;
                int score = (tileInfo.Diagonal == true ? 14 : 10); //if tile is diagonal score is higher

                if (neighbourTile.Id == 121)
                    Debug.Log("NOW 121");

                //Amount of checks performed
                checks++;
                Log.Instance.SetCheckAmount(checks);

                //If tile is the end tile
                if(neighbourTile.Type == Tile.TileType.End)
                {                 
                    Debug.Log("found end");
                    found = true;
                    neighbourTile.GotoTile = center;
                    break;
                }

                //Add tile to open list if it isn't in any of the lists
                if (!openList.Contains(neighbourTile.Id) && !closedList.Contains(neighbourTile.Id))
                {
                    openList.Add(neighbourTile.Id);
                    neighbourTile.GotoTile = center;
                }
              
                //Update tile scores
                neighbourTile.GScore = center.GScore + score;
                neighbourTile.FScore = neighbourTile.GScore + neighbourTile.HScore;

                //If this tile path is better than the existing one
                if (openList.Contains(neighbourTile.Id))
                    if (center.FScore + score < neighbourTile.FScore)
                    {
                        neighbourTile.GotoTile = center;
                    }
            }

            foreach (int id in openList)
            {
                idToTile(id).ChangeColor(Color.cyan);
            }

            if(center.Type == Tile.TileType.Start)
                center.ChangeColor(Color.green);
            else
                center.ChangeColor(Color.yellow);

            openList.Remove(center.Id);
            closedList.Add(center.Id);
            
            yield return new WaitForSeconds(WaitTime);
        } while (!found);

        if (!found)
        {
            Log.Instance.AddToQueue("No solution found :'(");
            Running = false;
            StopCoroutine(Loop());
            yield return null;
        }
        else
        {


            bool retracing = true;
            Tile start = null;
            do
            {
                if (start == null)
                    start = Tilemap.Instance.EndTile.GotoTile;
                else
                    start = start.GotoTile;

                if (start == Tilemap.Instance.StartTile)
                {
                    Debug.Log("Dit it!");
                    retracing = false;
                }
                else
                {
                    start.ChangeColor(Color.magenta);
                }

                yield return new WaitForSeconds(WaitTime);

            } while (retracing);

            Running = false;
            StopCoroutine(Loop());
        }
    }

    Tile smallestFScoreInOpenList()
    {
        if (openList.Count == 0)
            return null;

        Tile lowest = idToTile(openList[0]);
        foreach (int id in openList)
        {
            Tile p = idToTile(id);
            if (p.FScore < lowest.FScore)
                lowest = p;
        }

        return lowest;
    }

    Tile idToTile(int Id)
    {
        foreach (Tile tile in Tilemap.Instance.Tiles)
        {
            if (Id == tile.Id)
                return tile;
        }
        return null;
    }

}