// Script écrit par Thomas Prévost et Frédéric Gaudreault

using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    [HideInInspector] public GameObject[] camerasChoisies;
    [HideInInspector] public GameObject cameraArrière;
    public KeyCode changerCaméra;
    public KeyCode caméraArrière;
    private bool regardeDerrière;
    private int indexCamera;
    
    private void Start()
    {
        // On s'assure qu'il n'y aie qu'une seule caméra et qu'un un seul "AudioListener" activés
        regardeDerrière = false;
        cameraArrière.SetActive(false);
        
        for (int i = 1; i < camerasChoisies.Length; ++i)
            camerasChoisies[i].SetActive(false);
        
        camerasChoisies[0].SetActive(true);
    }

    void Update()
    {
        // Le joueur veut regarder vers l'arrière
        if (Input.GetKeyDown(caméraArrière))
        {
            regardeDerrière = true; // active caméra arrière
            cameraArrière.SetActive(true);
            camerasChoisies[indexCamera].SetActive(false); // désactive caméra choisie
        }
        
        // Le joueur ne veut plus regarder vers l'arrière 
        if (Input.GetKeyUp(caméraArrière))
        {
            regardeDerrière = false; // désactive caméra arrière
            cameraArrière.SetActive(false);
            camerasChoisies[indexCamera].SetActive(true); // active caméra choisie
        }
        
        // Le joueur veut changer la caméra avant
        if (Input.GetKeyDown(changerCaméra))
        {
            if (!regardeDerrière) // pas besoin de désactiver si le joueur regarde à l'arrière
                camerasChoisies[indexCamera].SetActive(false);
            
            // On change quand même l'index de la caméralorsque le joueur regarde à l'arrière
            indexCamera = ++indexCamera % camerasChoisies.Length;
            
            if (!regardeDerrière) 
                camerasChoisies[indexCamera].SetActive(true);
        }
    }
}
