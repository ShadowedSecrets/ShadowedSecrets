using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("------ Audio Clip ------")]
    public AudioClip backgroundMusic;
    public AudioClip shootClip;
    public AudioClip enemyHitClip;
    public AudioClip keyGrabbed;
    public AudioClip dashClip;
    public AudioClip doorOpen;
    public AudioClip playerHit;
    public AudioClip pestClip;
    public AudioClip HealthPotion;

    [Header("------ Sliders ------")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private AudioSource musicSource;
    //private AudioSource sfxSource;
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
    
    void Start()
    {
        // Initialize sliders with saved values or default values
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Add listeners to sliders
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        //AudioSources, one for music and one for SFX
        musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        //sfxSource = GameObject.Find("SFXSource").GetComponent<AudioSource>();

        // Set initial values
        SetMasterVolume(masterVolumeSlider.value);
        SetMusicVolume(musicVolumeSlider.value);
        SetSFXVolume(sfxVolumeSlider.value);
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
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

    public void PlayPotionSound()
    {
        audioSource.PlayOneShot(HealthPotion);
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.pitch = 1f;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music clip not assigned!");
        }
    }

    public void StopBackgroundMusic()
    {
        if (musicSource.clip == backgroundMusic)
        {
            musicSource.Stop();
        }
    }

    public bool IsBackgroundMusicPlaying()
    {
        return audioSource.clip == backgroundMusic && audioSource.isPlaying;
    }
}
