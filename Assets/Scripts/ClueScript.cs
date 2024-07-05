using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueScript : MonoBehaviour
{
    public RiddleManager riddleManager;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            riddleManager.ShowRiddlePanel();
            Destroy(gameObject);
        }
    }
}

