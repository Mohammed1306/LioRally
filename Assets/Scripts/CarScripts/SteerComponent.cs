//Script écrit par Frédéric Gaudreault

using UnityEngine;

public class SteerComponent : MonoBehaviour
{
    
    public bool estJoueur;
    [SerializeField] private float angleParSecondes = 200;
    [SerializeField] private float angleMax = 30;
    [SerializeField] private float coupleFreinAMain = 10000;
    [HideInInspector] public float angleRoues;
    [HideInInspector] public float cfam; // Couple frein à main
    private float addition;
    private static int[] directions = {1,-1};
    private int directionGD;
    [HideInInspector] public bool estFreinAMainActif;
    
    
    public void InputDirectionRoue(int indexDirection) => directionGD += directions[indexDirection];
    
    // Le paramètre est inutile à la fonction, mais utile à d'autre endroit: IL FAUT LE GARDER
    public void InputFreinAMain(int param) => estFreinAMainActif = true;
    
    private void Awake()
    {
        directionGD = 0;
        angleRoues = 0;
    }

    private void Update()
    {
        // Appliquer direction des roues
        if (estJoueur) // Dans le cas qu'un joueur est aux commandes
        {
            if (directionGD == 0) // Retour au centre
            {
                if (Mathf.Abs(angleRoues) > 1)
                    directionGD += angleRoues < 0 ? 1 : -1;
                else 
                    angleRoues = 0;
            }
            addition = directionGD * angleParSecondes * Time.deltaTime; // Tourner les roues à la bonne vitesse
            if (Mathf.Abs(angleRoues + addition) < angleMax) // Ne pas dépasser angleMax
                angleRoues += addition;
        }
        else // Dans le cas qu'un ordinateur est aux commandes
        {
            directionGD = angleRoues > 0 ? 1 : -1;
            if (Mathf.Abs(angleRoues) > angleMax) // Ne pas dépasser angleMax
                angleRoues = angleMax * directionGD;
        }
        
        if (estFreinAMainActif) // Appliquer frein à main
        {
            cfam = coupleFreinAMain;
            estFreinAMainActif = false;
        }
        else
            cfam = 0;
        
        directionGD = 0;
    }
}
