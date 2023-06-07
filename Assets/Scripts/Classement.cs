//Écrit par Thomas Prévost, Mohamed Abdellatif Kallel
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Classement : MonoBehaviour
{
    private DistanceCalculator[] voitures;
    private UIClassement ui;
    //private int index = 0;
    //private int oldIndex = 0;
    private IDictionary<string, float> pilotesDeCourse;
    public IDictionary<string, float> classementFinal { get; private set; }
    public string focus = "Joueur";
    public bool joueurEnCourse = true;

    private void Start()
    {
        voitures = FindObjectsOfType<DistanceCalculator>();
        ui = GetComponent<UIClassement>();
        pilotesDeCourse = new Dictionary<string, float>();

        for (int i = 0; i < voitures.Length; i++)
            pilotesDeCourse.Add(voitures[i].name, voitures[i].GetDistance());
    }

    private void Update()
    {
        for (int i = 0; i < voitures.Length; i++)
        {
            pilotesDeCourse[voitures[i].name] = voitures[i].GetDistance();
        }

        classementFinal =
            pilotesDeCourse.OrderBy(x => x.Value).Reverse()
                .ToDictionary(x => x.Key, x => x.Value); //Ligne prit en ligne sur un forum
        if (joueurEnCourse)
        {
            ui.ChangerClassement(classementFinal.Keys.ToList().IndexOf(focus));
        }
        else
        {
            ui.ChangerClassement(classementFinal.Keys.ToList().IndexOf(focus)+1);
        }
    }
}
