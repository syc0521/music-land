using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileController : MonoBehaviour
{
	public static Song song;
	public static bool isEX;
	public SpriteRenderer grade;
	public SpriteRenderer bgTitle;
	public Sprite[] grades = new Sprite[7];
	public Text songName;
	public Text songLevel;
	public Text score;
	public Text rate;
	public Text[] judges = new Text[6];
	public PlayableDirector mask;
	//S 95+ A 90+ B 80+ C 70+ D 60+ E 50+ F 50-
	void Start()
	{
		Settings.playerLevel++;
		SetGrade(JudgeStatistics.realRate / 100.0f);
		SetJudge();
		GetTitlePic(song);
		GetSongInformation();
	}

	private void GetSongInformation()
	{
		songName.text = song.Name;
		int diff = ReadSong.diff;
		int[] level = { song.Ez, song.Adv, song.Hd, song.Ex };
		Dictionary<int, string> dic = new Dictionary<int, string>
		{
			{ 0, "easy" },
			{ 1, "advanced" },
			{ 2, "hard" },
			{ 3, "expert" }
		};
		songLevel.text = dic[diff] + " lv." + level[diff].ToString();
	}

	private void SetJudge()
	{
		score.text = JudgeStatistics.score.ToString() + " / " + JudgeStatistics.totalScore;
		rate.text = Math.Round(JudgeStatistics.realRate, 2).ToString() + "%";
		judges[0].text = JudgeStatistics.perfect.ToString();
		judges[1].text = JudgeStatistics.great.ToString();
		judges[2].text = JudgeStatistics.good.ToString();
		judges[3].text = JudgeStatistics.bad.ToString();
		judges[4].text = JudgeStatistics.poor.ToString();
		judges[5].text = JudgeStatistics.maxCombo.ToString();
	}

	private void SetGrade(float rate)
	{
		if (rate >= 0.95f)
		{
			grade.sprite = grades[0];
		}
		else if (rate >= 0.9f)
		{
			grade.sprite = grades[1];
		}
		else if (rate >= 0.8f)
		{
			grade.sprite = grades[2];
		}
		else if (rate >= 0.7f)
		{
			grade.sprite = grades[3];
		}
		else if (rate >= 0.6f)
		{
			grade.sprite = grades[4];
		}
		else if (rate >= 0.5f)
		{
			grade.sprite = grades[5];
		}
		else
		{
			grade.sprite = grades[6];
		}
	}

	private void GetTitlePic(Song song)
	{
		try
		{
			bgTitle.sprite = Resources.Load<Sprite>("Songs/" + song.Path + "/title");
		}
		catch (Exception)
		{
			throw;
		}
	}
	private void LoadGetSong()
	{
		SceneManager.LoadScene("GetSong");
	}	
	private void LoadGetMode()
	{
		SceneManager.LoadScene("GetMode");
	}
	private void LoadSelect()
	{
		SceneManager.LoadScene("Transition");
	}
	public void JumpScene()
	{
		switch (Settings.playerLevel)
		{
			case 1:
				mask.Play();
				GetSong.songName = "End of the Moonlight";
				Invoke("LoadGetSong", 0.75f);
				break;
			case 2:
				mask.Play();
				GetSong.songName = "Endymion";
				Invoke("LoadGetMode", 0.75f);
				break;
			default:
				if (isEX)
				{
					mask.Play();
					Transition.scene = "SelectEX";
					Invoke("LoadSelect", 0.75f);
				}
				else
				{
					mask.Play();
					Transition.scene = "Select";
					Invoke("LoadSelect", 0.75f);
				}
				break;
		}
		
	}
}
