using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] cutSound;
    public AudioClip gameoverSound;
    public AudioClip buttonSound;
    public AudioClip bombSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioPlay(string what)
    {
        switch (what)
        {
            case "Cut":
                int rand = Random.Range(0, cutSound.Length);
                audioSource.clip = cutSound[rand];
                break;
            case "Bt":
                audioSource.clip = buttonSound;
                break;
            case "Over":
                FindObjectOfType<Director>().GetComponent<AudioSource>().Stop();
                audioSource.clip = gameoverSound;
                break;
            case "Bomb":
                FindObjectOfType<Director>().GetComponent<AudioSource>().Stop();
                audioSource.clip = bombSound;
                break;
        }

        audioSource.Play();
    }
}
