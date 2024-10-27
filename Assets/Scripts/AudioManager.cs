using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        DontDestroyOnLoad(Instance);
        audioSource.playOnAwake = false;
        audioSource.Stop();
    }

    public void PlaySong()
    {
        audioSource.Play();
    }

    public void StopSong()
    {
        audioSource.Stop();
    }
}
