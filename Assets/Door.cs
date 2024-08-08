using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isUnlocked = false;

    public void UnlockDoor()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            Destroy(gameObject);
        }
    }
}

