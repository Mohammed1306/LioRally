//Script écrit par Thomas Prévost, Charles Trottier et Frédéric Gaudreault

using UnityEngine;

public class AIHandler : MonoBehaviour
{
    // Variables concernant les waypoints :
    public float décélération;
    private WaypointManager wm;
    private Vector3[] positionsWaypoint;
    private Vector3 directionDuWaypoint;
    private float[] distanceMinimumWaypoints;
    private float[] vitesseMaxÀCesWaypoints;
    private float distanceDuWaypoint;
    private int nbWaypoints;
    private int indexWaypointActuel;

    // Varaibles concernant le contrôle du véhicule :
    private TransmissionComponent tc;
    private SteerComponent sc;
    private Recovery recovery;
    private Rigidbody rigidbodyVéhicule;
    private Transform transformVéhicule;
    private Vector3 positionVéhicule;
    private Vector3 directionDevantVéhicule;
    private Vector3 vélocitéVéhicule;
    private double vitesseVéhicule;
    private float angle;
    private float tempsÉcouléEnMarcheArrière;
    private float délaiMarcheArrière;
    private bool doitAccélérer = true;

    // Variables concernant les raycasts :
    [SerializeField] private float distanceAvantCollision = 10f;
    [SerializeField] private int nbRays; //Nombre de rayons qui seront projetés vers l'avant du véhicule
    private BoxCollider origineRaycasts;
    private RaycastHit[] hitsInfo;
    private RaycastHit hit;
    private Vector3[] raycastVectors;
    private Vector3 cible;
    private float[] distanceDesRayons;
    private float angleEntreRayons;
    private float anglePourLongueur;
    private int layerObstacles = 1 << 6; // 65 = 01000001 en binaire : représente layer default et voiture
    private int indexRaycastCentre;
    private bool[] contactsAvecRaycast;
    private bool estEnDérapage;
    //private int layerColliderVoiture = 64;    // 01000000 (layer voiture)
    //private int layerColliderMur = 1;         // 00000001 (layer default)   *un layer spécifique aux murs pourrait être fait
    
    
    
    void Awake()
    {
        tc                  = GetComponent<TransmissionComponent>();
        sc                  = GetComponent<SteerComponent>();
        recovery            = GetComponent<Recovery>();
        rigidbodyVéhicule   = GetComponent<Rigidbody>();
        origineRaycasts     = GetComponentInChildren<BoxCollider>();
        wm     = FindObjectOfType<WaypointManager>();
        transformVéhicule   = transform;
        hitsInfo            = new RaycastHit[nbRays];
        raycastVectors      = new Vector3[nbRays];
        distanceDesRayons   = new float[nbRays];
        contactsAvecRaycast = new bool[nbRays];
        angleEntreRayons    = 180f / (nbRays - 1);
        anglePourLongueur   = 180f / (nbRays + 1);
        indexRaycastCentre  = nbRays / 2;
    }
    
    private void Start()
    {
        positionsWaypoint        = wm.waypointPositions;
        distanceMinimumWaypoints = wm.minimumDistanceToWaypoints;
        vitesseMaxÀCesWaypoints  = wm.maxSpeedAtWaypoints;
        nbWaypoints              = wm.nbWaypoints;
        indexWaypointActuel      = 0;
        directionDuWaypoint      = positionsWaypoint[indexWaypointActuel] - transform.position;
        distanceDuWaypoint       = directionDuWaypoint.magnitude;
        
        for (int i = 0; i < nbRays; ++i)
            distanceDesRayons[i] = distanceAvantCollision * Mathf.Pow(Mathf.Sin((i + 1) * anglePourLongueur * Mathf.Deg2Rad), 2);
    }
    
    private void Update()
    {
        DéterminerWaypointActuel();
        directionDevantVéhicule = transformVéhicule.forward;
        positionVéhicule        = transformVéhicule.position;
        vélocitéVéhicule        = rigidbodyVéhicule.velocity;
        vitesseVéhicule         = vélocitéVéhicule.magnitude;
        estEnDérapage           = Vector3.Angle(vélocitéVéhicule + directionDevantVéhicule, directionDevantVéhicule) > 5;

        if (tempsÉcouléEnMarcheArrière < délaiMarcheArrière) //Dans le cas où le véhicule se coince sur un mur, il fait marche arrière pendant 1 seconde, puis reprend sa course.
        {
            tc.InputDirectionVoiture(1);
            TournerReculonsVersWaypoint();
            tempsÉcouléEnMarcheArrière += Time.deltaTime;
        }
        else
        {
            // Gestion des raycasts pour éviter les collisions :
            GénérerVecteursDirection();
            for (int i = 0; i < nbRays; ++i)
            {
                Debug.DrawRay(transformVéhicule.TransformPoint(origineRaycasts.center), raycastVectors[i] * distanceDesRayons[i], Color.blue);
                if (Physics.Raycast(transformVéhicule.TransformPoint(origineRaycasts.center), raycastVectors[i], out hit, distanceDesRayons[i],layerObstacles))
                {
                    hitsInfo[i] = hit;
                    contactsAvecRaycast[i] = true;
                }
            }

            // Diriger le véhicule vers le waypoint :
            tempsÉcouléEnMarcheArrière = 0;
            délaiMarcheArrière = 0;
            SuivreWaypoints();
            if (doitAccélérer)
                tc.InputDirectionVoiture(0);
                    
            for (int i = 0; i < nbRays; ++i) // réinitialisation des détections de collision
                contactsAvecRaycast[i] = false;
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        if (vitesseVéhicule < 1f && transformVéhicule.InverseTransformPoint(collision.GetContact(0).point).z > 0)
            délaiMarcheArrière = 1f;
    }
    
    
    
    // Scripts pour le contrôle du véhicule
    private void TournerVersWaypoint()
    {
        angle = Vector3.SignedAngle(directionDevantVéhicule, directionDuWaypoint, Vector3.up);
        
        if (contactsAvecRaycast[indexRaycastCentre]) // éviter les collisions si possible
            ContournerObstacle();
        else
            ÉviterContact();        
        
        if(Mathf.Abs(angle) > 30 && vitesseVéhicule > 10 && !estEnDérapage) // freiner pour réduire le sous-virage
            tc.InputDirectionVoiture(1);
        if (estEnDérapage) // contre-braquage
            angle *= 1.2f;
        
        sc.angleRoues = angle;
    }
    
    private void TournerReculonsVersWaypoint()
    {
        angle = Vector3.SignedAngle(directionDevantVéhicule, directionDuWaypoint, Vector3.down);
        sc.angleRoues = angle;
    }     
    
    private void DéterminerWaypointActuel() // détermine le waypoint que le véhicule doit cibler
    {
        if (distanceDuWaypoint <= distanceMinimumWaypoints[indexWaypointActuel]) // a-t-on atteind le waypoint?
            indexWaypointActuel = ++indexWaypointActuel % nbWaypoints; // on change le waypoint ciblé
        
        // misa à jour des informations du waypoint ciblé
        directionDuWaypoint = positionsWaypoint[indexWaypointActuel] - positionVéhicule;
        distanceDuWaypoint = directionDuWaypoint.magnitude;
    }
    
    private void SuivreWaypoints()
    {
        if (Vector3.Dot(transformVéhicule.up, Vector3.down) > 0) // retourner le véhicule s'il est à l'envers
            recovery.Retourner(0);

        // Il faut freiner quand même
        if (vitesseVéhicule - distanceDuWaypoint / vitesseVéhicule * décélération > vitesseMaxÀCesWaypoints[indexWaypointActuel])
        {
            if(Vector3.Angle(positionsWaypoint[indexWaypointActuel] - positionVéhicule, directionDevantVéhicule) > 40)
                sc.InputFreinAMain(0);
            doitAccélérer = false;
            tc.InputDirectionVoiture(1);
        }
        else
            doitAccélérer = vitesseVéhicule < 10 || !estEnDérapage; // éviter un tête-à-queue
        
        TournerVersWaypoint();
    }
    
    
    
    // Méthodes pour les raycasts
    private void ContournerObstacle() // Il y a un véhicule droit devant
    {
        int obstructionGauche = 0;
        int obstructionDroit = 0;
        
        for (int i = 1; i <= nbRays / 2; ++i) // on collecte les contacts des RayCasts à gauche et à droite
        {
            if (contactsAvecRaycast[indexRaycastCentre + i]) ++obstructionDroit;
            if (contactsAvecRaycast[indexRaycastCentre - i]) ++obstructionGauche;
        }
        
        if (obstructionDroit == obstructionGauche)
        {
            if (obstructionDroit == nbRays / 2)    // Il n'y a nulle part où aller, alors freine
                tc.InputDirectionVoiture(1);
            else                                   // Effectue un dépassement
                angle += Vector3.Dot(directionDevantVéhicule, positionsWaypoint[(indexWaypointActuel + 1) % nbWaypoints] - positionVéhicule) > 0 ? -10 : 10;
        }
        else // dirige-toi vers l'espace libre
            angle += obstructionDroit - obstructionGauche > 0 ? -10 : 10;
    }

    private void ÉviterContact() // L’on ne détecte rien devant, on regarde les côtés
    {
        int obstructionGauche = 0;
        int obstructionDroit = 0;
        
        for (int i = 1; i <= nbRays / 2; ++i) // on collecte les contacts des RayCasts à gauche et à droite
        {
            if (contactsAvecRaycast[indexRaycastCentre + i]) ++obstructionDroit;
            if (contactsAvecRaycast[indexRaycastCentre - i]) ++obstructionGauche;
        }
        
        if (obstructionDroit != obstructionGauche) // Il y a un côté plus obstrué que l'autre
            angle += obstructionDroit - obstructionGauche > 0 ? -10 : 10;
    }
    
    private void GénérerVecteursDirection() //Crée un demi-cercle de vecteurs qui couvriront la vision avant du véhicule
    {
        for (int i = 0; i < nbRays; ++i)
            raycastVectors[i] = transformVéhicule.TransformDirection(Quaternion.AngleAxis(i * angleEntreRayons,Vector3.up) * Vector3.left);
    }
}