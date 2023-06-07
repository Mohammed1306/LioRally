//Script écrit par Thomas Prévost et Frédéric Gaudreault

using System.Collections.Generic;
using UnityEngine;

public class AdherencePneu : MonoBehaviour
{
    private WheelCollider[] wcs;
    private Collider[] colliderPrécédant;
    private SteerComponent sc;
    private WheelFrictionCurve friction;
    private Dictionary<string, float> adhérenceDuMatériel;
    private float[] adhérenceActuelle;
    //public bool estSurAsphalte;         -> en commentaire dans le cas où l'on trouve un son adéquat pour l'asphalte
    public bool estSurGravelle;
    public bool estEnLAire;
    
    void Awake()
    {
        wcs = GetComponentsInChildren<WheelCollider>();
        sc = GetComponent<SteerComponent>();
        colliderPrécédant = new Collider[4];
        adhérenceDuMatériel = new Dictionary<string, float>();
        adhérenceActuelle = new float[4];
        friction = wcs[0].forwardFriction;
        adhérenceDuMatériel.Add("Pavement (Instance)",1.2f);
        adhérenceDuMatériel.Add("Dirt (Instance)",0.8f);
        adhérenceDuMatériel.Add("Terrain (Instance)",0.9f);
        adhérenceDuMatériel.Add("Mur (Instance)",1.2f);
    }


    void Update()
    {
        //estSurAsphalte = estSurGravelle = estEnLAire = false;
        estSurGravelle = estEnLAire = false;
        WheelHit hit;

        for (int i = 0; i < 2; ++i) // pour les roues avant
        {
            wcs[i].GetGroundHit(out hit);
            if (hit.collider != colliderPrécédant[i]) // On ne calcul rien si c'est le même collider
            {
                adhérenceActuelle[i] = adhérenceDuMatériel[hit.collider.material.name];
                friction.stiffness = adhérenceActuelle[i];
                wcs[i].forwardFriction = friction;
                wcs[i].sidewaysFriction = friction;
                
                if (wcs[i].isGrounded && adhérenceActuelle[i] < 1) // utilisé pour le son des pneus
                    estSurGravelle = true;
                
                // if (wcs[i].isGrounded)
                //{
                //     if (adhérenceActuelle[i] < 1)
                //         estSurGravelle = true;    
                //     else
                //         estSurAsphalte = true;
                // }
            }
        }
        
        for (int i = 2; i < 4; ++i) // pour les roues arrières
        {
            wcs[i].GetGroundHit(out hit);
            if (hit.collider != colliderPrécédant[i])
            {
                adhérenceActuelle[i] = adhérenceDuMatériel[hit.collider.material.name];
                friction.stiffness = sc.estFreinAMainActif ? adhérenceActuelle[i] - 0.3f : adhérenceActuelle[i];
                wcs[i].forwardFriction = friction;
                wcs[i].sidewaysFriction = friction;
                
                if (wcs[i].isGrounded && adhérenceActuelle[i] < 1) 
                    estSurGravelle = true;
                
                // if (wcs[i].isGrounded)
                //{
                //     if (adhérenceActuelle[i] < 1)
                //         estSurGravelle = true;    
                //     else
                //         estSurAsphalte = true;
                // }
            }
        }

        //estEnLAire = !estSurAsphalte && !estSurGravelle;
        estEnLAire = !estSurGravelle;
    }
}
