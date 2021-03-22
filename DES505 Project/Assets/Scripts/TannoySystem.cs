using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TannoySystem :Singleton<TannoySystem>
{
    public AudioClip[] clipsGameover;
    public AudioClip[] clipsRandom;
    public AudioClip[] clipsKnockPackage;
    public AudioClip[] clipsPhotoFound;

    AudioSource audioSource;

    float randomFrequency = 30f;
    float randomRange = 10f;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlayRandomAudio());
    }

    IEnumerator PlayRandomAudio()
    {
        while(true)
        {
            float waitSeconds = randomFrequency + Random.Range(-randomRange, randomRange);
            yield return new WaitForSeconds(waitSeconds);
            Debug.Log("Tannoy: Random");
            if(clipsRandom.Length > 0)
                audioSource.PlayOneShot(clipsRandom[Random.Range(0, clipsRandom.Length)]);
        }
    }

    public void PlayGameOver()
    {
        Debug.Log("Tannoy: GameOver");
        if (clipsGameover.Length > 0)
            audioSource.PlayOneShot(clipsGameover[Random.Range(0, clipsGameover.Length)]);
    }

    public void PlayPhotoFound(int index)
    {
        if(index < clipsPhotoFound.Length)
        {
            Debug.Log("Tannoy: Photo " + index);
            if (clipsPhotoFound.Length > 0)
                audioSource.PlayOneShot(clipsPhotoFound[index]);
        }
    }

    public void PlayTannoyAudio(AudioClip clip)
    {
        Debug.Log("Play Tannoy " + clip.name);
        audioSource.PlayOneShot(clip);
    }
}
