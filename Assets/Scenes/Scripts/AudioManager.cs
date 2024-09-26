using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Private
    [Header("====== Audio Source ======")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;



    // Public
    [Header("====== Audio Clip ======")]
    public AudioClip background;
    public AudioClip tileMovement;
    public AudioClip levelCompleted;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
