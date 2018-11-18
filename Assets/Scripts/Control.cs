using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Control : MonoBehaviour {

    public static Control Instance;

    [SerializeField]
    Slider slider;
    [SerializeField]
    TextMeshProUGUI lblWaitTime;
    [SerializeField]
    Toggle tglDiagonal;

    void Start()
    {
        Instance = this;

        UpdateWaitTime();
    }
  
    public void Begin()
    {
        if(Tilemap.Instance.StartTile == null)
        {
            Log.Instance.AddToQueue("Choose a start tile!");
            return;
        }
        else if(Tilemap.Instance.EndTile == null)
        {
            Log.Instance.AddToQueue("Choose a end tile!");
            return;
        }

        Log.Instance.AddToQueue("Let's get started!");
        Pathfinding.Instance.Begin();
    }

    public void UpdateWaitTime()
    {
        Pathfinding.Instance.WaitTime = slider.value;
        lblWaitTime.text = slider.value.ToString() + " sec";
    }

    public bool DoDiagonal
    {
        get
        {
            return tglDiagonal.isOn;
        }
    }

    public void ClearGrid()
    {
        if(Pathfinding.Instance.Running == false)
        {
            foreach (Tile tile in Tilemap.Instance.Tiles)
            {
                tile.UpdateTileColor();
                tile.GScore = 0;
                tile.FScore = 0;
                tile.HScore = 0;
                tile.GotoTile = null;
                tile.ChangeFScore(0);
                tile.ChangeGoToTile(null);
            }
        }
        else
        {
            Log.Instance.AddToQueue("Can't clear grid while running!");
        }
    }

}
