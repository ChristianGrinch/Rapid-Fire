using UnityEngine;

public class BackgroundMusic : MonoBehaviour // Not my script! Thanks to https://stackoverflow.com/a/53684356 for it. 
{
    public bool randomPlay = false; // checkbox for random play
    public AudioClip[] clips;
    private AudioSource audioSource;
    int clipOrder = 0; // for ordered playlist

    public static BackgroundMusic Instance { get; private set; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    void FixedUpdate() // changed to fixedupdate to see if it can help reduce the random stuttering when transitioning songs
    {
        if (!audioSource.isPlaying)
        {
            // if random play is selected
            if (randomPlay == true)
            {
                audioSource.clip = GetRandomClip();
                audioSource.Play();
                // if random play is not selected
            }
            else
            {
                audioSource.clip = GetNextClip();
                audioSource.Play();
            }
        }
    }

    // function to get a random clip
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    // function to get the next clip in order, then repeat from the beginning of the list.
    private AudioClip GetNextClip()
    {
        if (clipOrder >= clips.Length - 1)
        {
            clipOrder = 0;
        }
        else
        {
            clipOrder += 1;
        }
        return clips[clipOrder];
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
