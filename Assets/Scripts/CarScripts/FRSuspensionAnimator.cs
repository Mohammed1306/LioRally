// Script écrit par Frédéric Gaudreault

using UnityEngine;

public class FRSuspensionAnimator : MonoBehaviour
{
    private readonly Vector3[] startWcsPos = new Vector3[4];
    private readonly Transform[] wheels = new Transform[4];
    private WheelCollider[] wcs;
    
    private Transform frArm;
    private Transform flArm;
    private Transform rrArm;
    private Transform rlArm;
    
    private Transform frame;
    private Transform frSpring;
    private Transform flSpring;
    private Transform rrSpring;
    private Transform rlSpring;
    
    private float rpmInDeg;
    private float deg0;
    private float deg1;
    private float deg2;
    private float deg3;
    
    private float angle;
    private float distance;
    private Vector3 pos;
    private Quaternion rot;
    
    void Awake()
    {
        // Obtenir le Chassis
        frame = transform;
        
        // Obtenir les WheelCollider
        wcs = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < wcs.Length; ++i)
        {
            // Obtenir la position d'origine des WheelCollider
            startWcsPos[i] = wcs[i].gameObject.transform.localPosition;
            // Obtenir les roues visuelles 
            wheels[i] = wcs[i].gameObject.transform.GetChild(0);  
        }

        // Obtenir les différents éléments visibles de la suspension
        frArm = transform.Find("FRArm");
        flArm = transform.Find("FLArm");
        rrArm = transform.Find("RRArm");
        rlArm = transform.Find("RLArm");
        frSpring = transform.Find("FRSpring"); 
        flSpring = transform.Find("FLSpring");  
        rrSpring = transform.Find("RRSpring"); 
        rlSpring = transform.Find("RLSpring"); 

        // Les angles pour la rotation des roues visibles
        deg0 = 0;
        deg1 = 0;
        deg2 = 0;
        deg3 = 0;
    }
    
    
    
    void Update()
    {
        rpmInDeg = 6 * Time.deltaTime;
        
        
        
        // Pour l'avant droit :
        wcs[0].GetWorldPose(out pos, out rot); // Valeurs initiales
        distance = (wcs[0].transform.position - frArm.position).magnitude;
        angle = Mathf.Atan(frame.InverseTransformDirection(wcs[0].transform.position - pos).y / distance);
        
        frArm.localRotation = Quaternion.Euler(-180, 0, Mathf.Rad2Deg * angle); // Anime la suspension
        wcs[0].transform.localPosition = startWcsPos[0] - new Vector3(distance * (1 - Mathf.Cos(angle)),0,0);

        frSpring.localScale = new Vector3(0.5f,0.7f + 0.7f * angle,0.5f); // Anime le ressort
        DegWheel(ref deg0, wcs[0]); // gestion de la rotation de la roue

        wcs[0].GetWorldPose(out pos, out rot); // Anime la roue visuelle (mesh)
        wheels[0].position = pos + frArm.TransformVector(new Vector3(0.07f,0,0));
        wheels[0].localRotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(180 + wcs[0].steerAngle,new Vector3(0,1,0)) 
                                  * Quaternion.AngleAxis(-deg0,new Vector3(1,0,0));


        
        // Pour l'avant gauche :
        wcs[1].GetWorldPose(out pos, out rot); // Valeurs initiales
        distance = (wcs[1].transform.position - flArm.position).magnitude;
        angle = Mathf.Atan(frame.InverseTransformDirection(wcs[1].transform.position - pos).y/ distance);
        
        flArm.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle); // Anime la suspension
        wcs[1].transform.localPosition = startWcsPos[1] + new Vector3(distance * (1 - Mathf.Cos(angle)),0,0);
        
        flSpring.localScale = new Vector3(0.5f,0.7f + 0.7f * angle,0.5f); // Anime le ressort
        DegWheel(ref deg1, wcs[1]); // gestion de la rotation de la roue
        
        wcs[1].GetWorldPose(out pos, out rot); // Anime la roue visuelle (mesh)
        wheels[1].position = pos + flArm.TransformVector(new Vector3(0.07f, 0, 0));
        wheels[1].localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(wcs[0].steerAngle,new Vector3(0,1,0)) 
                                  * Quaternion.AngleAxis(deg0,new Vector3(1,0,0));
        
        
        
        // Pour l'arrière droit :
        wcs[2].GetWorldPose(out pos, out rot); // Valeurs initiales
        distance = (wcs[2].transform.position - rrArm.position).magnitude;
        angle = Mathf.Atan(frame.InverseTransformDirection(wcs[2].transform.position - pos).y / distance);
        
        rrArm.localRotation = Quaternion.Euler(-180, 0, Mathf.Rad2Deg * angle); // Anime la suspension
        wcs[2].transform.localPosition = startWcsPos[2] - new Vector3(distance * (1 - Mathf.Cos(angle)),0,0);

        rrSpring.localScale = new Vector3(0.5f,0.7f + 0.7f * angle,0.5f); // Anime le ressort
        DegWheel(ref deg2, wcs[2]); // gestion de la rotation de la roue
        
        wcs[2].GetWorldPose(out pos, out rot); // Anime la roue visuelle (mesh)
        wheels[2].position = pos + rrArm.TransformVector(new Vector3(0.07f,0,0));
        wheels[2].localRotation = Quaternion.AngleAxis(-Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(180,new Vector3(0,1,0)) 
                                  * Quaternion.AngleAxis(-deg2,new Vector3(1,0,0));


        
        // Pour l'arrière gauche :
        wcs[3].GetWorldPose(out pos, out rot); // Valeurs initiales
        distance = (wcs[3].transform.position - rlArm.position).magnitude;
        angle = Mathf.Atan(frame.InverseTransformDirection(wcs[3].transform.position - pos).y / distance);
        
        rlArm.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle); // Anime la suspension
        wcs[3].transform.localPosition = startWcsPos[3] + new Vector3(distance * (1 - Mathf.Cos(angle)),0,0);
        
        rlSpring.localScale = new Vector3(0.5f,0.7f + 0.7f * angle,0.5f); // Anime le ressort
        DegWheel(ref deg3, wcs[3]); // gestion de la rotation de la roue
        
        wcs[3].GetWorldPose(out pos, out rot); // Anime la roue visuelle (mesh)
        wheels[3].position = pos + rlArm.TransformVector(new Vector3(0.07f, 0, 0));
        wheels[3].localRotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle,new Vector3(0,0,1)) 
                                  * Quaternion.AngleAxis(deg3,new Vector3(1,0,0));
    }
    
    private void DegWheel(ref float deg, WheelCollider wc)
    {
        deg += wc.rpm * rpmInDeg;
        if (deg > 360)
            deg -= 360;
    }
}
