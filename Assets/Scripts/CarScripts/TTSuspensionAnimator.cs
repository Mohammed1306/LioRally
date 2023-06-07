//Script écrit par Frédéric Gaudreault.

using UnityEngine;

public class TTSuspensionAnimator : MonoBehaviour
{
    // valeurs et références aux roues
    private readonly Vector3[] startWcsPos = new Vector3[4];
    private readonly Transform[] wheels = new Transform[4];
    private WheelCollider[] wcs;
    
    // éléments visuels de la suspension
    private Transform frArm;
    private Transform flArm;
    private Transform rrArm;
    private Transform rlArm;
    private Transform axle;

    private Transform frame;
    private Transform frSpring;
    private Transform flSpring;
    private Transform rrSpring;
    private Transform rlSpring;

    // variables utilisées pour la rotation des roues
    private float rpmInDeg;
    private float deg0;
    private float deg1;
    private float deg2;
    private float deg3;
    
    // variables utilisées pour animer la suspension
    private float angle;
    private float distance;
    private float h2;
    private float h3;
    private float f2;
    private float f3;
    private Vector3 pos;
    private Vector3 pos3;
    private Quaternion rot;

    private void Awake()
    {
        // Référence au Chassis
        frame = transform;
        
        // Références aux WheelCollider
        wcs = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < wcs.Length; ++i)
        {
            // Références aux positions d'origine (position au repos) des WheelCollider 
            startWcsPos[i] = wcs[i].gameObject.transform.localPosition;
            // Références aux roues visuelles (mesh)
            wheels[i] = wcs[i].gameObject.transform.GetChild(0);  
        }

        // Références aux différents éléments visibles de la suspension
        frArm = transform.Find("FRArm");
        flArm = transform.Find("FLArm");
        rrArm = transform.Find("RRArm");
        rlArm = transform.Find("RLArm");
        axle = transform.Find("Axle");
        frSpring = transform.Find("FRSpring"); 
        flSpring = transform.Find("FLSpring");  
        rrSpring = transform.Find("RRSpring"); 
        rlSpring = transform.Find("RLSpring"); 

        // Les angles pour la rotation des roues visibles
        rpmInDeg = 6 * Time.deltaTime; // conversion de rpm en degrés
        deg0 = 0;
        deg1 = 0;
        deg2 = 0;
        deg3 = 0;
    }
    
    
    
    private void Update()
    {
        // Pour l'avant droit :
        wcs[0].GetWorldPose(out pos, out rot); // Valeurs initiales
        distance = (wcs[0].transform.position - frArm.position).magnitude;
        angle = Mathf.Atan(frame.InverseTransformDirection(wcs[0].transform.position - pos).y / distance);
        
        frArm.localRotation = Quaternion.Euler(-180, 0, Mathf.Rad2Deg * angle); // Anime le bras de suspension
        wcs[0].transform.localPosition = startWcsPos[0] - new Vector3(distance * (1 - Mathf.Cos(angle)),0,0);

        frSpring.localScale = new Vector3(1,1 + 0.7f * angle,1); // Anime le ressort
        DegWheel(ref deg0, wcs[0]);
        
        wcs[0].GetWorldPose(out pos, out rot); // Anime la roue visuelle (mesh)
        wheels[0].position = pos + frArm.TransformVector(new Vector3(0.1f,0,0));
        wheels[0].localRotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(180 + wcs[0].steerAngle,new Vector3(0,1,0)) 
                                  * Quaternion.AngleAxis(-deg0,new Vector3(1,0,0));


        
        // Pour l'avant gauche :
        wcs[1].GetWorldPose(out pos, out rot); // Valeurs initiales
        distance = (wcs[1].transform.position - flArm.position).magnitude;
        angle = Mathf.Atan(frame.InverseTransformDirection(wcs[1].transform.position - pos).y / distance);
        
        flArm.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle); // Anime la suspension
        wcs[1].transform.localPosition = startWcsPos[1] + new Vector3(distance * (1 - Mathf.Cos(angle)),0,0);
        
        flSpring.localScale = new Vector3(1,1 + 0.7f * angle,1); // Anime le ressort
        DegWheel(ref deg1, wcs[1]);
        
        wcs[1].GetWorldPose(out pos, out rot); // Anime la roue visuelle (mesh)
        wheels[1].position = pos + flArm.TransformVector(new Vector3(0.1f,0,0));
        wheels[1].localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(wcs[0].steerAngle,new Vector3(0,1,0)) 
                                  * Quaternion.AngleAxis(deg1,new Vector3(1,0,0));
        

        
        // Pour l'essieu arrière :
        wcs[2].GetWorldPose(out pos, out rot); // Valeurs initiales
        wcs[3].GetWorldPose(out pos3, out rot);
        h2 = frame.InverseTransformDirection(pos - wcs[2].transform.position).y;
        h3 = frame.InverseTransformDirection(pos3 - wcs[3].transform.position).y;
        f2 = 1.6f * (1 - Mathf.Cos(Mathf.Atan(h2 / 1.4f)));
        f3 = 1.6f * (1 - Mathf.Cos(Mathf.Atan(h3 / 1.4f)));
        distance = (wcs[3].transform.position - wcs[2].transform.position).magnitude;
        angle = Mathf.Atan((h2 - h3) / distance);
        
        axle.localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                             * Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan((f2 - f3) / distance),new Vector3(0,1,0)); // Anime la suspension
        axle.localPosition = new Vector3(0,0.4f + h2 + (h3 - h2) / 2 ,-1.6f + f2 + (f3 - f2) / 2);
        wcs[2].transform.localPosition = startWcsPos[2] + new Vector3(distance / 2 * (1 - Mathf.Cos(angle)),0,f2);
        wcs[3].transform.localPosition = startWcsPos[3] - new Vector3(distance / 2 * (1 - Mathf.Cos(angle)),0,-f3);

        DegWheel(ref deg2, wcs[2]);
        DegWheel(ref deg3, wcs[3]);
        
        wcs[2].GetWorldPose(out pos, out rot); // Anime les roues visuelles (mesh)
        wheels[2].position = pos + axle.TransformVector(new Vector3(-0.1f,0,0));
        wheels[2].localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(180,new Vector3(0,1,0)) 
                                  * Quaternion.AngleAxis(-deg2,new Vector3(1,0,0));
        
        wcs[3].GetWorldPose(out pos3, out rot);
        wheels[3].position = pos3 + axle.TransformVector(new Vector3(0.1f,0,0));
        wheels[3].localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(deg3,new Vector3(1,0,0));
        
        
        
        // Pour le bras de suspension arrière droit :
        angle = Mathf.Atan(h2 / 1.4f); // Valeur initiale
        rrArm.localRotation = Quaternion.AngleAxis(-180 + Mathf.Rad2Deg * angle,new Vector3(1,0,0)); // Anime la suspension
        rrSpring.localScale = new Vector3(1,1.5f - angle,1); // Anime le ressort
        
        
        
        // Pour le bras de suspension arrière gauche :
        angle = Mathf.Atan(h3 / 1.4f); // Valeur initiale
        rlArm.localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle,new Vector3(1,0,0)); // Anime la suspension
        rlSpring.localScale = new Vector3(1,1.5f - angle,1); // Anime le ressort
    }
    
    // Méthode qui définit la rotation des roues (celle générée par le moteur)
    private void DegWheel(ref float deg, WheelCollider wc)
    {
        deg += wc.rpm * rpmInDeg;
        if (deg > 360)
            deg -= 360;
    }
}
