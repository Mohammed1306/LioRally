//Écrit par Mohamed Abdellatif Kallel
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objetsMinimap;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private List<Transform> spawns;
    
    private Transform spawnJoueur;
    private Transform[] spawnAI;
    
    public Transform targetDC;    

    [SerializeField] private PlayerCameraMovement camera;

    private Random rnd;
    private int indexSpawnJoueur;

    private GameObject[] AIs;

    private GameObject chosenCar;
    private int nbAI;

    private GameObject[] cameras;

    private void Awake()
    {
        camera.enabled = true;
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == ChoicesMenu.choicesMenu.chosenCar)
            {
                chosenCar = prefabs[i];
            }
        }

        
        nbAI = ChoicesMenu.choicesMenu.nbAI;
        
        AIs = new GameObject[nbAI];

        rnd = new Random();
        indexSpawnJoueur = rnd.Next(0, spawns.Count);
        spawnJoueur = spawns[indexSpawnJoueur];
        spawns.Remove(spawnJoueur);

        SpawnJoueur();
        SpawnAI();
    }

    private void SpawnJoueur()
    {
        GameObject joueur = Instantiate(chosenCar, spawnJoueur.position, spawnJoueur.rotation);
        joueur.name = "Joueur";
        
        GameObject icone = Instantiate(objetsMinimap[0]);
        icone.GetComponent<SurvolerObjet>().objet = joueur.transform;
        icone.GetComponent<SurvolerObjet>().hauteur = 7f;
        icone.name = "IconeJoueur";


        GameObject canvas = GameObject.Find("Canvas");
        canvas.GetComponent<NeedleMovement>().vehicle = joueur;
        canvas.GetComponent<CompteurVitesse>().véhicule = joueur;
        canvas.GetComponent<BarreDeVie>().véhicule = joueur;
        canvas.GetComponent<DamageBlinker>().vehicle = joueur;
        

        GameObject listeCameras = joueur.transform.Find("ListeCameras").GameObject();
        camera.camerasChoisies[0] = listeCameras.transform.Find("CameraSuivreDerrière").GameObject();
        camera.camerasChoisies[1] = listeCameras.transform.Find("CameraSuivreCôté").GameObject();
        camera.camerasChoisies[2] = listeCameras.transform.Find("CameraTop").GameObject();
        camera.camerasChoisies[3] = listeCameras.transform.Find("CameraCapot").GameObject();
        camera.cameraArrière = listeCameras.transform.Find("CameraArrière").GameObject();
        
        joueur.GetComponent<DistanceCalculator>().target = joueur.transform;
        joueur.GetComponent<DistanceCalculator>().enabled = true;

        joueur.GetComponent<SteerComponent>().estJoueur = true;
        foreach (AudioSource son in joueur.GetComponents<AudioSource>())
            son.enabled = true;
        
        joueur.GetComponent<SonSurface>().enabled = true; 
    }

    private void SpawnAI()
    {
        for (int i = 0; i < nbAI; i++)
        {
            
            AIs[i] = Instantiate(prefabs[rnd.Next(0, prefabs.Length)], spawns[i].position, spawns[i].rotation);
            AIs[i].name = $"AI{i}";
            AIs[i].GetComponent<AIHandler>().enabled = true;

            GameObject icone = Instantiate(objetsMinimap[1]);
            icone.GetComponent<SurvolerObjet>().objet = AIs[i].transform;
            icone.GetComponent<SurvolerObjet>().hauteur = 5f;
            icone.name = $"IconeEnnemi{i}";
            
            AIs[i].GetComponent<DistanceCalculator>().target = AIs[i].transform;
            AIs[i].GetComponent<DistanceCalculator>().enabled = true;

            AIs[i].GetComponent<AiSpeedManager>().enabled = true;

        }
    }

    private GameObject AIToSpawn()
    {
        GameObject AItoSpawn = prefabs[rnd.Next(0, prefabs.Length)];
        while (AItoSpawn==chosenCar)
        {
            AItoSpawn = prefabs[rnd.Next(0, prefabs.Length)];
        }

        return AItoSpawn;
    }
}
