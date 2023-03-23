using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private List<string> SoundQue = new List<string>();
    private AudioSource _source;

    void Awake()
    {
        instance = this;

        _source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip sound)
    {
        if(!SoundQue.Contains(sound.name))
        {
            StartCoroutine(SoundQueCoroutine(sound.name));
            _source.PlayOneShot(sound);
        }
        
        //SoundManager.instance.PlaySound(sound); // this line will play any sound from any script. nice
    }

    private IEnumerator SoundQueCoroutine(string nameOfSound)
    {
        SoundQue.Add(nameOfSound);
        yield return new WaitForSeconds(.1f);
        SoundQue.Remove(nameOfSound);
    }
}
