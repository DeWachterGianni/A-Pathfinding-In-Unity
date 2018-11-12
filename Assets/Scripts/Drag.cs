using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 cameraOrigin;


    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            cameraOrigin = Camera.main.transform.position;
            return;
        }

        if (!Input.GetMouseButton(2)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Camera.main.transform.position = cameraOrigin - (pos * dragSpeed);
    }

}
