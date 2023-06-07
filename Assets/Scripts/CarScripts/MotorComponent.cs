using UnityEngine;
using UnityEngine.Serialization;

//Travaillé par Charles Trottier, Fred Gaudreault, Thomas Prévost
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
public class MotorComponent : MonoBehaviour
{
    [Header("Coefficients du moteur")]
    [SerializeField] private float augCoupleParSec;
    [SerializeField] private float dimCoupleParSec;
    
    [Header("Limites du moteur")]
    [SerializeField] public float maxCoupleAvant;
    [SerializeField] private float maxCoupleArrière;
    [FormerlySerializedAs("coupleIdle")] [SerializeField] private float coupleInactif;
    
    [Header("Couple du Moteur")] public float coupleMoteur;
    
    private Health vie;
    private float vieRatio;

    private AiSpeedManager AiVitesse;
    private float distanceRatio = 1;

    
    
    private void Start()
    {
        vie = GetComponent<Health>(); 
        coupleMoteur = coupleInactif;
        AiVitesse = GetComponent<AiSpeedManager>();
    }
    
    
    
    public void ControleCoupleMoteur(int dir)
    { 
        vieRatio = vie.HealthRatio();
        
        if(name != "Joueur")
            distanceRatio = AiVitesse.distanceRatio;


        if (dir == 0 && coupleMoteur > coupleInactif) // Assurer que le moteur ne diminue pas en-dessous d'un certain régime.
        {
            coupleMoteur -= dimCoupleParSec * Time.deltaTime;
        }
        else if ((dir > 0 && coupleMoteur < maxCoupleAvant * vieRatio && coupleMoteur < maxCoupleAvant * distanceRatio) || // Couple max en avançant
                 (dir < 0 && coupleMoteur < maxCoupleArrière * vieRatio && coupleMoteur < maxCoupleAvant * distanceRatio)) // Couple max en reculant
        {
            coupleMoteur += augCoupleParSec * Time.deltaTime * distanceRatio;
        }
    }
}
