//Écrit par Thomas Prévost
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class UIClassement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texte;
    private float compteur;

    public string ChangerClassement(int index)
    {
        index += 1;
        return texte.text = index == 1 ? $"{index}er" : $"{index}e";
    }
}
