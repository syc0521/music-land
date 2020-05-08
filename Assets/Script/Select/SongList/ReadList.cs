using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ReadList : List
{
	public static int[] diff = new int[3];
	public Text[] levels = new Text[3];
	public Text songName;
	public Text songArtist;
	public GameObject[] levelButtons = new GameObject[3];
	public Transform Content;
	public GameObject btn;
	public static Song SelectedSong;
	public SpriteRenderer title;
	public SpriteRenderer mask;
	public static float a;
	public static ReadList _instance;
	public AudioSource music;

	void Start()
	{
		_instance = this;
		a = 0;
		if (Preference.isFirst)
		{
			GetList();
		}
		ShowItem();
		Preference.isFirst = false;
		SelectedSong = songList[0];
		diff[0] = SelectedSong.Ez;
		diff[1] = SelectedSong.Adv;
		diff[2] = SelectedSong.Hd;
		ShowLevel();
		PlayAudio(SelectedSong);
	}
	private void Update()
	{
		ShowLevel();
		ShowSong(SelectedSong);
	}
	public void PlayAudio(Song song)
	{
		AudioClip clip = Resources.Load<AudioClip>("Songs/" + song.Path + "/preview");
		music.clip = clip;
		music.Play();
	}

	private void ShowItem()
	{
		Selectable sl = null;
		Navigation n = new Navigation
		{
			mode = Navigation.Mode.Explicit
		};
		List<Song> songs = songList.Where(s => Settings.playerLevel >= s.UnlockLevel
			&& (s.Ez != 0 || s.Adv != 0 || s.Hd != 0)).ToList();
		for (int i = 0; i < songs.Count; i++)
		{
			Song song = songs[i];
			GameObject button = Instantiate(btn) as GameObject;
			button.transform.SetParent(Content, false);
			button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			button.GetComponent<SongSelect>().text.text = song.Name;
			button.GetComponent<SongSelect>().song = song;
			if (songs.Count >= 2)
			{
				if (i == songs.Count - 2)
				{
					sl = button.GetComponent<Button>();
				}
			}
			if (songs.Count >= 1)
			{
				if (i == songs.Count - 1)
				{
					n.selectOnUp = sl;
					button.GetComponent<Button>().navigation = n;
				}
			}
		}
	}
	private void ShowLevel()
	{
		for (int i = 0; i < 3; i++)
		{
			if (diff[i] > 0)
			{
				levels[i].enabled = true;
				levelButtons[i].SetActive(true);
			}
			else
			{
				levels[i].enabled = false;
				levelButtons[i].SetActive(false);
			}
			levels[i].text = Convert.ToString(diff[i]);
		}
	}
	private void ShowSong(Song song)
	{
		try
		{
			songName.text = song.Name;
			songArtist.text = song.Artist;
			GetTitlePic(song);
		}
		catch (Exception)
		{
			songName.text = "";
			songArtist.text = "";
		}
	}

	private void GetTitlePic(Song song)
	{
		try
		{
			title.sprite = Resources.Load<Sprite>("Songs/" + song.Path + "/title");
		}
		catch (Exception)
		{
			throw;
		}
	}
}

