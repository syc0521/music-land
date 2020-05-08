using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour
{
	public static List<Song> songList = new List<Song>();
	public static void PassSong(Song song)
	{
		ReadSong.song = song;
		FileController.song = song;
		FileController.isEX = false;
		ReadSong.vol = Settings.vol;
		LoadScene.song = song;
	}
	public void GetList()
	{
		try
		{
			TextAsset text = Resources.Load<TextAsset>("SongList");
			string[] lines = text.text.Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i] != "")
				{
					string[] line = lines[i].Split(',');
					Song song = new Song(line[0], line[1], line[2], int.Parse(line[3]),
						int.Parse(line[4]), int.Parse(line[5]), int.Parse(line[6]), int.Parse(line[7]));
					songList.Add(song);
				}
			}
		}
		catch (Exception)
		{
			Debug.LogError("Songlist not exist.");
		}
	}

}
