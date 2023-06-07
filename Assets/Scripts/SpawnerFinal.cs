//Écrit par Thomas Prévost

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class SpawnerFinal : MonoBehaviour
{
    [SerializeField] private Transform exitFireworks;
    [SerializeField] private GameObject[] objectsToSpawn;
    private Camera[] cameras;
    private GameObject joueur;
    [SerializeField]private Camera mainCamera;
    private float coolDown;
    private GameObject[] cars;
    private MotorComponent[] mc;
    [SerializeField]
    private TextMeshPro text1;
    [SerializeField]
    private TextMeshPro text2;
    [SerializeField]
    private TextMeshPro text3;
    
    

    private void Start()
    { 
        mc = FindObjectsOfType<MotorComponent>();
       cars = new GameObject[mc.Length];
       for (int i = 0; i < mc.Length; i++)
           cars[i] = mc[i].gameObject;


        foreach (var car in cars)
       {
           car.GetComponent<AIHandler>().enabled = false;
           car.GetComponent<SteerComponent>().estJoueur = true;
           car.GetComponent<MotorComponent>().enabled = false;
           car.GetComponent<TransmissionComponent>().enabled = false;
           car.GetComponent<DistanceCalculator>().enabled = false;
           car.GetComponent<CalculateurTemps>().courseTerminé = true;
           car.GetComponent<CarSound>().enabled = false;
           car.GetComponent<SonSurface>().enabled = false;
           foreach (AudioSource son in car.GetComponents<AudioSource>())
               son.enabled = false;
           
           int position = car.GetComponent<CalculateurTemps>().positionFinal;
           if (position == 0)
           {
               car.transform.position = new Vector3(0.15f, 3.09f, 0);
               car.transform.rotation = Quaternion.Euler(0,90,0);
               text1.gameObject.SetActive(true);
               text1.text = $": {car.name}";
           }
           if (position == 1)
           {
               car.transform.position = new Vector3(-6.4f, 2.09f, 0);
               car.transform.rotation = Quaternion.Euler(0,90,0);
               text2.gameObject.SetActive(true);
               text2.text = $": {car.name}";
           }

           if (position == 2)
           {
               car.transform.position = new Vector3(6.1f, 1.09f, 0);
               car.transform.rotation = Quaternion.Euler(0, 90, 0);
               text3.gameObject.SetActive(true);
               text3.text = $": {car.name}";
           }

           car.GetComponent<Rigidbody>().isKinematic = true;
       }
       
       cameras = FindObjectsOfType<Camera>();
       foreach (var cam in cameras)
           cam.gameObject.SetActive(false);

        mainCamera.gameObject.SetActive(true);
    }
    
    void Update()
    {
        if (coolDown > 2)
        {
            foreach (var firework in objectsToSpawn)
                Instantiate(firework, exitFireworks);
            coolDown = 0;
        }

        coolDown += Time.deltaTime;
    }
}
