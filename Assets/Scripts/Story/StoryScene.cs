using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : ScriptableObject
{
    public List<Sentence> Sentences;
    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
    }
}
