using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Log : MonoBehaviour {

    public static Log Instance;

    [SerializeField]
    float timePerMessage = 1.8f;
    [SerializeField]
    TextMeshProUGUI lblLog, lblCount, lblCheckAmount;

    List<string> logs;
    bool canSend;
    float timeLeft;

	void Start () {
        Instance = this;

        logs = new List<string>();
        canSend = false;
    }
	
	void Update () {       
        lblCount.text = logs.Count.ToString() + " messages left";

        if (logs.Count == 0 && canSend)
            return;

        if (canSend)
        {
            canSend = false;
            timeLeft = timePerMessage;
            lblLog.text = logs[0];
            logs.RemoveAt(0);
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }

        if(timeLeft <= 0f)
        {
            lblLog.text = "";
            canSend = true;
        }
	}

    public void AddToQueue(string message)
    {
        logs.Add(message);
    }

    public void SetCheckAmount(int amount)
    {
        lblCheckAmount.text = "Checks: " + amount.ToString(); 
    }

}
