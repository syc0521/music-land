using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GetMode : MonoBehaviour
{
	public AudioSource bgm;
	public VideoPlayer video;
	void Start()
	{
		StartCoroutine(Play());
	}
	private IEnumerator Play()
	{
		yield return new WaitForSeconds(0.6f);
		bgm.Play();
		video.Play();
		yield return new WaitForSeconds(bgm.clip.length + 1.3f);
		SceneManager.LoadScene("GetSong");
	}
}
