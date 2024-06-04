using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        //scoreText.text = " ";
    }
    // Start is called before the first frame update
    void Update()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}