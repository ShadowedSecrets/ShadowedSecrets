using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTxt;
    [SerializeField] float remainingTime;

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        } 
        else if(remainingTime < 0)
        {
            remainingTime = 0;
            timerTxt.color = Color.red ;
        }
        //remainingTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        // Debug.Log(remainingTime);

    }
}
