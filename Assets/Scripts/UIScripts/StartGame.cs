//Écrit par Thomas Prévost et Charles Trottier
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UIScripts
{
    public class StartGame : MonoBehaviour
    {
        private int countdownTime = 3;

        [SerializeField] private TextMeshProUGUI countdownDisplay;

        private float timerDelay = 1;

        private bool notVisible;

        private void Start()
        {
            StartCoroutine(CountDownToStart());
        }

        //Cette Coroutine a été prise dans la vidéo YouTube suivante : https://www.youtube.com/watch?v=ulxXGht5D2U
        public IEnumerator CountDownToStart()
        {
            Time.timeScale = 0;
            countdownTime = 3;
            countdownDisplay.gameObject.SetActive(true);
            while (countdownTime >= 1)
            {
                countdownDisplay.text = countdownTime.ToString();

                yield return new WaitForSecondsRealtime(1);
                //Debug.Log("oui");

                countdownTime--;
            }

            Time.timeScale = 1;

            countdownDisplay.text = "GO!";

            yield return new WaitForSeconds(timerDelay);
        
            countdownDisplay.gameObject.SetActive(notVisible);
        }
    }
}