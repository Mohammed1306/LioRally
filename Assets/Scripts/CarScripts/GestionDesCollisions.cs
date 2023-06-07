//Script écrit par Charles Trottier.

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionDesCollisions : MonoBehaviour
{
   private float damageAmount;
   private const int realisticDamage = 1000; //permet de réduire l'impact d'une collision de manière réaliste
   private Rigidbody rb;
   private Health health;
   [HideInInspector] public bool damageTaken;
   private float timer;
   private float duration = 5f;

   private void Awake()
   {
      BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
      health = GetComponent<Health>();
      rb = GetComponent<Rigidbody>();
      damageTaken = false;
   }
   //Coefficient de dommage selon la zone d'impact
   private float[] ImpactZoneDamage =
   {
      1.5f, 1.25f, 1.0f, 1.0f
   };
   //Tableau représentant les zones d'impacts possibles
   private string[] ZonesCollision =
   {
      "Engine", "RearBumper", "RightDoor", "LeftDoor"
   };
   //Lors d'une collision, la méthode ci-dessous permet de trouver la zone d'impact et calculer les dommages
   //qui seront infligés en fonction de la zone d'impact et de la force de l'impact
   private void OnCollisionEnter(Collision collision)
   {
      damageTaken = true;
      ContactPoint[] contacts = collision.contacts;
      ContactPoint firstContact = contacts[0];
      Collider thisCollider = firstContact.thisCollider;

      for (int i = 0; i < ZonesCollision.Length; ++i)
      {
         if (thisCollider.name == ZonesCollision[i])
         {
            damageAmount = collision.impulse.magnitude / realisticDamage + ImpactZoneDamage[i];
            
         }
        
      }
      if (health != null)
      {
         health.TakeDamage(damageAmount);
      }
   }
}
