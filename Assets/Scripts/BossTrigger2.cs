using UnityEngine;
using System.Collections;

public class RoomTrigger2 : MonoBehaviour
{
    public BossLevel2 boss;
    public string musicSourceName = "MusicSource";
    public AudioClip bossIntroClip;
    public AudioClip bossMusicClip;

    private AudioSource audioSource;

    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();

       
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component is missing from the GameObject. Adding one.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        
        if (bossMusicClip != null)
        {
            audioSource.clip = bossMusicClip;
            audioSource.Play();
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the room.");

            if (boss != null)
            {
                boss.Activate();
            }

            
            Transform musicSourceTransform = transform.Find(musicSourceName);
            if (musicSourceTransform != null)
            {
                Destroy(musicSourceTransform.gameObject);
            }
            else
            {
                Debug.LogWarning("MusicSource GameObject not found.");
            }

            
            if (audioSource != null && bossIntroClip != null && bossMusicClip != null)
            {
                StartCoroutine(PlayBossMusicSequence());
            }
            else
            {
                Debug.LogError("AudioSource or audio clips are not set.");
            }
        }
    }

    private IEnumerator PlayBossMusicSequence()
    {
        // Play the intro clip
        audioSource.clip = bossIntroClip;
        audioSource.Play();

        // Wait for the intro clip to finish
        yield return new WaitForSeconds(bossIntroClip.length);

        // Play the boss music clip
        audioSource.clip = bossMusicClip;
        audioSource.loop = true; // Loop the boss music
        audioSource.Play();
    }
}



