using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AlbumButton : MonoBehaviour
{
	public Text songName;
	public Text songArtist;
	public Song song;
	public Image songImage;

	void Start()
	{
		GetTitlePic(song);
	}
	private void GetTitlePic(Song song)
	{
		try
		{
			songImage.sprite = Resources.Load<Sprite>("Songs/" + song.Path + "/title");
		}
		catch (Exception)
		{
			throw;
		}
	}
	public void PlaySong()
	{
		AlbumController._instance.PlayMask();
		List.PassSong(song);
		Invoke("GoToScene", 0.65f);
	}
	private void GoToScene()
	{
		SceneManager.LoadScene("Decide");
	}
}
