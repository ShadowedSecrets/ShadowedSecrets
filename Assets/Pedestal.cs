using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public string requiredGemTag; // Tag of the gem required to destroy this pedestal
    public Door door;            // Reference to the door that will be unlocked

    private static int destroyedPedestalCount = 0; // Track the number of destroyed pedestals
    private static int totalPedestals = 2;        // Total number of pedestals

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(requiredGemTag))
        {
            DestroyPedestal();
        }
    }

    private void DestroyPedestal()
    {
        Destroy(gameObject);
        destroyedPedestalCount++;

        if (destroyedPedestalCount >= totalPedestals)
        {
            door.UnlockDoor();                                  // Unlock the door if both pedestals are destroyed
        }
    }
}

