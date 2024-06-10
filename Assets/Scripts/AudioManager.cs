using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundMusic;
    public AudioClip bossIntroClip;
    public AudioClip bossMusic;


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
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        audioSource.Stop();
    }

    public void PlayBossMusic()
    {
        StopBackgroundMusic();
        StartCoroutine(PlayBossMusicCoroutine());
    }

    private IEnumerator PlayBossMusicCoroutine()
    {
        if (bossIntroClip != null)
        {
            audioSource.clip = bossIntroClip;
            audioSource.loop = false;
            audioSource.Play();
            yield return new WaitForSeconds(bossIntroClip.length);
        }

        
        if (bossMusic != null)
        {
            audioSource.clip = bossMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}

