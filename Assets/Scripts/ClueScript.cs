using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueScript : MonoBehaviour
{
    public RiddleManager riddleManager;
    [TextArea]
    public string clueText;

    //public SpriteRenderer myRenderer;

    bool wasSeen = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!wasSeen)
            {
                wasSeen = true;
                //if (myRenderer != null)
                  //  myRenderer.color *= 0.5f;
            }

            if (clueText != "")
            {
                riddleManager.ShowText(clueText);
            }
            else
            {
                riddleManager.ShowRiddlePanel();
            }
            Destroy(gameObject);
        }
    }
}



