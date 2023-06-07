using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script fait par Mohammed Abdellatif Kallel;

public class ChoicesMenu : MonoBehaviour
{
    public static ChoicesMenu choicesMenu;

    private void Awake()
    {
        if (choicesMenu == null)
        {
            choicesMenu = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string chosenCar { get; private set; } = "notChosen";
    public int nbAI { get; private set; }= 0;

    public void ChooseCar1()
    {
        chosenCar = "FoxBody";
    }
    
    public void ChooseCar2()
    {
        chosenCar = "TrophyTruck";
    }
    
    public void ChooseCar3()
    {
        chosenCar = "FocusRallyV1";
    }
    
    public void ChooseCar4()
    {
        chosenCar = "FocusRallyV2";
    }

    public void Choose1AI()
    {
        nbAI = 1;
    }
    
    public void Choose2AI()
    {
        nbAI = 2;
    }
    
    public void Choose3AI()
    {
        nbAI = 3;
    }
}
