using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBootSync : MonoBehaviour
{
    public List<AudioClip> clips;
    private AudioSource audioS;

    public void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        audioS.PlayOneShot(clips[Random.Range(0,clips.Count)]);
        Debug.Log("I should be making noise!");
    }
}
