using UnityEngine;
using UnityEngine.SceneManagement;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the ladder. Loading next scene.");
            SceneManager.LoadScene("FloorTwoScene");
        }
    }
}

