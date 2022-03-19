using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;
    Dictionary<AudioClip, float> audioLatencyMap;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioLatencyMap = new Dictionary<AudioClip, float>();
    }

    private void Update()
    {
        foreach(var key in audioLatencyMap.Keys.ToList())
        {
            audioLatencyMap[key] -= Time.deltaTime;
            if (audioLatencyMap[key] < 0.0f) audioLatencyMap[key] = 0.0f;
        }
    }

    public void PlayOneShot(AudioClip audioClip, float volume = 1.0f)
    {
        if (!audioLatencyMap.ContainsKey(audioClip))
        {
            audioLatencyMap[audioClip] = 0.0f;
        }
        volume *= 0.075f / (audioLatencyMap[audioClip] + 0.075f);
        StartCoroutine(PlayOneShotCoroutine(audioClip, volume, audioLatencyMap[audioClip]));
        audioLatencyMap[audioClip] += 0.015f;
    }

    IEnumerator PlayOneShotCoroutine(AudioClip audioClip, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(audioClip,volume);
    }
}
