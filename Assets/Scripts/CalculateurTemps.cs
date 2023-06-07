//Écrit par Thomas Prévost

using System.Linq;
using UnityEngine;

public class CalculateurTemps : MonoBehaviour
{
    public float TempsCourse { get; private set; }
    public bool courseTerminé;
    public int positionFinal;
    public Classement classement;

    private void Awake()
    {
        classement = FindObjectOfType<Classement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!courseTerminé)
        {
            TempsCourse += Time.deltaTime;
            positionFinal = classement.classementFinal.Keys.ToList().IndexOf(gameObject.name);
        }
    }
}
