using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioEffectPlayer : MonoBehaviour
{
    public AudioClip[] Effects;

    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlayEffect(int clipIndex)
    {
        if (Effects.Length == 0) Debug.LogError(name + " contains no audioclips in effects array");
        if (clipIndex >= Effects.Length) Debug.LogError("Index " + clipIndex + " is out of range");

        AudioSource.clip = Effects[clipIndex];
        AudioSource.Play();
    }
}
