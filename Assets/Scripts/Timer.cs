using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] float elapsedTime;

    // Update is called once per frame
    void Update()
    {

        elapsedTime += Time.deltaTime;
      
     
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (elapsedTime >= 600)    //Logic for taking too long
        {

        }
       

    }
}
