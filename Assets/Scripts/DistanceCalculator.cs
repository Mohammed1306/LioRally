//Script écrit par Mohamed Abdellatif Kallel, Thomas Prévost
using System;
using UnityEngine;


public class DistanceCalculator : MonoBehaviour
{
    public GameObject road;
    public Transform target { get; set; }   
    public Transform startRace;
    [HideInInspector] public Transform[] nodesList;
    [HideInInspector] public int nextNode;
    [HideInInspector] public int previousNode;
    [HideInInspector] public int targetNextNode;
    [HideInInspector] public int targetPreviousNode;
    private float startDistance;
    private float endDistance;
    private float middleDistance;
    public float TotalDistance { get; private set; }
    public float DistanceRatio { get; private set; }

    private void Start()
    {
        road = GameObject.Find("WaypointManager");
        startRace = GameObject.Find("DC1").transform;
        nodesList = GetNodesList();
    }
    
    private void Update()
    {
        //premier noeud qui suit this
        nextNode = LocatePoint("first", startRace);
        //premier noeud qui suit le traget
        targetNextNode = LocatePoint("first", target.transform);
        //premier noeud qui précède this
        previousNode = LocatePoint("last", startRace);
        //premier noeud qui précède target
        previousNode = LocatePoint("last", target.transform);
        
        
        if (previousNode > 0)
        {
            middleDistance = CalculateInBetweenDistance(0, previousNode);
            endDistance = CalculateDistance(target.transform, nodesList[previousNode]);
            TotalDistance = endDistance + middleDistance;
        }
        else
        {
            TotalDistance = CalculateDistance(startRace, target.transform);
        }
    }
    
    
    
    public float GetDistance() => TotalDistance;

    private float CalculateDistance(Transform transform1, Transform transform2) => Vector3.Magnitude(transform1.position - transform2.position);
    
    private Transform[] GetNodesList()
    {
        var listeNoeuds = new Transform[road.transform.childCount];
        for (int i = 0; i < listeNoeuds.Length; i++)
            listeNoeuds[i] = road.transform.GetChild(i);
        return listeNoeuds;
    }
    
    private float CalculateInBetweenDistance(int first, int last)
    {
        float distance = 0;
        for (int i = first+1; i < last; i++)
            distance += CalculateDistance(nodesList[i], nodesList[i - 1]);
        return distance;
    }

    public int LocatePoint(string condition, Transform àLocaliser)
    {
        // Stocker les deux noeuds les plus proches
        // Retourner celui avec l'indice le plus élevé si c'est pour le point de départ
        // Retourner celui avec l'indice le moins élevé si c'est pour le point d'arrivée
        
        int[] nearestNodes = NearestNodes(àLocaliser); //Les deux points les plus proches
        
        if(condition == "first") //Si c'est pour point de départ
            return Math.Max(nearestNodes[0], nearestNodes[1]);
        
        return Math.Min(nearestNodes[0], nearestNodes[1]); //Si c'est pour point d'arrivée
    }

    private int[] NearestNodes(Transform àLocaliser)
    {
        //Trouver le node le plus proche
        //Trouver le deuxiéme node le plus proche

        int indiceNodeProche = 0;

        for (int i = 1; i < nodesList.Length; i++)
        {
            if (CalculateDistance(àLocaliser, nodesList[indiceNodeProche]) > CalculateDistance(àLocaliser, nodesList[i - 1]))
                indiceNodeProche = i-1;   
        }


        int indiceDeuxièmeNodeProche = indiceNodeProche+1;

        for (int i = 1; i < nodesList.Length; i++)
        {
            if (CalculateDistance(àLocaliser, nodesList[indiceDeuxièmeNodeProche]) > CalculateDistance(àLocaliser, nodesList[i - 1]) && i -1 != indiceNodeProche)
                indiceDeuxièmeNodeProche = i-1;   
        }


        return new[] { indiceNodeProche, indiceDeuxièmeNodeProche };
    }
}
