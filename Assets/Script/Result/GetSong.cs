using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GetSong : MonoBehaviour
{
	public AudioSource bgm;
	public VideoPlayer video;
	public GameObject text;
	public static string songName;
	void Start()
	{
		StartCoroutine(Play());
		ShowText();
	}
	private IEnumerator Play()
	{
		yield return new WaitForSeconds(0.6f);
		bgm.Play();
		video.Play();
		yield return new WaitForSeconds(bgm.clip.length + 1.2f);
		Transition.scene = "Select";
		SceneManager.LoadScene("Transition");
	}
	private void ShowText()
	{
		text.GetComponent<Text>().text = "获得新歌曲 : " + songName;
	}
}
