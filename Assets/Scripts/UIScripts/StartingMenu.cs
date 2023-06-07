//Mohamed Abdellatif Kallel
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartingMenu : MonoBehaviour
{
    public GameObject starting;
    public GameObject main;

    public bool KeyPressed()
    {
        return Input.anyKey;
    }

    private void Update()
    {
        if (KeyPressed())
        {
            starting.SetActive(false);
            main.SetActive(true);
        }
    }
}
