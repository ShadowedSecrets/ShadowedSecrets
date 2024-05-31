using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    public float lifeTime = 2f;

    

    void Start()
    {
        
        Destroy(gameObject, lifeTime);
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //we need collision logic here
        Destroy(gameObject);
    }
}
