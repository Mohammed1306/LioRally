//Ce script à été copié du troisième cours de programmation,
//mais risque d'être modifié dans le futur afin de supporter des manettes de jeux (inputs variables).
//Plusieurs modifications faite par Mohamed Abdellatif Kallel

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManagment : MonoBehaviour
{
    // Touches du clavier consacrées au mouvement
    [SerializeField] private KeyCode[] movementKeys;
    // Les actions associées à chaque touche de mouvement
    /*[SerializeField]*/ private UnityEvent<int>[] movementActions;

    // Le joueur qui va être controllé
    private GameObject joueur;
    private TransmissionComponent transmissionComponent;
    private SteerComponent steerComponent;
    private Recovery recovery;

    // Tableau de Bit. Beaucoup plus petit et efficace qu'un tableau de booléens
    // Chaque bit représente un intrant du clavier
    private BitArray inputBuffer;
    
    private void Start()
    {
        movementActions = new UnityEvent<int>[6];
        for (int i = 0; i < movementActions.Length; i++)
        {
            movementActions[i] = new UnityEvent<int>();
        }
        
        joueur = GameObject.Find("Joueur");
        transmissionComponent = joueur.GetComponent<TransmissionComponent>();
        steerComponent = joueur.GetComponent<SteerComponent>();
        recovery = joueur.GetComponent<Recovery>();



        movementActions[0].AddListener(transmissionComponent.InputDirectionVoiture);
        movementActions[0].Invoke(0);
        
        movementActions[1].AddListener(transmissionComponent.InputDirectionVoiture);
        movementActions[1].Invoke(1);
        
        movementActions[2].AddListener(steerComponent.InputDirectionRoue);
        movementActions[2].Invoke(1);
        
        movementActions[3].AddListener(steerComponent.InputDirectionRoue);
        movementActions[3].Invoke(0);
        
        movementActions[4].AddListener(steerComponent.InputFreinAMain);
        movementActions[4].Invoke(0);
        
        movementActions[5].AddListener(recovery.Retourner);
        movementActions[5].Invoke(0);
        
        
        Debug.Assert(movementKeys.Length == movementActions.Length);
        inputBuffer = new BitArray(movementKeys.Length);
    }
    private void Update()
    {
        // Poll inputs -- Vérifier l'état de tous les intrants
        PollDigitalInputs();
        // Process input -- faire des actions en conséquence des intrants
        ProcessDigitalInputs();
    }

    private void PollDigitalInputs()
    {
        for (int i = 0; i < movementKeys.Length; ++i)
            inputBuffer[i] = Input.GetKey(movementKeys[i]);
    }

    private void ProcessDigitalInputs()
    {
        for (int i = 0; i < inputBuffer.Count; ++i)
            if (inputBuffer[i])
                InvokeAction(i); // -1 
    }

    private void InvokeAction(int i)
    {
        if (i == 0 || i == 3)
        {
            movementActions[i].Invoke(0);
        }
        else if (i == 1 || i == 2)
        {
            movementActions[i].Invoke(1);
        }
        else
        {
            movementActions[i].Invoke(i);
        }
    }
}
