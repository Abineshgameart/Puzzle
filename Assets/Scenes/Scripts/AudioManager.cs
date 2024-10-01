using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    // Private
    [Header("====== Sound Button ======")]
    [SerializeField] Image soundButton;
    Color newColor;

    [Header("====== Audio Source ======")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    


    // Public
    [Header("====== Audio Clip ======")]
    public AudioClip background;
    public AudioClip tileMovement;
    public AudioClip levelCompleted;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        
    }


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
        ColorUtility.TryParseHtmlString("888888", out newColor);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            soundButton.color = Color.grey;
            musicSource.Stop();
        }
        else
        {
            soundButton.color = Color.white;
            musicSource.Play();
        }
        
    }
}
