//Script écrit par Frédéric Gaudreault

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonSurface : MonoBehaviour
{
    [Header("valeurs audio gravelle")]
    [SerializeField] float pitchMinGravelle;
    [SerializeField] float pitchMaxGravelle;
    [SerializeField] float volumeMaxGravelle;
    
    // en commentaire -> dans le cas où l'on trouve un son adéquat pour l'asphalte
    
    // [Header("valeurs audio asphalte")]       
    // [SerializeField] float pitchMinAsphalte;
    // [SerializeField] float pitchMaxAsphalte;
    // [SerializeField] float volumeMaxAsphalte;
    
    private TransmissionComponent tc;
    private AudioSource[] sourcesAudio;
    private AdherencePneu ap;
    private float deltaPitchGravelle;
    //private float deltaPitchAsphalte;

    void Start()
    {
        tc = GetComponent<TransmissionComponent>();
        sourcesAudio = GetComponents<AudioSource>();
        ap = GetComponent<AdherencePneu>(); 
        deltaPitchGravelle = pitchMaxGravelle - pitchMinGravelle;
        //deltaPitchAsphalte = pitchMaxAsphalte - pitchMinAsphalte;
    }
    
    void Update()
    {
        if(ap.estEnLAire)
        {
            sourcesAudio[1].enabled = false;
            //audioSources[2].enabled = false;
        }
        else
        {
            sourcesAudio[1].enabled = ap.estSurGravelle;
            //audioSources[2].enabled = ap.estSurAsphalte;
            
            if (ap.estSurGravelle)
            {
                sourcesAudio[1].pitch = pitchMinGravelle + deltaPitchGravelle - deltaPitchGravelle / (1 + (float)tc.vitesse * 0.01f);
                sourcesAudio[1].volume = volumeMaxGravelle - volumeMaxGravelle / (1 + (float)tc.vitesse * 0.02f);
            }
            
            //if (ap.estSurAsphalte)
            //{
            //    audioSources[2].pitch = pitchMinAsphalte + deltaPitchAsphalte - deltaPitchAsphalte / (1 + (float)tc.speed * 0.01f);
            //    audioSources[2].volume = volumeMaxAsphalte - volumeMaxAsphalte / (1 + (float)tc.speed * 0.02f);
            //}         
        }
    }
}
