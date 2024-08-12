using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryGameController : MonoBehaviour
{
    public StoryScene currentScene;
    public StoryBarController bottomBar;

    private State state = State.Idle;
    private enum State
    {
        Idle, Animate
    }
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
                if( state == State.Idle && bottomBar.isLastSentence()) 
                {
                    PlayScene(currentScene);
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
                
            }
        }
    }
    private void PlayScene(StoryScene scene)
    {
        StartCoroutine(Switchscene(scene));
    }
    private IEnumerator Switchscene(StoryScene scene)
    {
        state = State.Animate;
        currentScene = scene;
        bottomBar.Hide();
        yield return new WaitForSeconds(1f);
        bottomBar.ClearText();
        bottomBar.Show();
        yield return new WaitForSeconds(1f);
        bottomBar.PlayScene(currentScene);
        state = State.Idle;
    }
}
