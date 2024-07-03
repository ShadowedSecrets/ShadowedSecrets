using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    public string color;
    public Crystals crystalManager;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            crystalManager.DestroyCrystal(color);
            gameObject.SetActive(false);
        }
    }
}

