using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXBGM : MonoBehaviour
{
    public AudioClip loop;
    void Start()
    {
        StartCoroutine(ChangeMusic());
    }
    private IEnumerator ChangeMusic()
    {
        AudioSource audio = GetComponent<AudioSource>();
        yield return new WaitForSeconds(audio.clip.length);
        audio.clip = loop;
        audio.Play();
    }
}
