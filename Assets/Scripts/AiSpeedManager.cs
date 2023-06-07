//Mohamed Abdellatif Kallel
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiSpeedManager : MonoBehaviour
{
    [SerializeField] private float ratioAvance = 0.5f;
    [SerializeField] private float ratioDerrière = 1.5f;
    [SerializeField] private float distanceMax = 20;
    
    private GameObject Classement;
    private GameObject joueur;
    
    private MotorComponent mc;
    private DistanceCalculator thisCalculateurDistance;
    private DistanceCalculator JoueurCalculateurDistance;
    private Classement classement;
    
    private float thisDistance;
    private float joueurDistance;
    
    public float distanceRatio { get; private set; }
    
    
    private void Start()
    {
        joueur = GameObject.Find("Joueur");
        JoueurCalculateurDistance = joueur.GetComponent<DistanceCalculator>();
        
        thisCalculateurDistance = GetComponent<DistanceCalculator>();
        Classement = GameObject.Find("PlayerManager");
        mc = GetComponent<MotorComponent>();

        classement = Classement.GetComponent<Classement>();

    }

    private int ClassementAI()
        => classement.classementFinal.Keys.ToList().IndexOf(this.name);

    private int ClassementJoueur()
        => classement.classementFinal.Keys.ToList().IndexOf("Joueur");

    private bool EstAvance()
        => ClassementAI() < ClassementJoueur();

    private bool EstLoin()
        => Mathf.Abs(thisDistance - joueurDistance) > distanceMax;


    // Update is called once per frame
    void Update()
    {
        joueurDistance = JoueurCalculateurDistance.TotalDistance;
        thisDistance = thisCalculateurDistance.TotalDistance;

        if (EstLoin()) 
        {
            distanceRatio = EstAvance() ? ratioAvance : ratioDerrière;
        }
        else
        {
            distanceRatio = 1;
        }
    }
}
