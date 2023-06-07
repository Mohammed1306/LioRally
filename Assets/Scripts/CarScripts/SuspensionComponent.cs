//Script écrit par Frédéric Gaudreault

using UnityEngine;

public class SuspensionComponent : MonoBehaviour
{
    [Header("Ressort à l'état initial")]
    [SerializeField] private float springInitial;
    [SerializeField] private float damper;
    [SerializeField] private float targetPosition;
    
    [Header("Ressort lorsque comprimé")]
    [SerializeField] private float springFinal;
    
    private WheelCollider[] wcs;
    private Vector3 pos;
    private Quaternion rot;
    private JointSpring js;
    private float h;
    
    
    
    void Awake()
    {
        wcs = GetComponentsInChildren<WheelCollider>();

        // Attribuer l'état initial de la suspension
        js = new JointSpring();
        js.spring = springInitial; 
        js.damper = damper;
        js.targetPosition = targetPosition;
    }
    
    void Update()
    {
        wcs[0].GetWorldPose(out pos, out rot);
        h = wcs[0].suspensionDistance / 2 - (pos - wcs[0].transform.position).magnitude;
        DéfinirDuretéRessort(0);
        
        wcs[1].GetWorldPose(out pos, out rot);
        h = wcs[1].suspensionDistance / 2 - (pos - wcs[1].transform.position).magnitude;
        DéfinirDuretéRessort(1);
        
        wcs[2].GetWorldPose(out pos, out rot);
        h = wcs[2].suspensionDistance / 2 - (pos - wcs[2].transform.position).magnitude;
        DéfinirDuretéRessort(2);
        
        wcs[3].GetWorldPose(out pos, out rot);
        h = wcs[3].suspensionDistance / 2 - (pos - wcs[3].transform.position).magnitude;
        DéfinirDuretéRessort(3);
    }
    
    
    // Méthode pour augmenter la dureté du ressort s'il est trop comprimé
    private void DéfinirDuretéRessort(int indice)
    {
        if (h > 0.1f) // Si le ressort n'est pas trop comprimé
        {
            // Si le ressort est trop raide, on réattribue la valeur de base
            if (wcs[indice].suspensionSpring.spring > springInitial + 1) // +1, car float imprécis
            { 
                js.spring = springInitial;
                wcs[indice].suspensionSpring = js;
            }
        }
        else // Si le ressort est très comprimé
        {
            // on augmente la dureté de façon progressive (simule un "bump stop")
            js.spring = springInitial + 10 * (0.1f - h) * (springFinal - springInitial);
            wcs[indice].suspensionSpring = js;
        }
    }
}
