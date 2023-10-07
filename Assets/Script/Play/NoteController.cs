using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
	public static float noteDropTime = 0.65f;
	public GameObject noteWhite;
	public GameObject noteBlue;
	public GameObject noteLongWhite;
	public GameObject noteLongBlue;
	public List<NoteAsset> noteList = new List<NoteAsset>();
	public Dictionary<string, (string, AudioClip)> soundList = new();
	public static NoteController _instance;
	public static float perfectTime = 0.04f;
	public static float greatTime = 0.065f;
	public static float goodTime = 0.1f;
	public static float badTime = 0.15f;
	public static float fixedTime = 0.015f;
	public GameObject audioPlayer;
	private void Awake()
	{
		_instance = this;
	}
	void Start()
	{
		GetFirstNotes();
		foreach (NoteAsset noteAsset in noteList)
		{
			StartCoroutine(CreateNotes(noteAsset));
		}
	}
	IEnumerator CreateNotes(NoteAsset note)
	{
		yield return new WaitForSeconds(note.Time);
		if (note.Dur == 0)
		{
			if (note.Pos % 2 == 0)
			{
				Instantiate(noteWhite).GetComponent<TapController>().note = note;
			}
			else
			{
				Instantiate(noteBlue).GetComponent<TapController>().note = note;
			}
		}
		else
		{
			if (note.Pos % 2 == 0)
			{
				Instantiate(noteLongWhite).GetComponent<HoldController>().note = note;
			}
			else
			{
				Instantiate(noteLongBlue).GetComponent<HoldController>().note = note;
			}
		}
		yield break;
	}
	public void GetFirstNotes()
	{
		for (int i = 0; i < 5; i++)
		{
			int firstIndex = noteList.FindIndex(note1 => note1.Pos == i);
			if (firstIndex != -1)
			{
				noteList[firstIndex].CanJudge = true;
			}
		}
	}
	public GameObject GetKey(NoteAsset note)
	{
		var player = Instantiate(_instance.audioPlayer);
		var soundData = _instance.soundList[note.Id];
		var audiosource = player.AddComponent<AudioSource>();
		player.AddComponent<KeySoundComponent>();
		audiosource.volume = ReadSong.vol;
		audiosource.clip = soundData.Item2;
		audiosource.playOnAwake = false;
		return player;
	}
}