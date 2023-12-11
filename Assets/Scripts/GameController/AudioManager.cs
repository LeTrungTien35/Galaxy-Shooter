using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip button;
    public AudioClip backGround_menu;
    public AudioClip backGround_level1;

    public AudioClip explosionSound_Player;
    public AudioClip explosionSound_Enemy;

    public AudioClip bullet_Player;

    public bool musicSourceMuted;
    public bool sfxSourceMuted;

    [Range(0, 1)]
    public float sfxVolume = 1f;

    [Range(0, 1)]
    public float musicVolume = 0.2f;
    private void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        musicSourceMuted = musicSource.mute;
        sfxSourceMuted = sfxSource.mute;
        PlayMusic(backGround_menu, true);
    }

    void MakeInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public void PlaySFX(AudioClip clip)
    {

        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    public void PlayMusic(AudioClip music, bool canLoop)
    {
        if (musicSource && music != null)
        {
            musicSource.clip = music;
            musicSource.loop = canLoop;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }

    public void OnOffMusic()
    {
        if(musicSource.mute) // DANG TAT AM 
        {
            musicSource.mute = false; // BAT AM THANH
            musicSourceMuted = false; // CAP NHAT TRANG THAI
        }
        else // DANG BAT AM THANH
        {
            musicSource.mute = true; // TAT AM THANH
            musicSourceMuted = true; // CAP NHAT TRANG THAI
        }
    }

    public void OnOffSFX()
    {
        if (sfxSource.mute) // DANG TAT AM
        {
            sfxSource.mute = false; // BAT AM THANH
            sfxSourceMuted = false; // CAP NHAT TRANG THAI
        }
        else // DANG BAT AM THANH
        {
            sfxSource.mute = true; // TAT AM THANH
            sfxSourceMuted = true; // CAP NHAT TRANG THAI
        }
    }
}
