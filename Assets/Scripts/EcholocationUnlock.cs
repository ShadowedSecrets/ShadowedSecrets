using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcholocationUnlock : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAbilities playerAbilitiesScript = collision.GetComponent<playerAbilities>();
            if (playerAbilitiesScript != null)
            {
                playerAbilitiesScript.UnlockEcholocation();
                Debug.Log("Echolocation unlocked!");
               // Destroy(gameObject);
            }
        }
    }
}

