// script écrit par Frédéric Gaudreault

using UnityEngine;
using UnityEngine.Serialization;

public class WaypointManager : MonoBehaviour
{
    // Toutes les informations importantes que les waypoints contiennent
    // les références de ces tableaux sont utilisés par AIHandler.cs
    [HideInInspector] public Waypoint[] waypoints;
    [HideInInspector] public Vector3[] waypointPositions;
    [FormerlySerializedAs("distanceMinimumWaypoints")] [HideInInspector] public float[] minimumDistanceToWaypoints;
    [FormerlySerializedAs("vitesseMaxÀCesWaypoints")] [HideInInspector] public float[] maxSpeedAtWaypoints;
    [HideInInspector] public int nbWaypoints;

    private void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>(); // Rassembler tous les waypoints formant les circuit
        nbWaypoints = waypoints.Length;
        waypointPositions = new Vector3[nbWaypoints];
        minimumDistanceToWaypoints = new float[nbWaypoints];
        maxSpeedAtWaypoints = new float[nbWaypoints];
        
        for (int i = 0; i < nbWaypoints; ++i)
        {
            // rassembler les positions, les distance min. et les vitesses max. des waypoints
            waypointPositions[i] = waypoints[i].transform.position;
            minimumDistanceToWaypoints[i] = waypoints[i].distanceMinimumWaypoint;
            maxSpeedAtWaypoints[i] = waypoints[i].vitesseMaxÀCeWaypoint;
        }
    }
}
