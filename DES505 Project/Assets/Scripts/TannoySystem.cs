using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TannoySystem :Singleton<TannoySystem>
{
    public AudioClip[] clipsGameover;
    public AudioClip[] clipsRandom;
    public AudioClip[] clipsKnockPackage;
    public AudioClip[] clipsPhotoFound;
    public AudioClip[] clipsPosterFound;

    AudioSource audioSource;

    float randomFrequency = 60f;
    float randomRange = 20f;
    List<AudioClip> clipsRandomUnplayed;
    Queue<AudioClip> clipsToPlay;
    int posterToPlayIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        clipsRandomUnplayed = new List<AudioClip>(clipsRandom);
        clipsToPlay = new Queue<AudioClip>();
        StartCoroutine(PlayRandomAudio());
    }

    private void Update()
    {
        if(clipsToPlay.Count > 0 && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clipsToPlay.Dequeue());
        }
    }

    void AddPlayCommand(AudioClip clip)
    {
        clipsToPlay.Enqueue(clip);
    }

    IEnumerator PlayRandomAudio()
    {
        while(true)
        {
            float waitSeconds = randomFrequency + Random.Range(-randomRange, randomRange);
            yield return new WaitForSeconds(waitSeconds);            
            if (clipsRandomUnplayed.Count > 0)
            {
                Debug.Log("Tannoy: Random");
                AudioClip clip = clipsRandomUnplayed[Random.Range(0, clipsRandomUnplayed.Count)];
                //audioSource.PlayOneShot(clip);
                AddPlayCommand(clip);
                clipsRandomUnplayed.Remove(clip);
            }
        }
    }

    public void PlayGameOver()
    {
        Debug.Log("Tannoy: GameOver");
        if (clipsGameover.Length > 0)
            //audioSource.PlayOneShot(clipsGameover[Random.Range(0, clipsGameover.Length)]);
            AddPlayCommand(clipsGameover[Random.Range(0, clipsGameover.Length)]);
    }

    public void PlayPhotoFound(int index)
    {
        if(index < clipsPhotoFound.Length)
        {
            Debug.Log("Tannoy: Photo " + index);
            if (clipsPhotoFound.Length > 0)
                //audioSource.PlayOneShot(clipsPhotoFound[index]);
                AddPlayCommand(clipsPhotoFound[index]);
        }
    }

    public void PlayPosterFound()
    {
        if(posterToPlayIndex < clipsPosterFound.Length)
        {
            AddPlayCommand(clipsPosterFound[posterToPlayIndex++]);
        }
    }

    public void PlayTannoyAudio(AudioClip clip)
    {
        Debug.Log("Play Tannoy " + clip.name);
        //audioSource.PlayOneShot(clip);
        AddPlayCommand(clip);
    }

    
}
