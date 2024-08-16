using UnityEngine;
using UnityEngine.SceneManagement;

public class Ladder2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the ladder. Loading next scene.");
            SceneManager.LoadScene("FloorThreeScene");
        }
    }
}
