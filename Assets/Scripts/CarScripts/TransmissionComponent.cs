//Thomas Prévost et Frédéric Gaudreault

using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MotorComponent))]
[RequireComponent(typeof(Rigidbody))]
public class TransmissionComponent : MonoBehaviour
{
    public Vector3 centreDeMasse;
    [HideInInspector] public bool estMêmeDirection;
    [HideInInspector] public float cm; // Couple moteur   varibles utilisés par les WheelComponent
    [HideInInspector] public float fm; // Frein moteur
    [HideInInspector] public float cf; // Couple frein
    [FormerlySerializedAs("speed")] [HideInInspector] public double vitesse;
    
    private Rigidbody rb;
    private MotorComponent mc;

    private int directionAA;
    private static int[] directions = {1,-1};
    
    [Header("Éléments ajustables")]
    [SerializeField] private float freinMoteur = 50;
    [SerializeField] private float coupleFrein = 500;

    public void InputDirectionVoiture(int indexDirection) => directionAA += directions[indexDirection];

    
    
    private void Awake()
    {
        directionAA = 0;
        rb = GetComponent<Rigidbody>();
        mc = GetComponent<MotorComponent>();
        rb.centerOfMass = centreDeMasse; // définit le centre de masse du véhicule
    }

    private void Update()
    {
        // vérifie si le véhicule se dirige dans la même diection que "l'input" du joueur.
        estMêmeDirection = directionAA * rb.transform.InverseTransformDirection(rb.velocity).z > -1;

        mc.ControleCoupleMoteur(estMêmeDirection ? directionAA : 0); // Dire au moteur si on ajoute de la puissance
        cf = !estMêmeDirection ? coupleFrein : 0;                    // Appliquer les freins lorsque directions opposées
        cm = estMêmeDirection ? directionAA * mc.coupleMoteur : 0;   // Appliquer le couple du moteur lorsque même direction
        fm = directionAA == 0 ? freinMoteur : 0;                     // Appliquer le frein moteur lorsque no input
        directionAA = 0;
        vitesse = rb.velocity.magnitude * 3.6; // Vitesse en km/h
    }
}
