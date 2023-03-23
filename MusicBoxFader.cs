using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxFader : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeIn(2));
    }

    public void TriggerFadeOut()
    {
        StartCoroutine(FadeOut(1));
    }

    public void TriggerFadeIn()
    {
        StartCoroutine(FadeIn(2));
    }

    public IEnumerator FadeOut (float FadeTime) {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.volume = 0;
    }

    public IEnumerator FadeIn (float FadeTime) {
        float startVolume = audioSource.volume;
        audioSource.volume = 0;
        while (audioSource.volume < startVolume) {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
