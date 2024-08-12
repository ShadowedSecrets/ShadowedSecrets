using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryBarController : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personName;

    private int sentenceIndex = -1;
    public StoryScene currentScene;
    private State state = State.Completed;
    private Animator animator;
    private bool isHidden;

    private enum State
    {
        Playing, Completed
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Hide()
    {
        if (isHidden) 
        {
            animator.SetTrigger("Hide");
            isHidden = true;
        }

    }
    public void Show()
    {
        animator.SetTrigger("Show");
        isHidden = false;
    }
    public void ClearText()
    {
        barText.text = "";
    }
    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        PlayNextSentence();
    }
   
    public void PlayNextSentence()
    {
        StartCoroutine(TypeText(currentScene.Sentences[++sentenceIndex].text));
        personName.text = currentScene.Sentences[sentenceIndex].speaker.speakerName;
        personName.color = currentScene.Sentences[sentenceIndex].speaker.textColor;
    }  
    
    public bool isCompleted()
    {
        return state == State.Completed;    
    }
    public bool isLastSentence()
    {
        return sentenceIndex + 1 == currentScene.Sentences.Count;
    }
    private IEnumerator TypeText(string text)
    {
        barText.text = "";
        state = State.Playing;
        int wordIndex = 0;
        while (state != State.Completed)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(0.05f);
            if(++wordIndex == text.Length)
            {
                state = State.Completed;
            }

        }
    }
}
