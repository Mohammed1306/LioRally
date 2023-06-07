using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneFinale : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [FormerlySerializedAs("tempsNécessaire")] [SerializeField] private float timeNeeded;
    
    private DontDestroy[] cars;

    private float elapsedTime = 0;
    
    public bool KeyPressed()
    {
        return Input.anyKey;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeNeeded)
        {
            text.SetActive(true);
            if (KeyPressed())
            {
                cars = FindObjectsOfType<DontDestroy>();
                foreach (var car in cars)
                {
                    Destroy(car.gameObject);
                }
                SceneManager.LoadScene("IntroScene");
            }
        }
    }
}
