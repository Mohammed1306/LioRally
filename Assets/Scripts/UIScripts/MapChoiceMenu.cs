//Mohamed Abdellatif Kallel
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChoiceMenu : MonoBehaviour
{
    [HideInInspector] public string chosenScene;
    public void LoadScene1()
    {
        chosenScene = "MAP01";
    }
    
    public void LoadScene2()
    {
        chosenScene = "MAP02";
    }
    
    public void LoadScene3()
    {
        chosenScene = "MAP03";
    }

    public void LoadRandomScene()
    {
        var rnd = new System.Random();
        chosenScene = $"MAP0{rnd.Next(1,4)}";
    }
}
