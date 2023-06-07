//Script travaillé par Charles Trottier et Frédéric Gaudreault et Thomas Prévost

using System;
using UnityEngine;

public class Health : MonoBehaviour

{
    [field : SerializeField] public float StartingHealth { get; private set; }
    [field : SerializeField] public float CurrentHealth { get; private set; }
    
    void Awake() => CurrentHealth = StartingHealth;
    
    // Méthode pour modifier la vie (entrer une valeur négative pour l'augmenter).
    public void TakeDamage(float damage) => CurrentHealth -= damage;
    
    // Méthode pour obtenir le ratio de la vie qui reste.
    public float HealthRatio() => CurrentHealth / StartingHealth;

    private void Update()
    {
        if (CurrentHealth < StartingHealth-0.05f)
            CurrentHealth += 0.05f;
    }
}