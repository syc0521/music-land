using System;
using System.Collections;
using UnityEngine;

public class KeySoundComponent : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _length;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _length = _audioSource.clip.length;
    }

    public void Play()
    {
        _audioSource.Play();
        StartCoroutine(DestroyKeySound(_length));
    }

    private IEnumerator DestroyKeySound(float length)
    {
        yield return new WaitForSeconds(length);
        Destroy(gameObject);
    }
}