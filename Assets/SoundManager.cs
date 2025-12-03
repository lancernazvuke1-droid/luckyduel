using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Звуки")]
    public AudioClip shotClip;       // выстрел
    public AudioClip hitClip;        // попадание по мишени
    public AudioClip winClip;        // победа
    public AudioClip loseClip;       // поражение
    public AudioClip spawnClip;      // появление мишени

    [Header("Источник звука")]
    public AudioSource sfxSource;    // AudioSource для эффектов

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // если хочешь через сцены сохранять
        }
        else
        {
            Destroy(gameObject);
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayShot()
    {
        PlaySound(shotClip);
    }

    public void PlayHit()
    {
        PlaySound(hitClip);
    }

    public void PlayWin()
    {
        PlaySound(winClip);
    }

    public void PlayLose()
    {
        PlaySound(loseClip);
    }

    public void PlaySpawn()
    {
        PlaySound(spawnClip);
    }
}
