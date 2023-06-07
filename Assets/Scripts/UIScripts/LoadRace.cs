using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Script fait par Mohammed Abdellatif Kallel

public class LoadRace : MonoBehaviour
{
    public GameObject SceneMenu; 
        
    public void LoadLevel()
    {
        if (GetComponent<ChoicesMenu>().chosenCar != "notChosen" && GetComponent<ChoicesMenu>().nbAI != 0)
        {
            SceneManager.LoadScene(SceneMenu.GetComponent<MapChoiceMenu>().chosenScene);
            //Debug.Log($"la map, la voiture et les AI sont choisies: {GetComponent<ChoicesMenu>().chosenCar} {GetComponent<ChoicesMenu>().nbAI}");
        }
    }
}
