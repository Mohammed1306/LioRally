//Écrit par Thomas Prévost
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndRace : MonoBehaviour
{
    [SerializeField] private RawImage minimap;
    [SerializeField]private TextMeshProUGUI countdownToPodium;    
    [SerializeField] private TextMeshProUGUI classementUI;
    [FormerlySerializedAs("texteFinal1")] [SerializeField] private TextMeshProUGUI finalText1;
    [FormerlySerializedAs("texteFinal2")] [SerializeField] private TextMeshProUGUI finalText2;
    [FormerlySerializedAs("texteFinal3")] [SerializeField] private TextMeshProUGUI finalText3;
    [FormerlySerializedAs("texteFinal4")] [SerializeField] private TextMeshProUGUI finalText4;
    private List<GameObject> oldTriggers;
    private GameObject newFocus;
    private GameObject canvas;
    private NeedleMovement revIndicator;
    private CompteurVitesse speedometer;
    private BarreDeVie lifeBar;
    private DamageBlinker damageBlinker;
    private PlayerCameraMovement pcm;
    private Classement ranking;
    private bool canShowRanking;
    private bool countdownStarted;

    private void Awake()
    {
        ranking = FindObjectOfType<Classement>();
        pcm = FindObjectOfType<PlayerCameraMovement>();
        canvas = GameObject.Find("Canvas");
        revIndicator = canvas.GetComponent<NeedleMovement>();
        speedometer = canvas.GetComponent<CompteurVitesse>();
        lifeBar = canvas.GetComponent<BarreDeVie>();
        damageBlinker = canvas.GetComponent<DamageBlinker>();
        oldTriggers = new List<GameObject>(2);
        classementUI.enabled = false;
        finalText1.enabled = false;
        finalText2.enabled = false;
        finalText3.enabled = false;
        finalText4.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        int position;
        GameObject véhicule = other.transform.parent.gameObject;
        
        if (!oldTriggers.Contains(véhicule))
        {
            oldTriggers.Add(véhicule);
            canShowRanking = true;
            position = ranking.classementFinal.Keys.ToList().IndexOf(véhicule.name);
            CalculateurTemps ct = véhicule.GetComponent<CalculateurTemps>();
            ct.courseTerminé = true;
            if (véhicule.name == "Joueur")
            {
                newFocus = véhicule;
                minimap.gameObject.SetActive(false);
                ranking.joueurEnCourse = false;
            }

            if (newFocus != null && position != ranking.classementFinal.Count - 1)
            {
                pcm.enabled = false;
                ranking.focus = newFocus.name;
                //Désactiver les caméras du joueur
                foreach (var cam in newFocus.GetComponentsInChildren<Camera>())
                    cam.gameObject.SetActive(false);

                //Activer les caméras du AI qui est juste après le joueur dans le classement
                newFocus = GameObject.Find($"{ranking.classementFinal.ElementAt(position + 1).Key}");
                newFocus.transform.Find("ListeCameras").Find("CameraSuivreDerrière").gameObject.SetActive(true);

                // Attribuer le nouveau véhicule aux différents indicateurs du UI
                revIndicator.vehicle = newFocus;
                speedometer.véhicule = newFocus;
                lifeBar.véhicule = newFocus;
                damageBlinker.vehicle = newFocus;
            }

            if (!finalText1.enabled&&canShowRanking)
            {
                ct.positionFinal = 0;
                finalText1.enabled = true;
                classementUI.gameObject.SetActive(true);
                finalText1.gameObject.SetActive(true);
                finalText1.text =
                    $"1er     {véhicule.name}     {TimeSpan.FromSeconds(véhicule.GetComponent<CalculateurTemps>().TempsCourse).ToString()}";
                canShowRanking = false;
            }

            if (!finalText2.enabled&&canShowRanking)
            {
                ct.positionFinal = 1;
                finalText2.enabled = true;
                finalText2.gameObject.SetActive(true);
                finalText2.text =
                    $"2e     {véhicule.name}     {TimeSpan.FromSeconds(véhicule.GetComponent<CalculateurTemps>().TempsCourse).ToString()}";
                canShowRanking = false;
            }

            if (!finalText3.enabled&&canShowRanking)
            {
                ct.positionFinal = 2;
                finalText3.enabled = true;
                finalText3.gameObject.SetActive(true);
                finalText3.text =
                    $"3e     {véhicule.name}     {TimeSpan.FromSeconds(véhicule.GetComponent<CalculateurTemps>().TempsCourse).ToString()}";
                canShowRanking = false;
            }

            if (!finalText4.enabled&&canShowRanking)
            {
                ct.positionFinal = 3;
                finalText4.enabled = true;
                finalText4.gameObject.SetActive(true);
                finalText4.text =
                    $"4e     {véhicule.name}     {TimeSpan.FromSeconds(véhicule.GetComponent<CalculateurTemps>().TempsCourse).ToString()}";
                canShowRanking = false;
            }

            if (position == ranking.classementFinal.Count - 1 && !countdownStarted)
            {
                pcm.enabled = false;
                countdownStarted = true;
                StartCoroutine(CoutdownToPodium(3));
            }
            else if (véhicule.name == "Joueur" && !countdownStarted)
            {
                countdownStarted = true;
                StartCoroutine(CoutdownToFreeze(2,véhicule));
                StartCoroutine(CoutdownToPodium(10));
            }
        }
    }

    public IEnumerator CoutdownToPodium(int countdownTime)
    {
        countdownToPodium.gameObject.SetActive(true);
        while (countdownTime >= 1)
        {
            countdownToPodium.text = countdownTime.ToString();

            yield return new WaitForSecondsRealtime(1);

            countdownTime--;
        }

        countdownToPodium.text = "0";

        yield return new WaitForSeconds(0.5f);
        
        countdownToPodium.gameObject.SetActive(false);
        SceneManager.LoadScene(4);
    }

    public IEnumerator CoutdownToFreeze(int countdownTime,GameObject car)
    {
        countdownToPodium.gameObject.SetActive(true);
        while (countdownTime >= 1)
        {
            yield return new WaitForSecondsRealtime(1);

            countdownTime--;
        }

        car.GetComponent<Rigidbody>().isKinematic = true;
    }
}
