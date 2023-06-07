// Écrit par Frédéric Gaudreault
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvolerObjet : MonoBehaviour
{
    public Transform objet; // objet que l'on survol
    public float hauteur; // hauteur à laquelle on survole l'objet

    private void Update()
    {
        // Survoler l'objet à la bonne hauteur
        transform.position = objet.position + new Vector3(0, hauteur, 0);
        // Pointer dans la même direction que l'objet survolé
        transform.rotation = Quaternion.Euler(new Vector3(0,objet.rotation.eulerAngles.y,0));
    }
}
