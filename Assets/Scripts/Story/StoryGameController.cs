using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGameController : MonoBehaviour
{
    public StoryScene currentScene;
    public StoryBarController bottomBar;

    // Start is called before the first frame update
    void Start()
    {
        bottomBar.PlayScene(currentScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (bottomBar.isCompleted())
            {
                if(bottomBar.isLastSentence()) 
                {
                    bottomBar.PlayScene(currentScene);
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
                
            }
        }
    }
}
