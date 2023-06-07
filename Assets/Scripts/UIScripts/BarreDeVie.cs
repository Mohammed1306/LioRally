//Script écrit par Frédéric Gaurdreault

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BarreDeVie : MonoBehaviour
{
    public GameObject véhicule;
    private GameObject vehiculePrécédent;
    [FormerlySerializedAs("yellowThreshold")] [SerializeField] private float seuilJaune;
    [FormerlySerializedAs("redThreshold")] [SerializeField] private float seuilRouge;
    private Image image;
    private Health vie;
    private RectTransform barreDeVie;
    
    private float vieRatio;

    private void Awake()
    {
        barreDeVie = transform.Find("BarreDeVie").GetComponent<RectTransform>();
        image = transform.Find("BarreDeVie").GetComponent<Image>();
    }

    private void Start()
    {
        vie = véhicule.GetComponent<Health>();
        vehiculePrécédent = véhicule;
    }

    private void Update()
    {   
        // Le script EndRace change le véhicule cible lorsque le joueur franchit la ligne d'arrivé
        if (vehiculePrécédent == véhicule) // Le véhicule cible n'a pas changé
        {
            vieRatio = vie.HealthRatio();
            barreDeVie.localScale = new Vector3(1, vieRatio,1); // niveau de la barre de vie
            
            // définir la couleur de la barre de vie en fonction de l'état du véhicule
            if (vieRatio < seuilJaune)
                image.color = vieRatio < seuilRouge ? new Color(1, 0, 0) : new Color(1, 1, 0);
            else
                image.color = new Color(0, 1, 0);
        }
        else // On change de véhicule cible!
        {
            vie = véhicule.GetComponent<Health>();
            vehiculePrécédent = véhicule;
        }
    }
}
