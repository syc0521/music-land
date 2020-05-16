using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour
{
	/// <summary>
	/// 歌曲列表
	/// </summary>
	public static List<Song> songList = new List<Song>();
	/// <summary>
	/// 把当前歌曲传到其他文件
	/// </summary>
	/// <param name="song">选择的歌曲</param>
	public static void PassSong(Song song)
	{
		ReadSong.song = song;
		FileController.song = song;
		FileController.isEX = false;
		ReadSong.vol = Settings.vol;
		LoadScene.song = song;
	}
	/// <summary>
	/// 从磁盘获取歌曲列表
	/// </summary>
	public void GetList()
	{
		try
		{
			TextAsset text = Resources.Load<TextAsset>("SongList");
			string[] lines = text.text.Split('\n');//按行分割
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i] != "")//如果不是空行
				{
					string[] line = lines[i].Split(',');//按逗号分割，存到line数组里
					Song song = new Song(line[0], line[1], line[2], int.Parse(line[3]),
						int.Parse(line[4]), int.Parse(line[5]), int.Parse(line[6]), int.Parse(line[7]));
					//创建song对象并添加到songList当中
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
