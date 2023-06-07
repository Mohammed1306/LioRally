// Script écrit par Frédéric Gaudreault
// *** utilisé à des fins de démonstration seulement ***

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDuTemps : MonoBehaviour
{
    public float timeScale;
    private float fixedDeltaTime;

    private void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        Time.timeScale = timeScale; // on ralentit le temps
        
        // on augmente la fréquence du moteur de physique (garde la simulation fluide)
        Time.fixedDeltaTime = fixedDeltaTime * timeScale;
    }
}
