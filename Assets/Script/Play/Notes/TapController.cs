using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static NoteController;

public class TapController : Notes
{
	public NoteAsset note;
	public GameObject effect;
	private GameObject audioPlayer;
	private bool isDestroyed = false;
	private bool isJudged = false;
	private AudioSource key;
	[NonSerialized]
	public bool isNotePlayed = false;
	private bool jk0 = false;
	private bool jk1 = false;
	private bool jk2 = false;
	
	void Start()
	{
		audioPlayer = _instance.GetKey(note);
		key = audioPlayer.GetComponent<AudioSource>();
		float basePos = 1.345f;
		transform.position = new Vector3(basePos * (note.Pos - 2), 5.6f);
	}

	void Update()
	{
		DropNote();
		if (!isJudged)
		{
			if (!ReadSong.isAutoPlay)
			{
				UserPlayMode();
			}
			else
			{
				AutoPlayMode();
			}
		}
	}

	public override void DropNote()
	{
		if (Time.timeSinceLevelLoad < note.Time + noteDropTime)
		{
			transform.position = new Vector3(transform.position.x,
				Utils.Lerp(Time.timeSinceLevelLoad, note.Time, noteDropTime + note.Time, 5.6f, -3.15f));
		}
		else if (!isDestroyed)
		{
			isDestroyed = true;
			StartCoroutine(DestroyNote());
		}
	}

	public override void AutoPlayMode()
	{
		if (Time.timeSinceLevelLoad >= note.Time + noteDropTime)
		{
			isJudged = true;
			isDestroyed = true;
			StartCoroutine(DestroyNote());
			if (!isNotePlayed)
			{
				StartCoroutine(PlaySingleKey(key));
			}
			CreateFX(PERFECT);
			InputController.combo++;
			JudgeStatistics.perfect++;
			InputController._instance.ShowJudge(PERFECT);
			InputController._instance.ShowCombo(PERFECT);
		}
	}

	private void UserPlayMode()
	{
		bool isFirstNote = false;
		NoteAsset previous = InputController._instance.GetPreviousNote(note);
		if (previous == null)
		{
			isFirstNote = true;
		}
		if (Time.timeSinceLevelLoad <= note.Time + noteDropTime + goodTime &&
			Time.timeSinceLevelLoad >= note.Time + noteDropTime - badTime)
		{
			if (note.CanJudge && ((!previous.CanJudge) || !isFirstNote))
			{
				int returnType = NoteJudge(note);
				if (returnType != POOR && returnType != BAD)
				{
					isJudged = true;
					isDestroyed = true;
					CreateFX(returnType);
					InputController.combo++;
					InputController._instance.ShowJudge(returnType);
					InputController._instance.ShowCombo(returnType);
					InputController._instance.ShowFastSlow(returnType);
					StartCoroutine(DestroyNote());
				}
				else if (returnType == BAD)
				{
					InputController._instance.ShowCombo(returnType);
					InputController._instance.ShowFastSlow(returnType);
				}
			}
		}
		else if (Time.timeSinceLevelLoad > note.Time + noteDropTime + goodTime)
		{
			isJudged = true;
			isDestroyed = true;
			InputController.combo = 0;
			JudgeStatistics.poor++;
			if (JudgeStatistics.life >= 8)
			{
				JudgeStatistics.life -= 8f;
			}
			InputController._instance.ShowJudge(POOR);
			InputController._instance.ShowCombo(POOR);
			InputController._instance.ShowFastSlow(POOR);
			Debug.Log(note + "poor");
		}
	}
	public override void CreateFX(int returnType)
	{
		if (returnType < 3)
		{
			GameObject judgeFX = Instantiate(effect) as GameObject;
			judgeFX.transform.position = new Vector3(transform.position.x + 0.4f, -3.44f);
		}
	}
	private IEnumerator DestroyNote()
	{
		if (!isJudged)
		{
			transform.position = new Vector3(transform.position.x, -3.25f);
			yield return new WaitForSeconds(goodTime);
		}
		else
		{
			note.CanJudge = false;
		}
		NoteAsset nextNote = InputController._instance.GetNextNote(note);
		yield return new WaitForSeconds(0.03f);
		if (nextNote != null)
		{
			nextNote.CanJudge = true;
		}
		DestroyImmediate(gameObject);
		yield break;
	}
	public override int NoteJudge(NoteAsset note)
	{
		AudioSource audio = key;
		int pos = note.Pos;
		if (InputController._instance.isControllerConnected)
		{
			if (Input.GetAxis("LeftRight") < 0 && pos == 0 && note.CanJudge)
			{
				if (!jk0)
				{
					if (!isNotePlayed)
					{
						StartCoroutine(PlaySingleKey(audio));
					}
					int i = JudgeNote(note);
					if (i != BAD)
					{
						return i;
					}
				}
				jk0 = true;
			}
			else
			{
				jk0 = false;
			}
			if (Input.GetAxis("LeftRight") > 0 && pos == 2 && note.CanJudge)
			{
				if (!jk2)
				{
					if (!isNotePlayed)
					{
						StartCoroutine(PlaySingleKey(audio));
					}
					int i = JudgeNote(note);
					if (i != BAD)
					{
						return i;
					}
				}
				jk2 = true;
			}
			else
			{
				jk2 = false;
			}
			if (Input.GetAxis("UpDown") > 0 && pos == 1 && note.CanJudge)
			{
				if (!jk1)
				{
					if (!isNotePlayed)
					{
						StartCoroutine(PlaySingleKey(audio));
					}
					int i = JudgeNote(note);
					if (i != BAD)
					{
						return i;
					}
				}
				jk1 = true;
			}
			else
			{
				jk1 = false;
			}
			if ((Utils.KeyJudge(KeyCode.JoystickButton2, note) && pos == 2) ||
				(Utils.KeyJudge(KeyCode.JoystickButton3, note) && pos == 3) ||
				(Utils.KeyJudge(KeyCode.JoystickButton1, note) && pos == 4))
			{
				if (!isNotePlayed)
				{
					Debug.Log("play");
					StartCoroutine(PlaySingleKey(audio));
				}
				return JudgeNote(note);
			}
		}
		else
		{
			if ((Utils.KeyJudge(KeyCode.D, note) && pos == 0) || (Utils.KeyJudge(KeyCode.F, note) && pos == 1) ||
				(Utils.KeyJudge(KeyCode.Space, note) && pos == 2) || (Utils.KeyJudge(KeyCode.J, note) && pos == 3) ||
				(Utils.KeyJudge(KeyCode.K, note) && pos == 4))
			{
				int i = JudgeNote(note);
				if (!isNotePlayed && i != BAD)
				{
					Debug.Log("play");
					StartCoroutine(PlaySingleKey(audio));
				}
				return i;
			}
		}
		return -1;
	}
	public IEnumerator PlaySingleKey(AudioSource audio)
	{
		if (!isNotePlayed)
		{
			audio.enabled = true;
			yield return new WaitForSeconds(ReadSong.delay);
			audio.Play();
			Debug.Log("audio is playing");
			isNotePlayed = true;
		}
		yield break;
	}
}