using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform target;
    public Camera cam;

    public float smoothSpeed = 10f;
    public Vector3 offset;

    public float minZoom = 40f;
    public float maxZoom = 25f;
    public float zoomLimiter = 50f;

    void Start()
    {
        cam = GetComponent<Camera>();
        
    }
    void Update()
    {
        Move();
        Zoom();
    }

    void Move()
    {
        //Set Target from Position of Players
        Vector3 desiredPosition = FindCenterOfPlayers() + offset;
        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPostion;
    }

    void Zoom()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float[] zarray = new float[players.Length];
        int i = 0;
        foreach (GameObject player in players)
        {
            zarray[i] = player.transform.position.z;
            i++;
        }

        float maxDistance = Mathf.Max(zarray) - Mathf.Min(zarray);

        float newZoom = Mathf.Lerp(maxZoom, minZoom, maxDistance/zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    Vector3 FindCenterOfPlayers()
    {
        Vector3 centerPoint = new Vector3();
        // Find all players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // calculate maximum extents in x and z
        float[] xarray = new float[players.Length];
        float[] zarray = new float[players.Length];
        int i = 0;
        foreach (GameObject player in players)
        {
            xarray[i] = player.transform.position.x;
            zarray[i] = player.transform.position.z;
            i++;
        }

        centerPoint.x = (Mathf.Max(xarray) + Mathf.Min(xarray)) / 2;
        centerPoint.y = 0;
        centerPoint.z = (Mathf.Max(zarray) + Mathf.Min(zarray)) / 2;

        return centerPoint;
    }
}
