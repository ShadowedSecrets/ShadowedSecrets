using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundMusic;

    public AudioClip shootClip;
    public AudioClip enemyHitClip;
    public AudioClip keyGrabbed;
    public AudioClip dashClip;
    public AudioClip doorOpen;
    public AudioClip playerHit;
    public AudioClip pestClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.loop = false; // Ensure looping is initially off
            audioSource.pitch = 1f; // Ensure pitch is set to normal speed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootClip);
    }

    public void PlayEnemyHitSound()
    {
        audioSource.PlayOneShot(enemyHitClip);
    }

    public void PlayKeyGrabSound()
    {
        audioSource.PlayOneShot(keyGrabbed);
    }

    public void PlayDashSound()
    {
        audioSource.PlayOneShot(dashClip);
    }

    public void PlayPestSound()
    {
        audioSource.PlayOneShot(pestClip);
    }

    public void PlayDoorOpenSound()
    {
        audioSource.PlayOneShot(doorOpen);
    }

    public void PlayPlayerHitSound()
    {
        audioSource.PlayOneShot(playerHit);
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.pitch = 1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music clip not assigned!");
        }
    }

    public void StopBackgroundMusic()
    {
        if (audioSource.clip == backgroundMusic)
        {
            audioSource.Stop();
        }
    }

    public bool IsBackgroundMusicPlaying()
    {
        return audioSource.clip == backgroundMusic && audioSource.isPlaying;
    }
}
