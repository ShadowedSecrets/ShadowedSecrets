using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public Boss boss;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            Debug.Log("Player entered the room.");
            if (boss != null)
            {
                boss.Activate();
            }
        }
    }
}

