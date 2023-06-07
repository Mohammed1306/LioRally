//Script écrit par Frédéric Gaudreault

using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class WheelComponent : MonoBehaviour
{
    public bool estMotorisée;
    public bool estDirigée;
    public bool estFreinAMain;

    private WheelCollider wc;
    private SteerComponent sc;
    private TransmissionComponent tc;

    // Méthodes responsables d'appliquer les forces et mouvements aux WheelColliders
    private void AppliquerFrein(float cf) => wc.brakeTorque = cf;
    private void AppliquerCouple(float cm) => wc.motorTorque = cm;
    private void AppliquerDirection(float a) => wc.steerAngle = a;
    


    private void Awake()
    {
        wc = GetComponent<WheelCollider>();
        sc = GetComponentInParent<SteerComponent>();
        tc = GetComponentInParent<TransmissionComponent>();
    }

    private void Update()
    {
        if(estDirigée) //la roue fait partie du système de direction
            AppliquerDirection(sc.angleRoues);
        
        if(estMotorisée) // la roue est connectée au moteur
            AppliquerCouple(tc.cm);
        
        // appliquer le "frein moteur" à toutes les roues et le frein à main aux roues arrière
        AppliquerFrein(tc.cf + (wc.rpm > 0.5f ? tc.fm : 0) + (estFreinAMain ? sc.cfam : 0));
    }
}
