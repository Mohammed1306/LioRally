//Script écrit par Frédéric Gaudreault.

using UnityEngine;
using UnityEngine.Serialization;

public class Recovery : MonoBehaviour
{
    [FormerlySerializedAs("coolDown")] [SerializeField] private float tempsRecharge = 2;
    private bool toucheEstPesé;
    private float attente;
    private Transform objet;
    private Rigidbody rb;
    
    private void Awake()
    {
        toucheEstPesé = false;
        attente = 0;
        objet = gameObject.transform;
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (toucheEstPesé)
        {
            Retourner(0);
            toucheEstPesé = false;
        }
        attente += Time.deltaTime;
    }
    
    public void InverserBool() => toucheEstPesé = true;  
    
    // Le paramètre est inutile à la fonction, mais utile à d'autre endroit: IL FAUT LE GARDER
    public void Retourner(int param)
    { 
        if (attente > tempsRecharge)
        {
            rb.angularVelocity = Vector3.zero;
            objet.position += new Vector3(0,0.5f,0);
            objet.rotation = Quaternion.Euler(0, objet.rotation.eulerAngles.y, 0);
            attente = 0;
        }
    }
}
