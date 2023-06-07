using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//écrit par Charles Trottier
public class ClickManager : MonoBehaviour
{
    private PauseManager click;

    public bool doAction;
    // Start is called before the first frame update
    void Start()
    {
        click = GameObject.Find("Pause").GetComponent<PauseManager>();

    }
    
    public void ResumeButtonClicked()
    {
        click.ResumeGame();
    }

    public void QuitButtonClicked()
    {
        click.QuitGame();
    }

    public void RestartButtonClicked()
    {
        click.RestartGame();
    }
}
