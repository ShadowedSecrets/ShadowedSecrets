using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{
    public ParticleSystem collsionParticleSystem;
    public SpriteRenderer sR;
    public bool once = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && once)
        {
            var em = collsionParticleSystem.emission;
            var dur = collsionParticleSystem.duration;
            em.enabled = true;
            collsionParticleSystem.Play();

            once = false;
            //GetComponent<SpriteRenderer>().enabled = false;
            //Destroy(sR);
            //Invoke(nameof(DestroyObj), dur);
        }
    }
    private void DestroyObj()
    {
        Destroy(gameObject);
    }
}