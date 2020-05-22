using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ReadList : List
{
	/// <summary>
	/// 难度
	/// </summary>
	public static int[] diff = new int[3];
	/// <summary>
	/// 等级
	/// </summary>
	public Text[] levels = new Text[3];
	/// <summary>
	/// 歌曲名称
	/// </summary>
	public Text songName;
	/// <summary>
	/// 歌曲作者
	/// </summary>
	public Text songArtist;
	/// <summary>
	/// 难度按钮
	/// </summary>
	public GameObject[] levelButtons = new GameObject[3];
	/// <summary>
	/// 父对象
	/// </summary>
	public Transform Content;
	/// <summary>
	/// 按钮预置
	/// </summary>
	public GameObject btn;
	/// <summary>
	/// 选择的歌曲
	/// </summary>
	public static Song SelectedSong;
	/// <summary>
	/// 歌曲封面
	/// </summary>
	public SpriteRenderer title;
	/// <summary>
	/// 单例
	/// </summary>
	public static ReadList _instance;
	/// <summary>
	/// 预览音乐
	/// </summary>
	public AudioSource music;
	private void Awake()
	{
		//限制帧数
		//QualitySettings.vSyncCount = 0;
		//Application.targetFrameRate = 144;
	}
	void Start()
	{
		_instance = this;//创建单例
		if (Preference.isFirst)//如果第一次进select场景
		{
			GetList();
		}
		ShowItem();
		Preference.isFirst = false;
		SelectedSong = songList[0];//默认选择歌曲为第一个
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
	/// <summary>
	/// 播放音频
	/// </summary>
	/// <param name="song">当前歌曲</param>
	public void PlayAudio(Song song)
	{
		AudioClip clip = Resources.Load<AudioClip>("Songs/" + song.Path + "/preview");
        if (!SceneManager.GetActiveScene().name.Equals("SelectEX"))
        {
			music.clip = clip;
			music.Play();
		}
	}
	/// <summary>
	/// 显示右侧歌曲列表
	/// </summary>
	private void ShowItem()
	{
		Selectable sl = null;//按钮继承自Selectable
		Navigation n = new Navigation
		{
			mode = Navigation.Mode.Explicit
		};//调整Navigation.Mode至手动
		List<Song> songs = songList.Where(s => Settings.playerLevel >= s.UnlockLevel
			&& (s.Ez != 0 || s.Adv != 0 || s.Hd != 0)).ToList();//从songList中找到低于当前用户等级的歌曲
		for (int i = 0; i < songs.Count; i++)
		{
			Song song = songs[i];
			GameObject button = Instantiate(btn) as GameObject;
			button.transform.SetParent(Content, false);//设置父对象
			button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//设置大小
			button.GetComponent<SongSelect>().text.text = song.Name;
			button.GetComponent<SongSelect>().song = song;
			if (songs.Count >= 2 && i == songs.Count - 2)//把倒数第二个按钮赋给sl
			{
				sl = button.GetComponent<Button>();
			}
			if (songs.Count >= 1 && i == songs.Count - 1)//最后一个的navigation调整至手动，selectOnUp为sl
			{
				n.selectOnUp = sl;
				button.GetComponent<Button>().navigation = n;
			}
		}
	}
	/// <summary>
	/// 显示等级
	/// </summary>
	private void ShowLevel()
	{
		for (int i = 0; i < 3; i++)
		{
			if (diff[i] > 0)//如果列表当中等级大于0，则显示等级
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
	/// <summary>
	/// 显示歌曲详细信息
	/// </summary>
	/// <param name="song">当前歌曲</param>
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
	/// <summary>
	/// 得到歌曲的封面
	/// </summary>
	/// <param name="song">当前歌曲</param>
	private void GetTitlePic(Song song)
	{
		try
		{
			title.sprite = Resources.Load<Sprite>("Songs/" + song.Path + "/title");
		}
		catch (Exception)
		{
			Debug.LogError("Title pic not exist. ");
		}
	}
}

