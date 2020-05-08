using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReadListEX : List
{
	private readonly List<Song> songListEX = new List<Song>();
	public static int exlevel;
	public Text level;
	public Text songName;
	public Text songArtist;
	public GameObject levelButton;
	public Transform Content;
	public GameObject btn;
	public static Song SelectedSong;
	public SpriteRenderer title;

	void Start()
	{
		GetListEX();
		ShowItem();
		Preference.isEXFirst = false;
		SelectedSong = songListEX[0];
		exlevel = SelectedSong.Ex;
		ShowLevel();
	}
	private void Update()
	{
		ShowLevel();
		ShowSong(SelectedSong);
	}
	private void GetListEX()
	{
		songListEX.AddRange(songList.Where(song => song.Ex > 0));
	}

	private void ShowItem()
	{
		Selectable sl = null;
		Navigation n = new Navigation
		{
			mode = Navigation.Mode.Explicit
		};
		for (int i = 0; i < songListEX.Count; i++)
		{
			Song song = songListEX[i];
			GameObject button = Instantiate(btn) as GameObject;
			button.transform.SetParent(Content, false);
			button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
			button.GetComponent<SongSelect>().text.text = song.Name;
			button.GetComponent<SongSelect>().song = song;
			if (songListEX.Count >= 2)
			{
				if (i == songListEX.Count - 2)
				{
					sl = button.GetComponent<Button>();
				}
			}
			if (songListEX.Count >= 1)
			{
				if (i == songListEX.Count - 1)
				{
					n.selectOnUp = sl;
					button.GetComponent<Button>().navigation = n;
				}
			}
		}
	}
	private void ShowLevel()
	{
		if (exlevel > 0)
		{
			level.enabled = true;
			levelButton.SetActive(true);
		}
		else
		{
			level.enabled = false;
			levelButton.SetActive(false);
		}
		level.text = Convert.ToString(exlevel);
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

