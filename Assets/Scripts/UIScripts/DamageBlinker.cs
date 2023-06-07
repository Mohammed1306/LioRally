//Écrit par Charles Trottier

using UnityEngine;

public class DamageBlinker : MonoBehaviour
{
    public GameObject vehicle;
    private GameObject oldVehicle;
    private GestionDesCollisions damage;
    private RectTransform hazardLogo;
    private bool isVisible = true;
    private float timer;
    private float duration = 5f;
    
    private void Awake() 
    {
        hazardLogo = transform.Find("Hazard").GetComponent<RectTransform>();
        hazardLogo.gameObject.SetActive(!isVisible);
    }

    private void Start()
    {
        damage = vehicle.GetComponent<GestionDesCollisions>();
        oldVehicle = vehicle;
    }

    private void Update() 
    {
        //Si une collision est détectée, cette méthode permet de faire clignoter le logo de danger
        //et l'arrêter après un certain temps
        if (oldVehicle == vehicle)
        {               
            if (damage.damageTaken)
            {
                timer += Time.deltaTime;
                InvokeRepeating("BlinkerVisible", 0f, 1f);
            }

            if (timer >= duration)
            {
                isVisible = false;
                CancelInvoke("BlinkerVisible");
                damage.damageTaken = false;
                hazardLogo.gameObject.SetActive(isVisible);
                timer = 0;
            }
        }
        else
        {
            oldVehicle = vehicle;
            damage = vehicle.GetComponent<GestionDesCollisions>();
        }
    }


    private void BlinkerVisible()
    {
        isVisible = !isVisible;
        hazardLogo.gameObject.SetActive(isVisible);
    }
}