using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoanAudio : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource source;
    public float randomFrequency = 3f;
    float randomRange = 1f;

    void Start()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomAudio());
    }

    IEnumerator PlayRandomAudio()
    {
        while (true)
        {
            float waitSeconds = randomFrequency + Random.Range(-randomRange, randomRange);
            yield return new WaitForSeconds(waitSeconds);
            if (clips.Length > 0)
            {
                source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            }
        }
    }
}
