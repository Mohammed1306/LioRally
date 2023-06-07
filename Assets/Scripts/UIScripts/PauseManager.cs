using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

//Écrit par Charles Trottier, Thomas Prévost
public class PauseManager : MonoBehaviour
{
    private KeyCode pauseButton = KeyCode.Escape;
    private GameObject pauseCanevas;
    private bool isVisible;
    
        
    private int countdownTime = 3;
    private bool notVisible = false;
    [SerializeField] public TextMeshProUGUI countdownDisplay;
    private StartGame startGame;

    private DontDestroy[] cars;
    private string chosenMap;


    private void Awake()
    {
        countdownDisplay.gameObject.SetActive(notVisible);
        pauseCanevas = GameObject.Find("PauseCanevas");
        isVisible = false;
        pauseCanevas.gameObject.SetActive(isVisible);
        startGame = GameObject.Find("StartGame").GetComponent<StartGame>();
        chosenMap = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        //Afficher le canevas de menu pause
        if (Input.GetKey(pauseButton))
        {
            PauseGame();
            Invoke("CanevasVisible",0f);
        }
    }
    //Mettre l'entièreté de la scène en pause
    private void PauseGame()
    {
        Time.timeScale = 0;
        isVisible = true;
    }
    //Afficher le décompte pour ensuite remetre la scène active
    public void ResumeGame()
    {
        
        StartCoroutine(startGame.CountDownToStart());
        isVisible = false;
        pauseCanevas.gameObject.SetActive(isVisible);
    }
    //Détruire les autos existantes et recommencer une nouvelle partie
    public void RestartGame()
    {
        cars = FindObjectsOfType<DontDestroy>();
        foreach (var car in cars)
        {
            Destroy(car.gameObject);
        }
        SceneManager.LoadScene(chosenMap);
        Time.timeScale = 1;
    }
    //Détruire les autos existantes et revenir à la scène d'introduction
    public void QuitGame()
    {
        cars = FindObjectsOfType<DontDestroy>();
        foreach (var car in cars)
        {
            Destroy(car.gameObject);
        }
        SceneManager.LoadScene("IntroScene");
    }
    
    private void CanevasVisible()
    {
        pauseCanevas.gameObject.SetActive(isVisible);
    }

}