using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    // Singleton
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<SoundManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SoundManager");
                    instance = go.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private int sfxSourcePoolSize = 5;

    [Header("Volume Settings")]
    [Range(0f, 1f)] private float musicVolume = 1f;
    [Range(0f, 1f)] private float sfxVolume = 1f;

    [Header("Music Clips")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip bounceSound;
    [SerializeField] private AudioClip attachSound;
    [SerializeField] private AudioClip detachSound;
    [SerializeField] private AudioClip deathSoundbylostinspace;
    [SerializeField] private AudioClip deathSoundbySpike;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip PianoSound;

    private List<AudioSource> sfxSourcePool = new List<AudioSource>();
    private int currentSfxSourceIndex = 0;

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
            LoadVolumeSettings();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void InitializeAudioSources()
    {
        // Music source oluþtur
        if (musicSource == null)
        {
            GameObject musicGO = new GameObject("MusicSource");
            musicGO.transform.SetParent(transform);
            musicSource = musicGO.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        // SFX source pool oluþtur
        for (int i = 0; i < sfxSourcePoolSize; i++)
        {
            GameObject sfxGO = new GameObject($"SFXSource_{i}");
            sfxGO.transform.SetParent(transform);
            AudioSource source = sfxGO.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSourcePool.Add(source);
        }

        // Ana SFX source
        if (sfxSource == null && sfxSourcePool.Count > 0)
        {
            sfxSource = sfxSourcePool[0];
        }
    }

    void LoadVolumeSettings()
    {
        // PlayerPrefs'ten ses ayarlarýný yükle
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyVolumeSettings();
    }

    void ApplyVolumeSettings()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;

        foreach (var source in sfxSourcePool)
        {
            source.volume = sfxVolume;
        }
    }

    // Müzik Kontrolleri
    public void PlayMainMenuMusic()
    {
        if (mainMenuMusic != null)
            PlayMusic(mainMenuMusic);
    }

    public void PlayGameMusic()
    {
        if (gameMusic != null)
            PlayMusic(gameMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            if (musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null)
            musicSource.UnPause();
    }

    // SFX Oynatma Fonksiyonlarý
    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }

    public void PlayJump()
    {
        PlaySFX(jumpSound);
    }

    public void PlayLand()
    {
        PlaySFX(landSound);
    }

    public void PlayExplosion()
    {
        PlaySFX(explosionSound);
    }

    public void PlayBounce()
    {
        PlaySFX(bounceSound);
    }

    public void PlayAttach()
    {
        PlaySFX(attachSound);
    }

    public void PlayDetach()
    {
        PlaySFX(detachSound);
    }

    public void PlayDeathbySpike()
    {
        PlaySFX(deathSoundbySpike);
    }
    public void PlayDeathbylostinspace()
    {
        PlaySFX(deathSoundbylostinspace);
    }

    public void PlayVictory()
    {
        PlaySFX(victorySound);
    }
    public void PlayPiano()
    {
        PlaySFX(PianoSound);
    }

    // Genel SFX oynatma
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null && sfxSourcePool.Count > 0)
        {
            AudioSource source = GetAvailableSFXSource();
            source.clip = clip;
            source.volume = sfxVolume * volumeScale;
            source.Play();
        }
    }

    // 3D SFX oynatma
    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volumeScale = 1f)
    {
        if (clip != null)
        {
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = position;

            AudioSource tempSource = tempGO.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.volume = sfxVolume * volumeScale;
            tempSource.spatialBlend = 1f; // 3D ses
            tempSource.Play();

            Destroy(tempGO, clip.length);
        }
    }

    // Uygun SFX source bul
    private AudioSource GetAvailableSFXSource()
    {
        for (int i = 0; i < sfxSourcePool.Count; i++)
        {
            int index = (currentSfxSourceIndex + i) % sfxSourcePool.Count;
            if (!sfxSourcePool[index].isPlaying)
            {
                currentSfxSourceIndex = (index + 1) % sfxSourcePool.Count;
                return sfxSourcePool[index];
            }
        }

        // Hepsi meþgulse, en eskisini kullan
        currentSfxSourceIndex = (currentSfxSourceIndex + 1) % sfxSourcePool.Count;
        return sfxSourcePool[currentSfxSourceIndex];
    }

    // Ses Ayarlarý
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = musicVolume;

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);

        foreach (var source in sfxSourcePool)
        {
            source.volume = sfxVolume;
        }

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    // Ses Durumu Kontrolleri
    public bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }

    public void ToggleMusic()
    {
        if (IsMusicPlaying())
            PauseMusic();
        else
            ResumeMusic();
    }

    // Fade efektleri
    public void FadeMusicIn(float duration = 1f)
    {
        StartCoroutine(FadeMusic(0f, musicVolume, duration));
    }

    public void FadeMusicOut(float duration = 1f)
    {
        StartCoroutine(FadeMusic(musicSource.volume, 0f, duration));
    }

    private System.Collections.IEnumerator FadeMusic(float startVolume, float endVolume, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (musicSource != null)
                musicSource.volume = Mathf.Lerp(startVolume, endVolume, t);

            yield return null;
        }

        if (musicSource != null)
            musicSource.volume = endVolume;
    }
}