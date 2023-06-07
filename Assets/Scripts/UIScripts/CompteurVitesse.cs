//Script écrit par Frédéric Gaudreault

using System.Globalization;
using TMPro;
using UnityEngine;

public class CompteurVitesse : MonoBehaviour
{
    public GameObject véhicule;
    private GameObject vehiculePrécédent;
    private TransmissionComponent tc;
    private TextMeshProUGUI tmp;

    private void Awake() => tmp = transform.Find("IndicateurDeVitesse").GetComponent<TextMeshProUGUI>();

    private void Start()
    {
        tc = véhicule.GetComponent<TransmissionComponent>();
        vehiculePrécédent = véhicule;
    }

    void Update()
    {
        // Le script EndRace change le véhicule cible lorsque le joueur franchit la ligne d'arrivé
        if (vehiculePrécédent == véhicule) // le véhicule cible n'a pas changé
        {
            tmp.text = tc.vitesse.ToString("F0",CultureInfo.InvariantCulture) + "\nKm/h";
        }
        else // On change de véhicule cible!
        {
            tc = véhicule.GetComponent<TransmissionComponent>();
            vehiculePrécédent = véhicule;
        }
    } 
}
