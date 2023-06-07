//Travaillé par Charles Trottier et Frédéric Gaudreault

using UnityEngine;
using UnityEngine.Serialization;

public class NeedleMovement : MonoBehaviour
{
    private const float MIN_RPM = 90f, MAX_RPM = -85f;
    
    [FormerlySerializedAs("véhicule")] public GameObject vehicle;
    private GameObject oldVehicle;
    private MotorComponent mc;
    private RectTransform needle;
    private float angleValues;
    private float maxFrontCouple;
    
    private void Awake()
    {
        needle = transform.Find("Aiguille").GetComponent<RectTransform>();
        needle.rotation = Quaternion.Euler(0, 0, MIN_RPM);
        angleValues = MIN_RPM - MAX_RPM;
    }

    private void Start()
    {
        mc = vehicle.GetComponent<MotorComponent>();
        maxFrontCouple = mc.maxCoupleAvant;
        oldVehicle = vehicle;
    }

    private void Update()
    {
        if (oldVehicle == vehicle)
        {
            needle.localEulerAngles = new Vector3(0, 0, NeedleAngle());
        }
        else
        {
            mc = vehicle.GetComponent<MotorComponent>();
            maxFrontCouple = mc.maxCoupleAvant;
            oldVehicle = vehicle;
        }
    }
    
    private float NeedleAngle() => MIN_RPM - mc.coupleMoteur / maxFrontCouple * angleValues;
}
